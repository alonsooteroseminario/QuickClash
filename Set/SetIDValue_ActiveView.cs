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
    public static class SetIDValue_ActiveView
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param></param>
		public static void Do(ExternalCommandData commandData)
		{

			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;
			var activeView = uidoc.ActiveView;

			// category Duct.
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


			// category Duct.

			foreach (Element e in ducts)
			{
				Parameter param = e.LookupParameter("ID Element");

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}

			foreach (Element e in pipes)
			{
				Parameter param = e.LookupParameter("ID Element");

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}
			foreach (Element e in conduits)
			{
				Parameter param = e.LookupParameter("ID Element");

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}
			foreach (Element e in cabletrays)
			{
				Parameter param = e.LookupParameter("ID Element");

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}
			foreach (Element e in flexducts)
			{
				Parameter param = e.LookupParameter("ID Element");

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}

			foreach (Element e in flexpipes)
			{
				Parameter param = e.LookupParameter("ID Element");

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}

			// category Mechanical Equipment

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
				ElementClassFilter MEelemFilter = new ElementClassFilter(typeof(FamilyInstance));
				ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
				LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(MEelemFilter, MECategoryfilter);
				FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
				IList<Element> mechanicalequipment = MEcoll.WherePasses(MEInstancesFilter).ToElements();
				foreach (Element e in mechanicalequipment)
				{
					Parameter param = e.LookupParameter("ID Element");

					Autodesk.Revit.DB.ElementId selectedId = e.Id;
					string idString = selectedId.IntegerValue.ToString();

					using (Transaction t = new Transaction(doc, "ID Element"))
					{
						t.Start();
						param.Set(idString);
						t.Commit();
					}
				}
			}



		}
	}
}
