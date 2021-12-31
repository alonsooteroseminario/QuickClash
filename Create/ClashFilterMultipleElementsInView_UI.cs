using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickClash
{
	public static class ClashFilterMultipleElementsInView_UI
	{

		public static void Do(ExternalCommandData commandData, View3D view_3d)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

			activeView = view_3d;

			FilteredElementCollector elementss = new FilteredElementCollector(doc);
			FillPatternElement solidFillPattern = elementss.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);

			#region cats
			List<ElementId> cats = new List<ElementId>();

			cats.Add(new ElementId(BuiltInCategory.OST_DuctCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_PipeCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_Conduit));
			cats.Add(new ElementId(BuiltInCategory.OST_CableTray));
			cats.Add(new ElementId(BuiltInCategory.OST_FlexDuctCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_FlexPipeCurves));

			BuiltInCategory[] bics_familyIns = new BuiltInCategory[]
				{
					    //BuiltInCategory.OST_CableTray,
					    BuiltInCategory.OST_CableTrayFitting,
					    //BuiltInCategory.OST_Conduit,
					    BuiltInCategory.OST_ConduitFitting,
					    //BuiltInCategory.OST_DuctCurves,
					    BuiltInCategory.OST_DuctFitting,
						BuiltInCategory.OST_DuctTerminal,
						BuiltInCategory.OST_ElectricalEquipment,
						BuiltInCategory.OST_ElectricalFixtures,
						BuiltInCategory.OST_LightingDevices,
						BuiltInCategory.OST_LightingFixtures,
						BuiltInCategory.OST_MechanicalEquipment,
					    //BuiltInCategory.OST_PipeCurves,
					    //BuiltInCategory.OST_FlexDuctCurves,
					    //BuiltInCategory.OST_FlexPipeCurves,
					    BuiltInCategory.OST_PipeFitting,
						BuiltInCategory.OST_PlumbingFixtures,
						BuiltInCategory.OST_SpecialityEquipment,
						BuiltInCategory.OST_Sprinklers,
					//BuiltInCategory.OST_Wire,
				};

			foreach (BuiltInCategory bic in bics_familyIns)
			{
				cats.Add(new ElementId(bic));
			}
			#endregion


			List<ParameterFilterElement> lista_ParameterFilterElement1 = new List<ParameterFilterElement>();
			List<ParameterFilterElement> lista_ParameterFilterElement2 = new List<ParameterFilterElement>();
			List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();

			using (Transaction ta = new Transaction(doc, "create clash filter view"))
			{
				ta.Start();

				//activeView.Name = "COORD";

				FilteredElementCollector collector = new FilteredElementCollector(doc);

				Parameter param = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash");
				Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");

				FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));

				List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();


				foreach (ParameterFilterElement e in collector_filterview)
				{
					lista_filtros.Add(e);
				}

				List<ElementFilter> elementFilterList1 = new List<ElementFilter>();
				List<ElementFilter> elementFilterList_no = new List<ElementFilter>();

				FilterRule[] filterRule_lista = new FilterRule[]
				{
						ParameterFilterRuleFactory.CreateEqualsRule(param.Id,"YES", true),
						ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id,(int)0)
				};


				elementFilterList1.Add(new ElementParameterFilter(filterRule_lista));

				elementFilterList_no.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateNotContainsRule(param.Id, "YES", true)));


				for (int i = 0; i < lista_filtros.Count(); i++)
				{
					if (lista_filtros[i].Name == "CLASH YES FILTER")
					{
						lista_ParameterFilterElement1.Add(lista_filtros[i]);
						i = lista_filtros.Count();
						break;
					}

				}

				if (lista_ParameterFilterElement1.Count() == 0)
				{

					ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH YES FILTER", cats, elementFilterList1.First()); // ingresar un ElementFilter

					lista_ParameterFilterElement1.Add(parameterFilterElement);



				}

				for (int i = 0; i < lista_filtros.Count(); i++)
				{
					if (lista_filtros[i].Name == "CLASH NO FILTER")
					{
						lista_ParameterFilterElement_no.Add(lista_filtros[i]);
						i = lista_filtros.Count();
						break;
					}

				}
				if (lista_ParameterFilterElement_no.Count() == 0)
				{
					ParameterFilterElement parameterFilterElement_no = ParameterFilterElement.Create(doc, "CLASH NO FILTER", cats, elementFilterList_no.First());

					lista_ParameterFilterElement_no.Add(parameterFilterElement_no);
				}

				ParameterFilterElement aa = lista_ParameterFilterElement1.First();
				ParameterFilterElement aa_no = lista_ParameterFilterElement_no.First();

				OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
				ogs3.SetProjectionLineColor(new Color(250, 0, 0));
				ogs3.SetSurfaceForegroundPatternColor(new Color(250, 0, 0));
				ogs3.SetSurfaceForegroundPatternVisible(true);
				ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);

				OverrideGraphicSettings ogs4 = new OverrideGraphicSettings();
				ogs4.SetProjectionLineColor(new Color(192, 192, 192));
				ogs4.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
				ogs4.SetSurfaceForegroundPatternVisible(true);
				ogs4.SetSurfaceForegroundPatternId(solidFillPattern.Id);
				ogs4.SetHalftone(true);

				activeView.AddFilter(aa.Id);
				activeView.AddFilter(aa_no.Id);


				activeView.SetFilterOverrides(aa.Id, ogs3);
				activeView.SetFilterOverrides(aa_no.Id, ogs4);

				ta.Commit();
			}



		}
	}
}
