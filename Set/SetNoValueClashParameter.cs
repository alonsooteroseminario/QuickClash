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
	public static class SetNoValueClashParameter
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

			// get elements with "clash" parameter value == "YES"
			IList<Element> ductsclash = new List<Element>();
			// get elements with "clash" parameter value == "NO"
			IList<Element> ductsclash_no = new List<Element>();

			foreach (Element elem in ducts)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					ductsclash.Add(elem);
				}
				else
				{
					ductsclash_no.Add(elem);
				}
			}
			foreach (Element elem in pipes)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					ductsclash.Add(elem);
				}
				else
				{
					ductsclash_no.Add(elem);
				}
			}
			foreach (Element elem in conduits)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					ductsclash.Add(elem);
				}
				else
				{
					ductsclash_no.Add(elem);
				}
			}
			foreach (Element elem in cabletrays)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					ductsclash.Add(elem);
				}
				else
				{
					ductsclash_no.Add(elem);
				}
			}
			foreach (Element elem in flexducts)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					ductsclash.Add(elem);
				}
				else
				{
					ductsclash_no.Add(elem);
				}
			}
			foreach (Element elem in flexpipes)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					ductsclash.Add(elem);
				}
				else
				{
					ductsclash_no.Add(elem);
				}
			}

			foreach (Element e in ductsclash)
			{
				Parameter param = e.LookupParameter("Clash");
				//Parameter param2 = e.LookupParameter("Clash Solved");
				//Parameter param3 = e.LookupParameter("Clash Comments");
				Parameter param4 = e.LookupParameter("Clash Grid Location");
				Parameter param5 = e.LookupParameter("Clash Category");

				string param_value = "";

				using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
				{
					t.Start();
					param.Set(param_value);
					//param2.Set(1);
					//param3.Set(param_value);
					param4.Set(param_value);
					param5.Set(param_value);
					t.Commit();
				}
			}

			foreach (Element e in ductsclash_no)
			{
				Parameter param = e.LookupParameter("Clash");
				//Parameter param2 = e.LookupParameter("Clash Comments");
				Parameter param3 = e.LookupParameter("Clash Grid Location");
				Parameter param5 = e.LookupParameter("Clash Category");


				string param_value = "";

				using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
				{
					t.Start();
					param.Set(param_value);
					//param2.Set(param_value);
					param3.Set(param_value);
					param5.Set(param_value);
					t.Commit();
				}
			}

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
					//BuiltInCategory.OST_Wire,
				};

			// get elements with "clash" parameter value == "YES"
			IList<Element> ductfittingsclash = new List<Element>();
			// get elements with "clash" parameter value == "NO"
			IList<Element> ductfittingsclash_no = new List<Element>();

			foreach (BuiltInCategory bic in bics_familyIns)
			{
				// category ducts fittings
				ElementClassFilter DUFelemFilter = new ElementClassFilter(typeof(FamilyInstance));
				// Create a category filter for Ducts
				ElementCategoryFilter DUFCategoryfilter = new ElementCategoryFilter(bic);
				// Create a logic And filter for all MechanicalEquipment Family
				LogicalAndFilter DUFInstancesFilter = new LogicalAndFilter(DUFelemFilter, DUFCategoryfilter);
				// Apply the filter to the elements in the active document
				FilteredElementCollector DUFcoll = new FilteredElementCollector(doc, activeView.Id);
				IList<Element> ductfittings = DUFcoll.WherePasses(DUFInstancesFilter).ToElements();

				foreach (Element elem in ductfittings)
				{
					if (elem.LookupParameter("Clash").AsString() == "YES")
					{
						ductfittingsclash.Add(elem);
					}
					else
					{
						ductfittingsclash_no.Add(elem);
					}
				}

			}

			foreach (Element e in ductfittingsclash)
			{
				Parameter param = e.LookupParameter("Clash");
				//Parameter param2 = e.LookupParameter("Clash Solved");
				//Parameter param3 = e.LookupParameter("Clash Comments");
				Parameter param4 = e.LookupParameter("Clash Grid Location");
				Parameter param5 = e.LookupParameter("Clash Category");

				string param_value = "";

				using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
				{
					t.Start();
					param.Set(param_value);
					//param2.Set(1);
					//param3.Set(param_value);
					param4.Set(param_value);
					param5.Set(param_value);
					t.Commit();
				}
			}

			foreach (Element e in ductfittingsclash_no)
			{
				Parameter param = e.LookupParameter("Clash");
				//Parameter param2 = e.LookupParameter("Clash Comments");
				Parameter param3 = e.LookupParameter("Clash Grid Location");
				Parameter param5 = e.LookupParameter("Clash Category");

				string param_value = "";

				using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
				{
					t.Start();
					param.Set(param_value);
					//param2.Set(param_value);
					param3.Set(param_value);
					param5.Set(param_value);
					t.Commit();
				}
			}
		}
	}
}
