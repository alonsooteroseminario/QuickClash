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
	public static class CheckClashSolved
	{
		public static void Do(ExternalCommandData commandData, List<Element> clash_no_)
		{

			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			var activeView = uidoc.ActiveView;

			List<Element> clashsolved_yes = new List<Element>();
			List<Element> clashsolved_no = new List<Element>();

			List<Element> clash_yes = new List<Element>();
			List<Element> clash_no = clash_no_;

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
				ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
				// Create a category filter for MechanicalEquipment
				ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
				// Create a logic And filter for all MechanicalEquipment Family
				LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
				// Apply the filter to the elements in the active document
				FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
				IList<Element> mechanicalequipment = MEcoll.WherePasses(MEInstancesFilter).ToElements();
				foreach (Element e in mechanicalequipment)
				{
					Parameter param = e.LookupParameter("Clash Solved");
					Parameter param2 = e.LookupParameter("Clash");

					using (Transaction t = new Transaction(doc, "parametersME"))
					{
						t.Start();
						if (param.AsInteger() == 1 && param2.AsString() == "YES")
						{
							param2.Set("");
							clashsolved_yes.Add(e);
						}
						else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
						{
							clashsolved_yes.Add(e);
						}
						else
						{
							clashsolved_no.Add(e);
						}

						if (param2.AsString() == "YES")
						{
							clash_yes.Add(e);
						}
						//						 	if (!(param2.AsString() == "YES"))
						//						 	{
						//						 		clash_no.Add(e);
						//						 	}
						t.Commit();
					}
				}
			}



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

			foreach (Element e in ducts)
			{
				Parameter param = e.LookupParameter("Clash Solved");
				Parameter param2 = e.LookupParameter("Clash");

				using (Transaction t = new Transaction(doc, "parameters duct"))
				{
					t.Start();
					if (param.AsInteger() == 1 && param2.AsString() == "YES")
					{
						param2.Set("");
						clashsolved_yes.Add(e);
					}
					else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
					{
						clashsolved_yes.Add(e);
					}
					else
					{
						clashsolved_no.Add(e);
					}
					if (param2.AsString() == "YES")
					{
						clash_yes.Add(e);
					}
					t.Commit();
				}

			}

			foreach (Element e in pipes)
			{
				Parameter param = e.LookupParameter("Clash Solved");
				Parameter param2 = e.LookupParameter("Clash");

				using (Transaction t2 = new Transaction(doc, "parameters pipes"))
				{
					t2.Start();
					if (param.AsInteger() == 1 && param2.AsString() == "YES")
					{
						param2.Set("");
						clashsolved_yes.Add(e);
					}
					else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
					{
						clashsolved_yes.Add(e);
					}
					else
					{
						clashsolved_no.Add(e);
					}

					if (param2.AsString() == "YES")
					{
						clash_yes.Add(e);
					}

					t2.Commit();
				}

			}

			foreach (Element e in conduits)
			{
				Parameter param = e.LookupParameter("Clash Solved");
				Parameter param2 = e.LookupParameter("Clash");

				using (Transaction t3 = new Transaction(doc, "parameters conduits"))
				{
					t3.Start();
					if (param.AsInteger() == 1 && param2.AsString() == "YES")
					{
						param2.Set("");
						clashsolved_yes.Add(e);
					}
					else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
					{
						clashsolved_yes.Add(e);
					}
					else
					{
						clashsolved_no.Add(e);
					}
					if (param2.AsString() == "YES")
					{
						clash_yes.Add(e);
					}

					t3.Commit();
				}

			}

			foreach (Element e in cabletrays)
			{
				Parameter param = e.LookupParameter("Clash Solved");
				Parameter param2 = e.LookupParameter("Clash");

				using (Transaction t4 = new Transaction(doc, "parameters cable tray"))
				{
					t4.Start();
					if (param.AsInteger() == 1 && param2.AsString() == "YES")
					{
						param2.Set("");
						clashsolved_yes.Add(e);
					}
					else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
					{
						clashsolved_yes.Add(e);
					}
					else
					{
						clashsolved_no.Add(e);
					}

					if (param2.AsString() == "YES")
					{
						clash_yes.Add(e);
					}

					t4.Commit();
				}

			}

			foreach (Element e in flexducts)
			{
				Parameter param = e.LookupParameter("Clash Solved");
				Parameter param2 = e.LookupParameter("Clash");

				using (Transaction t = new Transaction(doc, "parameters flexduct"))
				{
					t.Start();
					if (param.AsInteger() == 1 && param2.AsString() == "YES")
					{
						param2.Set("");
						clashsolved_yes.Add(e);
					}
					else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
					{
						clashsolved_yes.Add(e);
					}
					else
					{
						clashsolved_no.Add(e);
					}

					if (param2.AsString() == "YES")
					{
						clash_yes.Add(e);
					}

					t.Commit();
				}

			}

			foreach (Element e in flexpipes)
			{
				Parameter param = e.LookupParameter("Clash Solved");
				Parameter param2 = e.LookupParameter("Clash");

				using (Transaction t = new Transaction(doc, "parameters flexpipes"))
				{
					t.Start();
					if (param.AsInteger() == 1 && param2.AsString() == "YES")
					{
						param2.Set("");
						clashsolved_yes.Add(e);
					}
					else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
					{
						clashsolved_yes.Add(e);
					}
					else
					{
						clashsolved_no.Add(e);
					}

					if (param2.AsString() == "YES")
					{
						clash_yes.Add(e);
					}

					t.Commit();
				}

			}

			foreach (Element elem in clash_no)
			{
				Parameter param2 = elem.LookupParameter("Clash");
				string vacio = "";
				using (Transaction t = new Transaction(doc, "Set CLASH = vacio "))
				{

					t.Start();

					param2.Set(vacio);

					t.Commit();
				}
			}

			if (clash_yes.Count() < 1)
			{
				TaskDialog.Show("Dynoscript", "NO HAY INTERFERENCIAS!! en esta vista activa \n\n Muy bien! :)");
				using (Transaction t = new Transaction(doc, "Cambiar nombre Comentado"))
				{
					t.Start();
					activeView.Name = activeView.Name.ToString() + " - RESUELTO";
					t.Commit();
				}

			}
		}
	}
}
