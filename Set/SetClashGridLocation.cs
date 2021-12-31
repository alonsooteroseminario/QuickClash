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
	public static class SetClashGridLocation
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

			// get elements with "clash" parameter value == "YES"
			IList<Element> clash = new List<Element>();
			IList<Element> clash_familyinstance = new List<Element>();
			IList<ElementId> clashID_elements = new List<ElementId>();
			IList<ElementId> clashID_familyinstance = new List<ElementId>();

			// get elements with "clash" parameter value == "NO"
			IList<Element> clash_no = new List<Element>();

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

			foreach (Element elem in clash)
			{
				clashID_elements.Add(elem.Id);
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
						clash_familyinstance.Add(elem);
					}
					else
					{
						clash_no.Add(elem);
					}
				}

			}

			foreach (Element elem in clash_familyinstance)
			{
				clashID_familyinstance.Add(elem.Id);
			}



			Dictionary<string, XYZ> intersectionPoints = Intersect.getIntersections(doc);
			string intersection = "??";
			string msg = "Los resultados son:\n" + Environment.NewLine;

			XYZ p1 = new XYZ();
			XYZ p2 = new XYZ();

			foreach (ElementId eid in clashID_elements)
			{
				Element e = doc.GetElement(eid);

				Options op = new Options();
				//            		op.View = doc.ActiveView;
				//            		op.ComputeReferences = true;
				GeometryElement gm = e.get_Geometry(op);
				Solid so = gm.First() as Solid;
				XYZ p = so.ComputeCentroid();

				XYZ xyz = new XYZ(p.X, p.Y, 0);

				double distanceMin = 0;
				double distance = 0;

				distanceMin = xyz.DistanceTo(intersectionPoints.First().Value);
				foreach (KeyValuePair<string, XYZ> kp in intersectionPoints)
				{
					distance = xyz.DistanceTo(kp.Value);
					if (distance < distanceMin)
					{
						distanceMin = distance;
						intersection = kp.Key;
					}

				}

				double distanceInMeter = distanceMin / 3.281;
				//TaskDialog.Show("Clash Grid Location", intersection);

				Parameter param = e.LookupParameter("Clash Grid Location");
				string param_value = intersection;

				using (Transaction t = new Transaction(doc, "Set Clash grid location to element"))
				{
					t.Start();
					param.Set(param_value);
					t.Commit();
				}
				msg += "Centroid Point : (" + p.X.ToString() + ", " + p.Y.ToString() + ", 0)  :  " + "Clash Grid Location : " + param_value + "  Category:" + e.Category.Name.ToString() + "  ID : " + e.Id.ToString() + Environment.NewLine;
			}

			foreach (ElementId eid in clashID_familyinstance)
			{
				Element e = doc.GetElement(eid);
				LocationPoint p = e.Location as LocationPoint;

				XYZ xyz = new XYZ(p.Point.X, p.Point.Y, 0);
				double distanceMin = 0;
				double distance = 0;

				distanceMin = xyz.DistanceTo(intersectionPoints.First().Value);
				foreach (KeyValuePair<string, XYZ> kp in intersectionPoints)
				{
					distance = xyz.DistanceTo(kp.Value);
					if (distance < distanceMin)
					{
						distanceMin = distance;
						intersection = kp.Key;
					}

				}

				double distanceInMeter = distanceMin / 3.281;
				//TaskDialog.Show("Clash Grid Location", intersection);

				Parameter param = e.LookupParameter("Clash Grid Location");
				string param_value = intersection;

				using (Transaction t = new Transaction(doc, "Set Clash grid location to element"))
				{
					t.Start();
					param.Set(param_value);
					t.Commit();
				}
				msg += "Clash Grid Location : " + param_value + "      Category:" + e.Category.Name.ToString() + "      ID : " + e.Id.ToString() + Environment.NewLine;
			}



		}
	}
}
