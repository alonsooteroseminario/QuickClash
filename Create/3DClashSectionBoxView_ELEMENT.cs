using Autodesk.Revit.ApplicationServices;
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
	public static class ClashSectionBoxView_ELEMENT
	{

		public static void Do(ExternalCommandData commandData)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			//Document doc = this.ActiveUIDocument.Document;
			//UIDocument uidoc = new UIDocument(doc);

			List<Element> clash_elements = new List<Element>();

			foreach (Element e in DYNO_GetAllClashElements_OnlyActiveView(commandData))
			{
				clash_elements.Add(e);
			}
			// get list of all levels
			//IList<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(l => l.Elevation).ToList();
			if (clash_elements.Count() == 0)
			{
				TaskDialog.Show("Dynoscript", "No se encontraron Elementos con Clash en la Vista Activa!");
				return;
			}
			else
			{
				// get a ViewFamilyType for a 3D View
				ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
												 OfClass(typeof(ViewFamilyType)).
												 Cast<ViewFamilyType>()
												 where v.ViewFamily == ViewFamily.ThreeDimensional
												 select v).First();

				List<View3D> tresDclashview = new List<View3D>();

				using (Transaction t = new Transaction(doc, "Create clash 3d view"))
				{
					int ctr = 0;
					// loop through all Elements
					foreach (Element elem in clash_elements)
					{
						t.Start();

						// Create the 3d view
						View3D clashview = View3D.CreateIsometric(doc, viewFamilyType.Id);

						Parameter param = elem.LookupParameter("Clash Category");

						string param_string = param.AsString();
						string param_string2 = param_string.Replace(':', '_');

						// Set the name of the view
						clashview.Name = "COORD - Section Box  " + elem.Name.ToString() + " / "
																+ "ID  " + elem.Id.ToString() + " / "
																+ " Clash Category " + param_string2;




						// Set the name of the transaction
						// A transaction can be renamed after it has been started
						t.SetName("Create view " + clashview.Name);

						// Create a new BoundingBoxXYZ to define a 3D rectangular space
						BoundingBoxXYZ elem_bb = elem.get_BoundingBox(null);




						double offset = 2;

						var SectionBox = clashview.GetSectionBox();
						var vMax = SectionBox.Max + SectionBox.Transform.Origin;
						var vMin = SectionBox.Min + SectionBox.Transform.Origin;
						var bbMax = elem_bb.Max; //Point
						var bbMin = elem_bb.Min; //Point




						double Max_X = elem_bb.Max.X;


						double Max_Y = elem_bb.Max.Y;


						double Max_Z = elem_bb.Max.Z;



						double Min_X = elem_bb.Min.X;


						double Min_Y = elem_bb.Min.Y;


						double Min_Z = elem_bb.Min.Z;



						XYZ Max = new XYZ(Max_X + offset, Max_Y + offset, Max_Z + offset);
						XYZ Min = new XYZ(Min_X - offset, Min_Y - offset, Min_Z - offset);

						BoundingBoxXYZ myBox = new BoundingBoxXYZ();

						myBox.Min = Min;
						myBox.Max = Max;

						clashview.SetSectionBox(myBox);

						//					if (!clashview.IsTemplate) 
						//					{
						clashview.DisplayStyle = DisplayStyle.Shading;
						clashview.DetailLevel = ViewDetailLevel.Fine;
						//					}

						t.Commit();

						// Open the just-created view
						// There cannot be an open transaction when the active view is set
						tresDclashview.Add(clashview);

						ctr++;
					}
					uidoc.ActiveView = tresDclashview.First();
					DYNO_CreateClashFilterMultipleElementsInMultipleViews_UI(commandData, tresDclashview);
					DYNO_CreateClashSOLVEDFilterMultipleElementsInMultipleViews_UI(commandData, tresDclashview);
				}
			}


		}

		public static void DYNO_CreateClashFilterMultipleElementsInMultipleViews_UI(ExternalCommandData commandData, List<View3D> lista_3dviews) // Crea filtros en la vista activa
		{
			//Get application and document objects
			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;

			// Get Active View
			var activeView = uidoc.ActiveView;

			FilteredElementCollector elementss = new FilteredElementCollector(doc);
			FillPatternElement solidFillPattern = elementss.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);

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



			foreach (View3D view3d in lista_3dviews)
			{

				//View activeView = this.ActiveUIDocument.ActiveView;
				activeView = view3d;

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


		}// Crea filtros en la lista de vistas ingresadas

		public static void DYNO_CreateClashSOLVEDFilterMultipleElementsInMultipleViews_UI(ExternalCommandData commandData, List<View3D> lista_3dviews) // Crea filtros en la vista activa
		{
			//Get application and document objects
			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;

			// Get Active View
			var activeView = uidoc.ActiveView;

			FilteredElementCollector elementss = new FilteredElementCollector(doc);
			FillPatternElement solidFillPattern = elementss.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);

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



			foreach (View3D view3d in lista_3dviews)
			{

				//View activeView = this.ActiveUIDocument.ActiveView;
				activeView = view3d;

				using (Transaction ta = new Transaction(doc, "create clash solved filter view"))
				{
					ta.Start();

					//activeView.Name = "COORD";
					List<ParameterFilterElement> lista_ParameterFilterElement = new List<ParameterFilterElement>();
					List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();

					FilteredElementCollector collector = new FilteredElementCollector(doc);

					Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");

					FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));

					List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();
					foreach (ParameterFilterElement e in collector_filterview)
					{
						lista_filtros.Add(e);
					}

					List<ElementFilter> elementFilterList = new List<ElementFilter>();

					elementFilterList.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id, (int)1))); // Clash Solved , EQUAL,  False(int=0),


					for (int i = 0; i < lista_filtros.Count(); i++)
					{
						if (lista_filtros[i].Name == "CLASH SOLVED FILTER")
						{
							lista_ParameterFilterElement.Add(lista_filtros[i]);
							i = lista_filtros.Count();
							break;
						}

					}

					if (lista_ParameterFilterElement.Count() == 0)
					{
						ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH SOLVED FILTER", cats, elementFilterList.First());
						lista_ParameterFilterElement.Add(parameterFilterElement);
					}

					ParameterFilterElement aa = lista_ParameterFilterElement.First();

					OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
					ogs3.SetProjectionLineColor(new Color(192, 192, 192));
					ogs3.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
					ogs3.SetSurfaceForegroundPatternVisible(true);
					ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);

					activeView.AddFilter(aa.Id);
					activeView.SetFilterOverrides(aa.Id, ogs3);

					ta.Commit();
				}
			}


		}// Crea filtros en la lista de vistas ingresadas

		public static List<Element> DYNO_GetAllClashElements_OnlyActiveView(ExternalCommandData commandData)
		{
			//Get application and document objects
			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;

			// Get Active View
			var activeView = uidoc.ActiveView;
			// FAMILY INSTANCES

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
					    BuiltInCategory.OST_FlexDuctCurves,
						BuiltInCategory.OST_FlexPipeCurves,
						BuiltInCategory.OST_PipeFitting,
						BuiltInCategory.OST_PlumbingFixtures,
						BuiltInCategory.OST_SpecialityEquipment,
						BuiltInCategory.OST_Sprinklers,
						BuiltInCategory.OST_Wire,
				};

			// get elements with "clash" parameter value == "YES"
			List<Element> clash = new List<Element>();
			// get elements with "clash" parameter value == "NO"
			List<Element> clash_no = new List<Element>();

			foreach (BuiltInCategory bic in bics_familyIns)
			{
				ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
				// Create a category filter for MechanicalEquipment
				ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
				// Create a logic And filter for all MechanicalEquipment Family
				LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
				// Apply the filter to the elements in the active document
				FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
				IList<Element> mechanicalequipment = MEcoll.WherePasses(MEInstancesFilter).ToElements();



				foreach (Element elem in mechanicalequipment)
				{
					if (elem.LookupParameter("Clash").AsString() == "YES")
					{
						clash.Add(elem);
					}
					else
					{
						clash_no.Add(elem);
					}
				}

			}

			// ELements
			ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
			ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
			ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
			ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
			ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
			ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));

			// Create a category filter for Ducts
			ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
			ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
			ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
			ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
			ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
			ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);


			// Create a logic And filter for all MechanicalEquipment Family
			LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);
			LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);
			LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);
			LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);
			LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);
			LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

			// Apply the filter to the elements in the active document
			FilteredElementCollector DUcoll = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements();

			FilteredElementCollector DUcoll2 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> pipes = DUcoll2.WherePasses(DUInstancesFilter2).ToElements();

			FilteredElementCollector DUcoll3 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> conduits = DUcoll3.WherePasses(DUInstancesFilter3).ToElements();

			FilteredElementCollector DUcoll4 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> cabletrays = DUcoll4.WherePasses(DUInstancesFilter4).ToElements();

			FilteredElementCollector DUcoll5 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> flexducts = DUcoll5.WherePasses(DUInstancesFilter5).ToElements();

			FilteredElementCollector DUcoll6 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> flexpipes = DUcoll6.WherePasses(DUInstancesFilter6).ToElements();


			foreach (Element elem in ducts)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in pipes)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in conduits)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in cabletrays)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in flexducts)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in flexpipes)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			return clash;

		}
	}
}
