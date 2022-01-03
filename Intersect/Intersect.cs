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
	public static class Intersect
	{
		public static Dictionary<string, XYZ> getIntersections(Document _doc)
		{

			string coordinate = null;
			string coordA = null;
			string coordB = null;
			string coordC = null;
			Dictionary<string, XYZ> intersectionPoints = new Dictionary<string, XYZ>();


			ICollection<ElementId> grids = new FilteredElementCollector(_doc).WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Grids)).WhereElementIsNotElementType().ToElementIds();

			ICollection<ElementId> refgrid = new FilteredElementCollector(_doc).WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Grids)).WhereElementIsNotElementType().ToElementIds();
			foreach (ElementId eid in grids)

			{
				Grid g = _doc.GetElement(eid) as Grid;

				coordA = g.Name;

				Curve c = g.Curve;

				refgrid.Remove(eid);

				foreach (ElementId eid2 in refgrid)
				{
					Grid g2 = _doc.GetElement(eid2) as Grid;

					coordB = g2.Name;
					Curve c2 = g2.Curve;

					IntersectionResultArray results;

					SetComparisonResult result = c.Intersect(c2, out results);

					if (result != SetComparisonResult.Overlap)
					{ }

					else if (results == null || results.Size != 1)

					{ }

					else if (results.Size > 0)
					{
						for (int i = 0; i < results.Size; i++)
						{
							IntersectionResult iresult = results.get_Item(i);
							coordC = i.ToString();

							coordinate = coordA + " / " + coordB + " - " + coordC;

							XYZ point = iresult.XYZPoint;

							intersectionPoints.Add(coordinate, point);

						}
					}
					continue;
				}

			}
			return intersectionPoints;
		}

		public static void SetClashGridLocation(ExternalCommandData commandData)
		{

			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			// Get Active View
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



			Dictionary<string, XYZ> intersectionPoints = getIntersections(doc);
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

			//TaskDialog.Show("Dynoscript", msg);

		} // Todos los elemnentos con CLASH = YES de la vista activa.
		public static void SetClashGridLocation_UI(ExternalCommandData commandData, IList<Element> clash_, IList<Element> clash_familyinstance_)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			// Get Active View
			var activeView = uidoc.ActiveView;

			List<ElementId> clashID_elements = new List<ElementId>();
			List<ElementId> clashID_familyinstance = new List<ElementId>();

			// get elements with "clash" parameter value == "YES"
			IList<Element> clash = clash_;
			IList<Element> clash_familyinstance = clash_familyinstance_;
			// get elements with "clash" parameter value == "NO"
			IList<Element> clash_no = new List<Element>();

			foreach (Element elem in clash)
			{
				clashID_elements.Add(elem.Id);
			}

			foreach (Element elem in clash_familyinstance)
			{
				clashID_familyinstance.Add(elem.Id);
			}

			Dictionary<string, XYZ> intersectionPoints = getIntersections(doc);
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

			//TaskDialog.Show("Dynoscript", msg);

		}


		// Element vs Element
		public static void MultipleElementsToMultipleCategory_UI(ExternalCommandData commandData, List<BuiltInCategory> UI_list1_, List<BuiltInCategory> UI_list3_)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			List<BuiltInCategory> UI_list1 = UI_list1_; // Element grupo 1
			List<BuiltInCategory> UI_list3 = UI_list3_; // Element grupo 2

			List<Element> allElements = new List<Element>();

			foreach (BuiltInCategory bic in UI_list1)
			{
				if (bic == BuiltInCategory.OST_CableTray)
				{
					IList<Element> cabletrays = Get.ElementsByBuiltCategory(commandData, bic, "cabletrays");
					foreach (Element i in cabletrays)
					{
						allElements.Add(i);
					}
				}

				if (bic == BuiltInCategory.OST_Conduit)
				{
					IList<Element> conduits = Get.ElementsByBuiltCategory(commandData, bic, "conduits");
					foreach (Element i in conduits)
					{
						allElements.Add(i);
					}
				}
				if (bic == BuiltInCategory.OST_DuctCurves)
				{
					IList<Element> ducts = Get.ElementsByBuiltCategory(commandData, bic, "ducts");
					foreach (Element i in ducts)
					{
						allElements.Add(i);
					}
				}
				if (bic == BuiltInCategory.OST_PipeCurves)
				{
					IList<Element> pipes = Get.ElementsByBuiltCategory(commandData, bic, "pipes");
					foreach (Element i in pipes)
					{
						allElements.Add(i);
					}
				}
				if (bic == BuiltInCategory.OST_FlexDuctCurves)
				{
					IList<Element> flexducts = Get.ElementsByBuiltCategory(commandData, bic, "flexducts");
					foreach (Element i in flexducts)
					{
						allElements.Add(i);
					}
				}
				if (bic == BuiltInCategory.OST_FlexPipeCurves)
				{
					IList<Element> flexpipes = Get.ElementsByBuiltCategory(commandData, bic, "flexpipes");
					foreach (Element i in flexpipes)
					{
						allElements.Add(i);
					}
				}
			}

			List<Element> clash_yesA = new List<Element>();

			foreach (Element e in allElements)
			{
				ElementId eID = e.Id;
				GeometryElement geomElement = e.get_Geometry(new Options());
				Solid solid = null;

				foreach (GeometryObject geomObj in geomElement)
				{
					solid = geomObj as Solid;
					if (solid != null)
					{
						break;
					}

				}// solid = geomObj;
				 // Find intersections 

				ICollection<ElementId> collectoreID = new List<ElementId>();
				collectoreID.Add(eID);

				foreach (BuiltInCategory bic in UI_list3)
				{
					if (bic == BuiltInCategory.OST_CableTray)
					{
						ElementClassFilter DUFilter4 = new ElementClassFilter(typeof(CableTray));
						ElementCategoryFilter DU2Categoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
						LogicalAndFilter DU2InstancesFilter4 = new LogicalAndFilter(DUFilter4, DU2Categoryfilter4);

						ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector4 = new FilteredElementCollector(doc, activeView.Id);

						collector4.OfClass(typeof(CableTray));
						collector4.WherePasses(DU2InstancesFilter4);
						collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector4.WherePasses(filter4);

						if (collector4.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector4.First().Category.Name.ToString() + " / ID: " + collector4.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector4)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}

					if (bic == BuiltInCategory.OST_Conduit)
					{
						ElementClassFilter DUFilter3 = new ElementClassFilter(typeof(Conduit));
						ElementCategoryFilter DU2Categoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
						LogicalAndFilter DU2InstancesFilter3 = new LogicalAndFilter(DUFilter3, DU2Categoryfilter3);

						ExclusionFilter filter3 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector3 = new FilteredElementCollector(doc, activeView.Id);

						collector3.OfClass(typeof(Conduit));
						collector3.WherePasses(DU2InstancesFilter3);
						collector3.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector3.WherePasses(filter3);

						if (collector3.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector3.First().Category.Name.ToString() + " / ID: " + collector3.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector3)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}

					if (bic == BuiltInCategory.OST_DuctCurves)
					{
						ElementClassFilter DUFilter = new ElementClassFilter(typeof(Duct));
						ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
						LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);

						ExclusionFilter filter = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);

						collector.OfClass(typeof(Duct));
						collector.WherePasses(DU2InstancesFilter);
						collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector.WherePasses(filter);

						if (collector.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector.First().Category.Name.ToString() + " / ID: " + collector.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}

					if (bic == BuiltInCategory.OST_PipeCurves)
					{
						ElementClassFilter DUFilter2 = new ElementClassFilter(typeof(Pipe));
						ElementCategoryFilter DU2Categoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
						LogicalAndFilter DU2InstancesFilter2 = new LogicalAndFilter(DUFilter2, DU2Categoryfilter2);

						ExclusionFilter filter2 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector2 = new FilteredElementCollector(doc, activeView.Id);

						collector2.OfClass(typeof(Pipe));
						collector2.WherePasses(DU2InstancesFilter2);
						collector2.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector2.WherePasses(filter2);
						if (collector2.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector2.First().Category.Name.ToString() + " / ID: " + collector2.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector2)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}

					}

					if (bic == BuiltInCategory.OST_FlexDuctCurves)
					{
						ElementClassFilter DUFilter5 = new ElementClassFilter(typeof(FlexDuct));
						ElementCategoryFilter DU2Categoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
						LogicalAndFilter DU2InstancesFilter5 = new LogicalAndFilter(DUFilter5, DU2Categoryfilter5);

						ExclusionFilter filter5 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector5 = new FilteredElementCollector(doc, activeView.Id);

						collector5.OfClass(typeof(FlexDuct));
						collector5.WherePasses(DU2InstancesFilter5);
						collector5.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector5.WherePasses(filter5);

						if (collector5.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector5.First().Category.Name.ToString() + " / ID: " + collector5.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector5)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}

					}

					if (bic == BuiltInCategory.OST_FlexPipeCurves)
					{
						ElementClassFilter DUFilter6 = new ElementClassFilter(typeof(FlexPipe));
						ElementCategoryFilter DU2Categoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
						LogicalAndFilter DU2InstancesFilter6 = new LogicalAndFilter(DUFilter6, DU2Categoryfilter6);

						ExclusionFilter filter6 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector6 = new FilteredElementCollector(doc, activeView.Id);

						collector6.OfClass(typeof(FlexPipe));
						collector6.WherePasses(DU2InstancesFilter6);
						collector6.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector6.WherePasses(filter6);

						if (collector6.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector6.First().Category.Name.ToString() + " / ID: " + collector6.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}

							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector6)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}
				}

			}

			foreach (Element elem in clash_yesA)
			{
				Parameter param = elem.LookupParameter("Clash");

				string clash = "YES";

				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					param.Set(clash);

					t.Commit();
				}
			}

			SetClashGridLocation(commandData);

		} // Elem vs Elem // Only Active View
		public static void MultipleElementsToMultipleCategory_UI_doc(ExternalCommandData commandData, List<BuiltInCategory> UI_list1_, List<BuiltInCategory> UI_list3_)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			// Get Active View
			var activeView = uidoc.ActiveView;

			List<BuiltInCategory> UI_list1 = UI_list1_; // Element grupo 1
			List<BuiltInCategory> UI_list3 = UI_list3_; // Element grupo 2

			List<Element> allElements = new List<Element>();

			foreach (BuiltInCategory bic in UI_list1)
			{
				if (bic == BuiltInCategory.OST_CableTray)
				{
					ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
					ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
					LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);

					FilteredElementCollector DUcoll4 = new FilteredElementCollector(doc);
					IList<Element> cabletrays = DUcoll4.WherePasses(DUInstancesFilter4).ToElements();

					foreach (Element i in cabletrays)
					{
						allElements.Add(i);
					}

				}

				if (bic == BuiltInCategory.OST_Conduit)
				{
					ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
					ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
					LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);

					FilteredElementCollector DUcoll3 = new FilteredElementCollector(doc);
					IList<Element> conduits = DUcoll3.WherePasses(DUInstancesFilter3).ToElements();

					foreach (Element i in conduits)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_DuctCurves)
				{
					ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
					ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
					LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);

					FilteredElementCollector DUcoll = new FilteredElementCollector(doc);
					IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements(); // DUCTS

					foreach (Element i in ducts)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_PipeCurves)
				{
					ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
					ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
					LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);

					FilteredElementCollector DUcoll2 = new FilteredElementCollector(doc);
					IList<Element> pipes = DUcoll2.WherePasses(DUInstancesFilter2).ToElements();

					foreach (Element i in pipes)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_FlexDuctCurves)
				{
					ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
					ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
					LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);

					FilteredElementCollector DUcoll5 = new FilteredElementCollector(doc);
					IList<Element> flexducts = DUcoll5.WherePasses(DUInstancesFilter5).ToElements();

					foreach (Element i in flexducts)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_FlexPipeCurves)
				{
					ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));
					ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
					LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

					FilteredElementCollector DUcoll6 = new FilteredElementCollector(doc);
					IList<Element> flexpipes = DUcoll6.WherePasses(DUInstancesFilter6).ToElements();

					foreach (Element i in flexpipes)
					{
						allElements.Add(i);
					}
				}
			}


			List<Element> clash_yesA = new List<Element>();

			string mensaje = "";

			// foreach ( Element e in ducts) // OUT: mensaje2  y  clash_yesA<List>
			#region 

			foreach (Element e in allElements)
			{

				ElementId eID = e.Id;

				GeometryElement geomElement = e.get_Geometry(new Options());

				Solid solid = null;

				foreach (GeometryObject geomObj in geomElement)
				{
					solid = geomObj as Solid;
					if (solid != null)
					{
						break;
					}

				}// solid = geomObj;
				 // Find intersections 

				ICollection<ElementId> collectoreID = new List<ElementId>();
				collectoreID.Add(eID);

				foreach (BuiltInCategory bic in UI_list3)
				{
					if (bic == BuiltInCategory.OST_CableTray)
					{
						ElementClassFilter DUFilter4 = new ElementClassFilter(typeof(CableTray));
						ElementCategoryFilter DU2Categoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
						LogicalAndFilter DU2InstancesFilter4 = new LogicalAndFilter(DUFilter4, DU2Categoryfilter4);

						ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector4 = new FilteredElementCollector(doc);

						collector4.OfClass(typeof(CableTray));
						collector4.WherePasses(DU2InstancesFilter4);
						collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector4.WherePasses(filter4);

						if (collector4.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector4.First().Category.Name.ToString() + " / ID: " + collector4.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector4)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}

					if (bic == BuiltInCategory.OST_Conduit)
					{
						ElementClassFilter DUFilter3 = new ElementClassFilter(typeof(Conduit));
						ElementCategoryFilter DU2Categoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
						LogicalAndFilter DU2InstancesFilter3 = new LogicalAndFilter(DUFilter3, DU2Categoryfilter3);

						ExclusionFilter filter3 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector3 = new FilteredElementCollector(doc);

						collector3.OfClass(typeof(Conduit));
						collector3.WherePasses(DU2InstancesFilter3);
						collector3.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector3.WherePasses(filter3);

						if (collector3.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector3.First().Category.Name.ToString() + " / ID: " + collector3.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector3)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}

					if (bic == BuiltInCategory.OST_DuctCurves)
					{
						ElementClassFilter DUFilter = new ElementClassFilter(typeof(Duct));
						ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
						LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);

						ExclusionFilter filter = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector = new FilteredElementCollector(doc);

						collector.OfClass(typeof(Duct));
						collector.WherePasses(DU2InstancesFilter);
						collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector.WherePasses(filter);

						if (collector.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector.First().Category.Name.ToString() + " / ID: " + collector.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}

					if (bic == BuiltInCategory.OST_PipeCurves)
					{
						ElementClassFilter DUFilter2 = new ElementClassFilter(typeof(Pipe));
						ElementCategoryFilter DU2Categoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
						LogicalAndFilter DU2InstancesFilter2 = new LogicalAndFilter(DUFilter2, DU2Categoryfilter2);

						ExclusionFilter filter2 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector2 = new FilteredElementCollector(doc);

						collector2.OfClass(typeof(Pipe));
						collector2.WherePasses(DU2InstancesFilter2);
						collector2.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector2.WherePasses(filter2);
						if (collector2.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector2.First().Category.Name.ToString() + " / ID: " + collector2.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector2)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}

					}

					if (bic == BuiltInCategory.OST_FlexDuctCurves)
					{
						ElementClassFilter DUFilter5 = new ElementClassFilter(typeof(FlexDuct));
						ElementCategoryFilter DU2Categoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
						LogicalAndFilter DU2InstancesFilter5 = new LogicalAndFilter(DUFilter5, DU2Categoryfilter5);

						ExclusionFilter filter5 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector5 = new FilteredElementCollector(doc);

						collector5.OfClass(typeof(FlexDuct));
						collector5.WherePasses(DU2InstancesFilter5);
						collector5.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector5.WherePasses(filter5);

						if (collector5.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector5.First().Category.Name.ToString() + " / ID: " + collector5.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector5)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}

					}

					if (bic == BuiltInCategory.OST_FlexPipeCurves)
					{
						ElementClassFilter DUFilter6 = new ElementClassFilter(typeof(FlexPipe));
						ElementCategoryFilter DU2Categoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
						LogicalAndFilter DU2InstancesFilter6 = new LogicalAndFilter(DUFilter6, DU2Categoryfilter6);

						ExclusionFilter filter6 = new ExclusionFilter(collectoreID);
						FilteredElementCollector collector6 = new FilteredElementCollector(doc);

						collector6.OfClass(typeof(FlexPipe));
						collector6.WherePasses(DU2InstancesFilter6);
						collector6.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
						collector6.WherePasses(filter6);

						if (collector6.Count() > 0)
						{
							Parameter param = e.LookupParameter("Clash Category");
							Parameter paramID = e.LookupParameter("ID Element");

							string elemcategory = collector6.First().Category.Name.ToString() + " / ID: " + collector6.First().Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}

							if (!clash_yesA.Contains(e))
							{
								clash_yesA.Add(e);
							}
						}

						foreach (Element elem in collector6)
						{
							Parameter param = elem.LookupParameter("Clash Category");
							Parameter paramID = elem.LookupParameter("ID Element");

							string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

							using (Transaction t = new Transaction(doc, "Clash Category"))
							{
								t.Start();
								param.Set(elemcategory);

								t.Commit();
							}
							if (clash_yesA.Contains(elem) == false)
							{
								clash_yesA.Add(elem);
							}
						}
					}
				}

			}

			string mensaje2 = "";
			string msg = "";

			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");

				string clash = "YES";

				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					param.Set(clash);

					t.Commit();
				}
			}
			mensaje2 = mensaje + Environment.NewLine + "\nSon: " + clash_yesA.Count().ToString() + " el numero de Elementos con Clashes : " + Environment.NewLine + msg;
			TaskDialog.Show("Revit", mensaje2);
			#endregion
			SetClashGridLocation(commandData);
		} // Elem vs Elem // Todo documento
		public static void MultipleElementsToMultipleCategory(ExternalCommandData commandData)
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;
			var activeView = uidoc.ActiveView;

			// category Duct PIpes COnduit Calbe tray Flexduct flex pipes
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

			IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements(); // DUCTS

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

			List<Element> clash_yesA = new List<Element>();
			List<Element> clash_noA = new List<Element>();

			string mensaje = "";

			List<Element> allElements = new List<Element>();

			foreach (Element i in ducts)
			{
				allElements.Add(i);
			}
			foreach (Element i in pipes)
			{
				allElements.Add(i);
			}
			foreach (Element i in conduits)
			{
				allElements.Add(i);
			}
			foreach (Element i in cabletrays)
			{
				allElements.Add(i);
			}
			foreach (Element i in flexducts)
			{
				allElements.Add(i);
			}
			foreach (Element i in flexpipes)
			{
				allElements.Add(i);
			}

			// foreach ( Element e in ducts) // OUT: mensaje2  y  clash_yesA<List>
			#region 

			foreach (Element e in allElements)
			{

				ElementId eID = e.Id;

				GeometryElement geomElement = e.get_Geometry(new Options());

				Solid solid = null;

				foreach (GeometryObject geomObj in geomElement)
				{
					solid = geomObj as Solid;
					if (solid != null)
					{
						break;
					}

				}// solid = geomObj;
				 // Find intersections 

				// category Duct.
				ElementClassFilter DUFilter = new ElementClassFilter(typeof(Duct));
				ElementClassFilter DUFilter2 = new ElementClassFilter(typeof(Pipe));
				ElementClassFilter DUFilter3 = new ElementClassFilter(typeof(Conduit));
				ElementClassFilter DUFilter4 = new ElementClassFilter(typeof(CableTray));
				ElementClassFilter DUFilter5 = new ElementClassFilter(typeof(FlexDuct));
				ElementClassFilter DUFilter6 = new ElementClassFilter(typeof(FlexPipe));
				// Create a category filter for Ducts
				ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
				ElementCategoryFilter DU2Categoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
				ElementCategoryFilter DU2Categoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
				ElementCategoryFilter DU2Categoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
				ElementCategoryFilter DU2Categoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
				ElementCategoryFilter DU2Categoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);

				LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);
				LogicalAndFilter DU2InstancesFilter2 = new LogicalAndFilter(DUFilter2, DU2Categoryfilter2);
				LogicalAndFilter DU2InstancesFilter3 = new LogicalAndFilter(DUFilter3, DU2Categoryfilter3);
				LogicalAndFilter DU2InstancesFilter4 = new LogicalAndFilter(DUFilter4, DU2Categoryfilter4);
				LogicalAndFilter DU2InstancesFilter5 = new LogicalAndFilter(DUFilter5, DU2Categoryfilter5);
				LogicalAndFilter DU2InstancesFilter6 = new LogicalAndFilter(DUFilter6, DU2Categoryfilter6);

				ICollection<ElementId> collectoreID = new List<ElementId>();
				collectoreID.Add(eID);

				//Collector = Clashes
				ExclusionFilter filter = new ExclusionFilter(collectoreID);
				ExclusionFilter filter2 = new ExclusionFilter(collectoreID);
				ExclusionFilter filter3 = new ExclusionFilter(collectoreID);
				ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
				ExclusionFilter filter5 = new ExclusionFilter(collectoreID);
				ExclusionFilter filter6 = new ExclusionFilter(collectoreID);

				FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);
				FilteredElementCollector collector2 = new FilteredElementCollector(doc, activeView.Id);
				FilteredElementCollector collector3 = new FilteredElementCollector(doc, activeView.Id);
				FilteredElementCollector collector4 = new FilteredElementCollector(doc, activeView.Id);
				FilteredElementCollector collector5 = new FilteredElementCollector(doc, activeView.Id);
				FilteredElementCollector collector6 = new FilteredElementCollector(doc, activeView.Id);

				collector.OfClass(typeof(Duct));
				collector2.OfClass(typeof(Pipe));
				collector3.OfClass(typeof(Conduit));
				collector4.OfClass(typeof(CableTray));
				collector5.OfClass(typeof(FlexDuct));
				collector6.OfClass(typeof(FlexPipe));

				collector.WherePasses(DU2InstancesFilter);
				collector2.WherePasses(DU2InstancesFilter2);
				collector3.WherePasses(DU2InstancesFilter3);
				collector4.WherePasses(DU2InstancesFilter4);
				collector5.WherePasses(DU2InstancesFilter5);
				collector6.WherePasses(DU2InstancesFilter6);

				collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
				collector.WherePasses(filter);
				collector2.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
				collector2.WherePasses(filter2);
				collector3.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
				collector3.WherePasses(filter3);
				collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
				collector4.WherePasses(filter4);
				collector5.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
				collector5.WherePasses(filter5);
				collector6.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
				collector6.WherePasses(filter6);

				if (collector.Count() > 0)
				{
					if (clash_yesA.Contains(e) == false)
					{
						clash_yesA.Add(e);
						Parameter param = e.LookupParameter("Clash Category");

						string elemcategory = collector.First().Category.Name.ToString() + " / ID: " + collector.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);
							t.Commit();
						}
					}

				}

				else if (collector2.Count() > 0)
				{
					if (clash_yesA.Contains(e) == false)
					{
						clash_yesA.Add(e);
						Parameter param = e.LookupParameter("Clash Category");

						string elemcategory = collector2.First().Category.Name.ToString() + " / ID: " + collector2.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);
							t.Commit();
						}
					}
				}
				else if (collector3.Count() > 0)
				{
					if (clash_yesA.Contains(e) == false)
					{
						clash_yesA.Add(e);
						Parameter param = e.LookupParameter("Clash Category");

						string elemcategory = collector3.First().Category.Name.ToString() + " / ID: " + collector3.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);
							t.Commit();
						}
					}
				}
				else if (collector4.Count() > 0)
				{
					if (clash_yesA.Contains(e) == false)
					{
						clash_yesA.Add(e);
						Parameter param = e.LookupParameter("Clash Category");

						string elemcategory = collector4.First().Category.Name.ToString() + " / ID: " + collector4.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);
							t.Commit();
						}
					}
				}
				else if (collector5.Count() > 0)
				{
					if (clash_yesA.Contains(e) == false)
					{
						clash_yesA.Add(e);
						Parameter param = e.LookupParameter("Clash Category");

						string elemcategory = collector5.First().Category.Name.ToString() + " / ID: " + collector5.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);
							t.Commit();
						}
					}
				}
				else if (collector6.Count() > 0)
				{
					if (clash_yesA.Contains(e) == false)
					{
						clash_yesA.Add(e);
						Parameter param = e.LookupParameter("Clash Category");

						string elemcategory = collector6.First().Category.Name.ToString() + " / ID: " + collector6.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);
							t.Commit();
						}
					}
				}
				else
				{
					clash_noA.Add(e);
				}



				foreach (Element elem in collector)
				{
					Parameter param = elem.LookupParameter("Clash Category");

					string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

					using (Transaction t = new Transaction(doc, "Clash Category"))
					{
						t.Start();
						param.Set(elemcategory);
						t.Commit();
					}

					if (clash_yesA.Contains(elem) == false)
					{
						clash_yesA.Add(elem);
					}
					else
					{
						clash_noA.Add(elem);
					}
				}
				foreach (Element elem in collector2)
				{
					Parameter param = elem.LookupParameter("Clash Category");

					string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

					using (Transaction t = new Transaction(doc, "Clash Category"))
					{
						t.Start();
						param.Set(elemcategory);
						t.Commit();
					}


					if (clash_yesA.Contains(elem) == false)
					{
						clash_yesA.Add(elem);
					}
					else
					{
						clash_noA.Add(elem);
					}
				}
				foreach (Element elem in collector3)
				{
					Parameter param = elem.LookupParameter("Clash Category");

					string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

					using (Transaction t = new Transaction(doc, "Clash Category"))
					{
						t.Start();
						param.Set(elemcategory);
						t.Commit();
					}

					if (clash_yesA.Contains(elem) == false)
					{
						clash_yesA.Add(elem);
					}
					else
					{
						clash_noA.Add(elem);
					}
				}
				foreach (Element elem in collector4)
				{
					Parameter param = elem.LookupParameter("Clash Category");

					string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

					using (Transaction t = new Transaction(doc, "Clash Category"))
					{
						t.Start();
						param.Set(elemcategory);
						t.Commit();
					}

					if (clash_yesA.Contains(elem) == false)
					{
						clash_yesA.Add(elem);
					}
					else
					{
						clash_noA.Add(elem);
					}
				}
				foreach (Element elem in collector5)
				{
					Parameter param = elem.LookupParameter("Clash Category");

					string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

					using (Transaction t = new Transaction(doc, "Clash Category"))
					{
						t.Start();
						param.Set(elemcategory);
						t.Commit();
					}

					if (clash_yesA.Contains(elem) == false)
					{
						clash_yesA.Add(elem);
					}
					else
					{
						clash_noA.Add(elem);
					}
				}
				foreach (Element elem in collector6)
				{
					Parameter param = elem.LookupParameter("Clash Category");

					string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

					using (Transaction t = new Transaction(doc, "Clash Category"))
					{
						t.Start();
						param.Set(elemcategory);
						t.Commit();
					}

					if (clash_yesA.Contains(elem) == false)
					{
						clash_yesA.Add(elem);
					}
					else
					{
						clash_noA.Add(elem);
					}
				}


				mensaje = mensaje + Environment.NewLine + (collector.Count() + collector2.Count() + collector3.Count()
														+ collector4.Count() + collector5.Count() + collector6.Count()).ToString()
														+ " elements intersect with the selected element ("
														+ e.Name.ToString()
														+ e.Category.Name.ToString() + " id:"
														+ eID.ToString() + ")" + Environment.NewLine;

			}

			string mensaje2 = "";
			string msg = "";
			//			foreach (Element elem in clash_noA)
			//			{
			//				Parameter param = elem.LookupParameter("Clash");
			//			 	string clashNOvacio = "";
			//			 	using (Transaction t = new Transaction(doc, "Clash NO"))
			//			 	{
			//			 		t.Start();
			//					param.Set(clashNOvacio);
			//					t.Commit();
			//			 	}
			//			}

			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					param.Set(clash);
					t.Commit();
				}
			}
			mensaje2 = mensaje + Environment.NewLine + "\nSon: " + clash_yesA.Count().ToString() + " el numero de Elementos con Clashes : " + Environment.NewLine + msg;
			//TaskDialog.Show("Dynoscript", mensaje2);
			#endregion
		} // todo con todo VIsta ACtiva


		// Element vs. FamilyInstance
		public static void MultipleElementsToMultipleFamilyInstances_UI(ExternalCommandData commandData, List<BuiltInCategory> UI_list1_, List<BuiltInCategory> UI_list4_)
		{
			// Find intersections between Ducts category with selected element

			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

			// Get Active View
			var activeView = uidoc.ActiveView;

			List<BuiltInCategory> UI_list1 = UI_list1_; // Element grupo 1
			List<BuiltInCategory> UI_list4 = UI_list4_; // Family instance grupo 2

			List<Element> allElements = new List<Element>();

			foreach (BuiltInCategory bic in UI_list1)
			{
				if (bic == BuiltInCategory.OST_CableTray)
				{
					ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
					ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
					LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);

					FilteredElementCollector DUcoll4 = new FilteredElementCollector(doc, activeView.Id);
					IList<Element> cabletrays = DUcoll4.WherePasses(DUInstancesFilter4).ToElements();

					foreach (Element i in cabletrays)
					{
						allElements.Add(i);
					}

				}

				if (bic == BuiltInCategory.OST_Conduit)
				{
					ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
					ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
					LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);

					FilteredElementCollector DUcoll3 = new FilteredElementCollector(doc, activeView.Id);
					IList<Element> conduits = DUcoll3.WherePasses(DUInstancesFilter3).ToElements();

					foreach (Element i in conduits)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_DuctCurves)
				{
					ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
					ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
					LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);

					FilteredElementCollector DUcoll = new FilteredElementCollector(doc, activeView.Id);
					IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements(); // DUCTS

					foreach (Element i in ducts)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_PipeCurves)
				{
					ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
					ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
					LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);

					FilteredElementCollector DUcoll2 = new FilteredElementCollector(doc, activeView.Id);
					IList<Element> pipes = DUcoll2.WherePasses(DUInstancesFilter2).ToElements();

					foreach (Element i in pipes)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_FlexDuctCurves)
				{
					ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
					ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
					LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);

					FilteredElementCollector DUcoll5 = new FilteredElementCollector(doc, activeView.Id);
					IList<Element> flexducts = DUcoll5.WherePasses(DUInstancesFilter5).ToElements();

					foreach (Element i in flexducts)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_FlexPipeCurves)
				{
					ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));
					ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
					LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

					FilteredElementCollector DUcoll6 = new FilteredElementCollector(doc, activeView.Id);
					IList<Element> flexpipes = DUcoll6.WherePasses(DUInstancesFilter6).ToElements();

					foreach (Element i in flexpipes)
					{
						allElements.Add(i);
					}
				}
			}


			List<Element> clash_yesA = new List<Element>();

			List<Element> clash_yesA_element = new List<Element>();
			List<Element> clash_yesA_familyinstance = new List<Element>();

			string mensaje = "";

			// foreach ( Element e in ducts) // OUT: mensaje2  y  clash_yesA<List>
			#region 

			foreach (Element e in allElements)
			{

				ElementId eID = e.Id;

				GeometryElement geomElement = e.get_Geometry(new Options());

				Solid solid = null;
				foreach (GeometryObject geomObj in geomElement)
				{
					solid = geomObj as Solid;
					if (solid != null) break;
				}

				IList<BuiltInCategory> bics_fi = UI_list4;

				foreach (BuiltInCategory bic in bics_fi)
				{
					// Find intersections between family instances and a selected element
					// category Mechanical Euqipment
					ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
					// Create a category filter for Mechanical Euqipment
					ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(bic);

					LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);

					ICollection<ElementId> collectoreID = new List<ElementId>();
					if (collectoreID.Contains(e.Id) == false)
					{
						collectoreID.Add(eID);
					}


					ExclusionFilter filter = new ExclusionFilter(collectoreID);
					FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);
					collector.OfClass(typeof(FamilyInstance));
					collector.WherePasses(DU2InstancesFilter);
					collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
																								 //collector.WherePasses(filter);

					if (collector.Count() > 0) // agrega el elemento
					{
						Parameter param = e.LookupParameter("Clash Category");
						Parameter paramID = e.LookupParameter("ID Element");

						string elemcategory = collector.First().Category.Name.ToString() + " / ID: " + collector.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);

							t.Commit();
						}



						if (clash_yesA_element.Contains(e) == false)
						{
							clash_yesA_element.Add(e);
							clash_yesA.Add(e);
						}
					}

					foreach (Element elem in collector) // agrega el family instances
					{
						Parameter param = elem.LookupParameter("Clash Category");
						Parameter paramID = elem.LookupParameter("ID Element");

						string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);

							t.Commit();
						}
						if (clash_yesA_familyinstance.Contains(elem) == false)
						{
							clash_yesA_familyinstance.Add(elem);
							clash_yesA.Add(elem);
						}
					}
					mensaje = mensaje + Environment.NewLine + collector.Count().ToString() + " Family Instance intersect with the selected element ("
																				+ e.Name.ToString()
																				+ e.Category.Name.ToString() + " id:"
																				+ eID.ToString() + ")"
																				+ Environment.NewLine;
				}
			}

			string mensaje2 = "";
			string msg = "";

			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					param.Set(clash);
					t.Commit();
				}
			}

			SetClashGridLocation_UI(commandData, clash_yesA_element, clash_yesA_familyinstance);


			mensaje2 = mensaje + Environment.NewLine + "\nSon: " + clash_yesA.Count().ToString() + " el numero de Elementos con Clashes : " + Environment.NewLine + msg;
			//TaskDialog.Show("Revit", mensaje2);
			#endregion


		} // Elem vs FamilyInstance // Only Active View
		public static void MultipleElementsToMultipleFamilyInstances_UI_doc(ExternalCommandData commandData, List<BuiltInCategory> UI_list1_, List<BuiltInCategory> UI_list4_)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			// Get Active View
			var activeView = uidoc.ActiveView;

			List<BuiltInCategory> UI_list1 = UI_list1_; // Element grupo 1
			List<BuiltInCategory> UI_list4 = UI_list4_; // Family instance grupo 2

			List<Element> allElements = new List<Element>();

			foreach (BuiltInCategory bic in UI_list1)
			{
				if (bic == BuiltInCategory.OST_CableTray)
				{
					ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
					ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
					LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);

					FilteredElementCollector DUcoll4 = new FilteredElementCollector(doc);
					IList<Element> cabletrays = DUcoll4.WherePasses(DUInstancesFilter4).ToElements();

					foreach (Element i in cabletrays)
					{
						allElements.Add(i);
					}

				}

				if (bic == BuiltInCategory.OST_Conduit)
				{
					ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
					ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
					LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);

					FilteredElementCollector DUcoll3 = new FilteredElementCollector(doc);
					IList<Element> conduits = DUcoll3.WherePasses(DUInstancesFilter3).ToElements();

					foreach (Element i in conduits)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_DuctCurves)
				{
					ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
					ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
					LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);

					FilteredElementCollector DUcoll = new FilteredElementCollector(doc);
					IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements(); // DUCTS

					foreach (Element i in ducts)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_PipeCurves)
				{
					ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
					ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
					LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);

					FilteredElementCollector DUcoll2 = new FilteredElementCollector(doc);
					IList<Element> pipes = DUcoll2.WherePasses(DUInstancesFilter2).ToElements();

					foreach (Element i in pipes)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_FlexDuctCurves)
				{
					ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
					ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
					LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);

					FilteredElementCollector DUcoll5 = new FilteredElementCollector(doc);
					IList<Element> flexducts = DUcoll5.WherePasses(DUInstancesFilter5).ToElements();

					foreach (Element i in flexducts)
					{
						allElements.Add(i);
					}

				}
				if (bic == BuiltInCategory.OST_FlexPipeCurves)
				{
					ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));
					ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
					LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

					FilteredElementCollector DUcoll6 = new FilteredElementCollector(doc);
					IList<Element> flexpipes = DUcoll6.WherePasses(DUInstancesFilter6).ToElements();

					foreach (Element i in flexpipes)
					{
						allElements.Add(i);
					}
				}
			}


			List<Element> clash_yesA = new List<Element>();

			List<Element> clash_yesA_element = new List<Element>();
			List<Element> clash_yesA_familyinstance = new List<Element>();

			string mensaje = "";

			// foreach ( Element e in ducts) // OUT: mensaje2  y  clash_yesA<List>
			#region 

			foreach (Element e in allElements)
			{

				ElementId eID = e.Id;

				GeometryElement geomElement = e.get_Geometry(new Options());

				Solid solid = null;
				foreach (GeometryObject geomObj in geomElement)
				{
					solid = geomObj as Solid;
					if (solid != null) break;
				}

				IList<BuiltInCategory> bics_fi = UI_list4;

				foreach (BuiltInCategory bic in bics_fi)
				{
					// Find intersections between family instances and a selected element
					// category Mechanical Euqipment
					ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
					// Create a category filter for Mechanical Euqipment
					ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(bic);

					LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);

					ICollection<ElementId> collectoreID = new List<ElementId>();
					if (collectoreID.Contains(e.Id) == false)
					{
						collectoreID.Add(eID);
					}


					ExclusionFilter filter = new ExclusionFilter(collectoreID);
					FilteredElementCollector collector = new FilteredElementCollector(doc);
					collector.OfClass(typeof(FamilyInstance));
					collector.WherePasses(DU2InstancesFilter);
					collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
																								 //collector.WherePasses(filter);

					if (collector.Count() > 0) // agrega el elemento
					{
						Parameter param = e.LookupParameter("Clash Category");
						Parameter paramID = e.LookupParameter("ID Element");

						string elemcategory = collector.First().Category.Name.ToString() + " / ID: " + collector.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);

							t.Commit();
						}



						if (clash_yesA_element.Contains(e) == false)
						{
							clash_yesA_element.Add(e);
							clash_yesA.Add(e);
						}
					}

					foreach (Element elem in collector) // agrega el family instances
					{
						Parameter param = elem.LookupParameter("Clash Category");
						Parameter paramID = elem.LookupParameter("ID Element");

						string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);

							t.Commit();
						}
						if (clash_yesA_familyinstance.Contains(elem) == false)
						{
							clash_yesA_familyinstance.Add(elem);
							clash_yesA.Add(elem);
						}
					}
					mensaje = mensaje + Environment.NewLine + collector.Count().ToString() + " Family Instance intersect with the selected element ("
																				+ e.Name.ToString()
																				+ e.Category.Name.ToString() + " id:"
																				+ eID.ToString() + ")"
																				+ Environment.NewLine;
				}
			}

			string mensaje2 = "";
			string msg = "";

			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					param.Set(clash);
					t.Commit();
				}
			}

			SetClashGridLocation_UI(commandData, clash_yesA_element, clash_yesA_familyinstance);


			mensaje2 = mensaje + Environment.NewLine + "\nSon: " + clash_yesA.Count().ToString() + " el numero de Elementos con Clashes : " + Environment.NewLine + msg;
			//TaskDialog.Show("Revit", mensaje2);
			#endregion


		} // Elem vs FamilyInstance // Todo documento
		public static void MultipleElementsToMultipleFamilyInstances(ExternalCommandData commandData)
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;
			var activeView = uidoc.ActiveView;

			// category Duct PIpes COnduit Calbe tray Flexduct flex pipes
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

			IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements(); // DUCTS

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

			List<Element> allElements = new List<Element>();

			foreach (Element i in ducts)
			{
				allElements.Add(i);
			}
			foreach (Element i in pipes)
			{
				allElements.Add(i);
			}
			foreach (Element i in conduits)
			{
				allElements.Add(i);
			}
			foreach (Element i in cabletrays)
			{
				allElements.Add(i);
			}
			foreach (Element i in flexducts)
			{
				allElements.Add(i);
			}
			foreach (Element i in flexpipes)
			{
				allElements.Add(i);
			}

			string numero_ductos = allElements.Count().ToString();
			string mensaje = "Iteraciones de elementos : " + numero_ductos + "\n\n" + Environment.NewLine;

			List<Element> clash_yesA = new List<Element>();
			List<Element> clash_noA = new List<Element>();

			foreach (Element e in allElements)
			{

				ElementId eID = e.Id;

				GeometryElement geomElement = e.get_Geometry(new Options());

				Solid solid = null;
				foreach (GeometryObject geomObj in geomElement)
				{
					solid = geomObj as Solid;
					if (solid != null) break;
				}

				BuiltInCategory[] bics_fi = new BuiltInCategory[]
				{
				    //BuiltInCategory.OST_CableTray,
				    BuiltInCategory.OST_CableTrayFitting,
				   // BuiltInCategory.OST_Conduit,
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
				    BuiltInCategory.OST_PipeFitting,
					BuiltInCategory.OST_PlumbingFixtures,
					BuiltInCategory.OST_SpecialityEquipment,
					BuiltInCategory.OST_Sprinklers
					//BuiltInCategory.OST_Wire,

					//builtInCategory.OST_Walls,
					//builtInCategory.OST_Ceilings,
					//BuiltInCategory.OST_StructuralFraming,
				};



				foreach (BuiltInCategory bic in bics_fi)
				{
					// Find intersections between family instances and a selected element
					// category Mechanical Euqipment
					ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
					// Create a category filter for Mechanical Euqipment
					ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(bic);

					LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);

					ICollection<ElementId> collectoreID = new List<ElementId>();
					if (collectoreID.Contains(e.Id) == false)
					{
						collectoreID.Add(eID);
					}


					ExclusionFilter filter = new ExclusionFilter(collectoreID);
					FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);
					collector.OfClass(typeof(FamilyInstance));
					collector.WherePasses(DU2InstancesFilter);
					collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
																								 //collector.WherePasses(filter);

					if (collector.Count() > 0)
					{
						Parameter param = e.LookupParameter("Clash Category");
						//						Parameter paramID = e.LookupParameter("ID Element");

						string elemcategory = collector.First().Category.Name.ToString() + " / ID: " + collector.First().Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);

							t.Commit();
						}

						if (clash_yesA.Contains(e) == false)
						{
							clash_yesA.Add(e);
						}

					}
					else
					{
						clash_noA.Add(e);
					}


					foreach (Element elem in collector)
					{
						Parameter param = elem.LookupParameter("Clash Category");
						//						Parameter paramID = elem.LookupParameter("ID Element");

						string elemcategory = e.Category.Name.ToString() + " / ID: " + e.Id.ToString();

						using (Transaction t = new Transaction(doc, "Clash Category"))
						{
							t.Start();
							param.Set(elemcategory);

							t.Commit();
						}

						if (clash_yesA.Contains(elem) == false)
						{
							clash_yesA.Add(elem);
						}

						else
						{
							clash_noA.Add(elem);
						}
					}
					mensaje = mensaje + Environment.NewLine + collector.Count().ToString() + " Family Instance intersect with the selected element ("
																				+ e.Name.ToString()
																				+ e.Category.Name.ToString() + " id:"
																				+ eID.ToString() + ")"
																				+ Environment.NewLine;
				}
			}
			string mensaje2 = "";
			string msg = "";

			//			foreach (Element elem in clash_noA)
			//			{
			//				Parameter param = elem.LookupParameter("Clash");
			//			 	string clashNOvacio = "";
			//			 	using (Transaction t = new Transaction(doc, "Clash NO"))
			//			 	{
			//			 		t.Start();
			//					param.Set(clashNOvacio);
			//					t.Commit();
			//			 	}
			//			}

			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					param.Set(clash);
					t.Commit();
				}
			}
			mensaje2 = mensaje + Environment.NewLine + "\nSon: " + clash_yesA.Count().ToString() + " el numero de Elementos con Clashes : " + Environment.NewLine + msg;
			//TaskDialog.Show("Revit", mensaje2);
		} // todo con todo VIsta ACtiva


		//FamilyIntance vs Familyinstance
		public static void MultipleFamilyInstanceToMultipleFamilyInstances_BBox(ExternalCommandData commandData) // Family Instance vs Family Instance
		{
			UIApplication uiApp = commandData.Application;
			UIDocument uidoc = uiApp.ActiveUIDocument;
			Document doc = uiApp.ActiveUIDocument.Document;
			Application app = uiApp.Application;
			var activeView = uidoc.ActiveView;


			// FAMILY INSTANCES

			// Family Instance
			BuiltInCategory[] bics_finst = new BuiltInCategory[]
				{

					BuiltInCategory.OST_CableTrayFitting,

					BuiltInCategory.OST_ConduitFitting,

					BuiltInCategory.OST_DuctFitting,
					BuiltInCategory.OST_DuctTerminal,
					BuiltInCategory.OST_ElectricalEquipment,
					BuiltInCategory.OST_ElectricalFixtures,
					BuiltInCategory.OST_LightingDevices,
					BuiltInCategory.OST_LightingFixtures,
					BuiltInCategory.OST_MechanicalEquipment,

					BuiltInCategory.OST_PipeFitting,
					BuiltInCategory.OST_PlumbingFixtures,
					BuiltInCategory.OST_SpecialityEquipment,
					BuiltInCategory.OST_Sprinklers,

				};

			BuiltInCategory[] bics_finst_2 = new BuiltInCategory[]
			{

					BuiltInCategory.OST_CableTrayFitting,

					BuiltInCategory.OST_ConduitFitting,

					BuiltInCategory.OST_DuctFitting,
					BuiltInCategory.OST_DuctTerminal,
					BuiltInCategory.OST_ElectricalEquipment,
					BuiltInCategory.OST_ElectricalFixtures,
					BuiltInCategory.OST_LightingDevices,
					BuiltInCategory.OST_LightingFixtures,
					BuiltInCategory.OST_MechanicalEquipment,

					BuiltInCategory.OST_PipeFitting,
					BuiltInCategory.OST_PlumbingFixtures,
					BuiltInCategory.OST_SpecialityEquipment,
					BuiltInCategory.OST_Sprinklers,

			};

			//			IList<BuiltInCategory> bics_familyIns = UI_list2;
			IList<BuiltInCategory> bics_familyIns = bics_finst;

			List<Element> clash_familyinstance = new List<Element>();



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

					clash_familyinstance.Add(elem);
				}

			}
			List<Element> clash_yesA = new List<Element>();

			for (int i = 0; i < clash_familyinstance.Count(); i++)
			{
				Element elem = clash_familyinstance[i];

				BoundingBoxXYZ elem_bb = elem.get_BoundingBox(null);

				IList<Element> salida = new List<Element>();


				foreach (BuiltInCategory bic in bics_finst_2)
				{

					Outline outline_2 = new Outline(elem_bb.Min, elem_bb.Max);
					BoundingBoxIntersectsFilter bbfilter_2 = new BoundingBoxIntersectsFilter(outline_2);

					ElementClassFilter famFil = new ElementClassFilter(typeof(FamilyInstance));

					ElementCategoryFilter Categoryfilter = new ElementCategoryFilter(bic);

					LogicalAndFilter InstancesFilter = new LogicalAndFilter(famFil, Categoryfilter);

					FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, doc.ActiveView.Id); // 2 muros que intercepta con la ventana siguiente

					IList<Element> elementss = coll_outline_2.WherePasses(bbfilter_2).WherePasses(InstancesFilter).ToElements();

					//					FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, doc.ActiveView.Id); // 2 muros que intercepta con la ventana siguiente
					//					IList<Element> elements = coll_outline_2.WherePasses(bbfilter_2).OfClass(typeof(FamilyInstance)).ToElements();


					//
					//					
					//					foreach (Element n in elements) 
					//					{
					//						salida.Add(n);
					//					}

					if (elementss.Count() > 0) // clash
					{

						if (!clash_yesA.Contains(elem))
						{


							foreach (Element pp in elementss)
							{
								if (!(pp.Id == elem.Id))
								{

									clash_yesA.Add(elem);

									Parameter param = elem.LookupParameter("Clash Category");
									Parameter paramID = elem.LookupParameter("ID Element");
									string elemcategory = elementss.First().Category.Name.ToString() + " / ID: " + elementss.First().Id.ToString();

									using (Transaction t = new Transaction(doc, "Clash Category"))
									{
										t.Start();

										param.Set(elemcategory);

										t.Commit();
									}
								}
							}

						}

					}


				}




			}



			#region borrador

			//	 		string numero_ductos = clashID_familyinstance.Count().ToString();
			//	 		string mensaje = "Iteraciones de elementos : " + numero_ductos + "\n\n" + Environment.NewLine;
			//	 		
			//	 		
			//			foreach (ElementId eID in clashID_familyinstance) 
			//	 		{
			//	 
			//	 			//ElementId eID = e.Id;
			//	 			Element e = doc.GetElement(eID);
			//				//LocationPoint p = e.Location as LocationPoint;
			//	 			
			//				//XYZ xyz = new XYZ(p.Point.X, p.Point.Y,p.Point.Z);
			//				
			//				//XYZ p = so.ComputeCentroid();
			//				//FamilyInstance efi = e as FamilyInstance;
			//				
			//				GeometryElement geomElement = e.get_Geometry(new Options() );
			//				
			//				GeometryInstance geomFinstace = geomElement.First() as GeometryInstance;
			//
			//				GeometryElement gSymbol = geomFinstace.GetSymbolGeometry();
			//				
			//				//Solid solid = gSymbol.First() as Solid;
			//	 
			//	 			Solid solid = null;
			//	 			foreach(GeometryObject geomObj in gSymbol)
			//	 			{
			//	 				solid = geomObj as Solid;
			//	 				if( solid != null ) break;
			//	 			}
			//
			////	 			IList<BuiltInCategory> bics_fi2 = UI_list4;
			//	 			IList<BuiltInCategory> bics_fi2 = bics_finst;
			//	 			
			//	 			
			//	 			foreach (BuiltInCategory bic in bics_fi2) 
			//	 			{
			//					// Find intersections between family instances and a selected element
			//		 			// category Mechanical Euqipment
			//		 			ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
			//		 			// Create a category filter for Mechanical Euqipment
			//		 			ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(bic);
			//		 
			//		 			LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);
			//		 
			//		 			ICollection<ElementId> collectoreID = new List<ElementId>();
			//		 			if (collectoreID.Contains(e.Id)==false)
			//		 			{
			//						collectoreID.Add(eID);
			//					}
			////		 			
			////		 
			//		 			ExclusionFilter filter = new ExclusionFilter(collectoreID);
			//					FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);
			//		 			
			//					if (bic == BuiltInCategory.OST_CableTray) 
			//		 			{
			//						collector.OfClass(typeof(CableTray));
			//					}
			//		 			if (bic == BuiltInCategory.OST_DuctCurves) 
			//		 			{
			//						collector.OfClass(typeof(Duct));
			//					}
			//		 			if (bic == BuiltInCategory.OST_PipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(Pipe));
			//					}
			//		 			if (bic == BuiltInCategory.OST_Conduit) 
			//		 			{
			//						collector.OfClass(typeof(Conduit));
			//					}
			//		 			if (bic == BuiltInCategory.OST_FlexPipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(FlexPipe));
			//					}
			//		 			if (bic == BuiltInCategory.OST_FlexPipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(FlexPipe));
			//					}
			//
			//		 			collector.WherePasses(DU2InstancesFilter);
			//		 			collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
			//		 			collector.WherePasses(filter);
			//		 			
			//		 			if (collector.Count() > 0) 
			//			 		{
			//		 				if (clash_yesA.Contains(e)==false)
			//			 			{
			//							clash_yesA.Add(e);
			//						}
			//			        }
			//			 		
			//			 		foreach (Element elem in collector) 
			//			 		{
			//		 				if (clash_yesA.Contains(elem)==false)
			//			 			{
			//							clash_yesA.Add(elem);
			//						}
			//					}
			//		 			mensaje = mensaje + Environment.NewLine + collector.Count().ToString() + " Family Instance intersect with the selected element ("
			//			 																	+ e.Name.ToString() 
			//			 																	+ e.Category.Name.ToString() + " id:"
			//																 				+ eID.ToString() + ")"
			//																				+ Environment.NewLine;
			//				}
			//	 		}
			#endregion

			string msg = "";
			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					if (!(param.AsString() == "YES"))
					{
						param.Set(clash);
					}

					t.Commit();
				}
			}

		} // Family Instance vs Family Instance
		public static void MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI_doc(ExternalCommandData commandData, List<BuiltInCategory> UI_list2_, List<BuiltInCategory> UI_list4_) // Family Instance vs Family Instance
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			// Get Active View
			var activeView = uidoc.ActiveView;

			List<BuiltInCategory> UI_list2 = UI_list2_; // Family instance grupo 1
			List<BuiltInCategory> UI_list4 = UI_list4_; // Family instance grupo 2

			// FAMILY INSTANCES

			// Family Instance
			#region familyinstances BuiltinCategory

			//			BuiltInCategory[] bics_finst = new BuiltInCategory[] 	
			//				{
			//			
			//				    BuiltInCategory.OST_CableTrayFitting,
			//
			//				    BuiltInCategory.OST_ConduitFitting,
			//	
			//				    BuiltInCategory.OST_DuctFitting,
			//				    BuiltInCategory.OST_DuctTerminal,
			//				    BuiltInCategory.OST_ElectricalEquipment,
			//				    BuiltInCategory.OST_ElectricalFixtures,
			//				    BuiltInCategory.OST_LightingDevices,
			//				    BuiltInCategory.OST_LightingFixtures,
			//				    BuiltInCategory.OST_MechanicalEquipment,
			//		
			//				    BuiltInCategory.OST_PipeFitting,
			//				    BuiltInCategory.OST_PlumbingFixtures,
			//				    BuiltInCategory.OST_SpecialityEquipment,
			//				    BuiltInCategory.OST_Sprinklers,
			//		
			//				};
			//			
			//				BuiltInCategory[] bics_finst_2 = new BuiltInCategory[] 	
			//				{
			//			
			//				    BuiltInCategory.OST_CableTrayFitting,
			//
			//				    BuiltInCategory.OST_ConduitFitting,
			//	
			//				    BuiltInCategory.OST_DuctFitting,
			//				    BuiltInCategory.OST_DuctTerminal,
			//				    BuiltInCategory.OST_ElectricalEquipment,
			//				    BuiltInCategory.OST_ElectricalFixtures,
			//				    BuiltInCategory.OST_LightingDevices,
			//				    BuiltInCategory.OST_LightingFixtures,
			//				    BuiltInCategory.OST_MechanicalEquipment,
			//		
			//				    BuiltInCategory.OST_PipeFitting,
			//				    BuiltInCategory.OST_PlumbingFixtures,
			//				    BuiltInCategory.OST_SpecialityEquipment,
			//				    BuiltInCategory.OST_Sprinklers,
			//		
			//				};
			#endregion

			IList<BuiltInCategory> bics_familyIns = UI_list2;
			//			IList<BuiltInCategory> bics_familyIns = bics_finst;

			IList<BuiltInCategory> bics_finst_4 = UI_list4;

			List<Element> clash_familyinstance = new List<Element>();



			foreach (BuiltInCategory bic in bics_familyIns)
			{
				ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
				// Create a category filter for MechanicalEquipment
				ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
				// Create a logic And filter for all MechanicalEquipment Family
				LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
				// Apply the filter to the elements in the active document
				FilteredElementCollector MEcoll = new FilteredElementCollector(doc);
				IList<Element> mechanicalequipment = MEcoll.WherePasses(MEInstancesFilter).ToElements();

				foreach (Element elem in mechanicalequipment)
				{

					clash_familyinstance.Add(elem);
				}

			}
			List<Element> clash_yesA = new List<Element>();

			for (int i = 0; i < clash_familyinstance.Count(); i++)
			{
				Element elem = clash_familyinstance[i];

				BoundingBoxXYZ elem_bb = elem.get_BoundingBox(null);

				IList<Element> salida = new List<Element>();


				foreach (BuiltInCategory bic in bics_finst_4)
				{

					Outline outline_2 = new Outline(elem_bb.Min, elem_bb.Max);
					BoundingBoxIntersectsFilter bbfilter_2 = new BoundingBoxIntersectsFilter(outline_2);

					ElementClassFilter famFil = new ElementClassFilter(typeof(FamilyInstance));

					ElementCategoryFilter Categoryfilter = new ElementCategoryFilter(bic);

					LogicalAndFilter InstancesFilter = new LogicalAndFilter(famFil, Categoryfilter);

					FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc); // 2 muros que intercepta con la ventana siguiente

					IList<Element> elementss = coll_outline_2.WherePasses(bbfilter_2).WherePasses(InstancesFilter).ToElements();

					//					FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, doc.ActiveView.Id); // 2 muros que intercepta con la ventana siguiente
					//					IList<Element> elements = coll_outline_2.WherePasses(bbfilter_2).OfClass(typeof(FamilyInstance)).ToElements();


					//
					//					
					//					foreach (Element n in elements) 
					//					{
					//						salida.Add(n);
					//					}

					if (elementss.Count() > 0) // clash
					{

						if (!clash_yesA.Contains(elem))
						{


							foreach (Element pp in elementss)
							{
								if (!(pp.Id == elem.Id))
								{

									clash_yesA.Add(elem);

									Parameter param = elem.LookupParameter("Clash Category");
									Parameter paramID = elem.LookupParameter("ID Element");
									string elemcategory = elementss.First().Category.Name.ToString() + " / ID: " + elementss.First().Id.ToString();

									using (Transaction t = new Transaction(doc, "Clash Category"))
									{
										t.Start();

										param.Set(elemcategory);

										t.Commit();
									}
								}
							}

						}

					}


				}




			}



			#region borrador

			//	 		string numero_ductos = clashID_familyinstance.Count().ToString();
			//	 		string mensaje = "Iteraciones de elementos : " + numero_ductos + "\n\n" + Environment.NewLine;
			//	 		
			//	 		
			//			foreach (ElementId eID in clashID_familyinstance) 
			//	 		{
			//	 
			//	 			//ElementId eID = e.Id;
			//	 			Element e = doc.GetElement(eID);
			//				//LocationPoint p = e.Location as LocationPoint;
			//	 			
			//				//XYZ xyz = new XYZ(p.Point.X, p.Point.Y,p.Point.Z);
			//				
			//				//XYZ p = so.ComputeCentroid();
			//				//FamilyInstance efi = e as FamilyInstance;
			//				
			//				GeometryElement geomElement = e.get_Geometry(new Options() );
			//				
			//				GeometryInstance geomFinstace = geomElement.First() as GeometryInstance;
			//
			//				GeometryElement gSymbol = geomFinstace.GetSymbolGeometry();
			//				
			//				//Solid solid = gSymbol.First() as Solid;
			//	 
			//	 			Solid solid = null;
			//	 			foreach(GeometryObject geomObj in gSymbol)
			//	 			{
			//	 				solid = geomObj as Solid;
			//	 				if( solid != null ) break;
			//	 			}
			//
			////	 			IList<BuiltInCategory> bics_fi2 = UI_list4;
			//	 			IList<BuiltInCategory> bics_fi2 = bics_finst;
			//	 			
			//	 			
			//	 			foreach (BuiltInCategory bic in bics_fi2) 
			//	 			{
			//					// Find intersections between family instances and a selected element
			//		 			// category Mechanical Euqipment
			//		 			ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
			//		 			// Create a category filter for Mechanical Euqipment
			//		 			ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(bic);
			//		 
			//		 			LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);
			//		 
			//		 			ICollection<ElementId> collectoreID = new List<ElementId>();
			//		 			if (collectoreID.Contains(e.Id)==false)
			//		 			{
			//						collectoreID.Add(eID);
			//					}
			////		 			
			////		 
			//		 			ExclusionFilter filter = new ExclusionFilter(collectoreID);
			//					FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);
			//		 			
			//					if (bic == BuiltInCategory.OST_CableTray) 
			//		 			{
			//						collector.OfClass(typeof(CableTray));
			//					}
			//		 			if (bic == BuiltInCategory.OST_DuctCurves) 
			//		 			{
			//						collector.OfClass(typeof(Duct));
			//					}
			//		 			if (bic == BuiltInCategory.OST_PipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(Pipe));
			//					}
			//		 			if (bic == BuiltInCategory.OST_Conduit) 
			//		 			{
			//						collector.OfClass(typeof(Conduit));
			//					}
			//		 			if (bic == BuiltInCategory.OST_FlexPipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(FlexPipe));
			//					}
			//		 			if (bic == BuiltInCategory.OST_FlexPipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(FlexPipe));
			//					}
			//
			//		 			collector.WherePasses(DU2InstancesFilter);
			//		 			collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
			//		 			collector.WherePasses(filter);
			//		 			
			//		 			if (collector.Count() > 0) 
			//			 		{
			//		 				if (clash_yesA.Contains(e)==false)
			//			 			{
			//							clash_yesA.Add(e);
			//						}
			//			        }
			//			 		
			//			 		foreach (Element elem in collector) 
			//			 		{
			//		 				if (clash_yesA.Contains(elem)==false)
			//			 			{
			//							clash_yesA.Add(elem);
			//						}
			//					}
			//		 			mensaje = mensaje + Environment.NewLine + collector.Count().ToString() + " Family Instance intersect with the selected element ("
			//			 																	+ e.Name.ToString() 
			//			 																	+ e.Category.Name.ToString() + " id:"
			//																 				+ eID.ToString() + ")"
			//																				+ Environment.NewLine;
			//				}
			//	 		}
			#endregion

			string msg = "";
			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					if (!(param.AsString() == "YES"))
					{
						param.Set(clash);
					}

					t.Commit();
				}
			}

		} // Family Instance vs Family Instance
		public static void MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI(ExternalCommandData commandData, List<BuiltInCategory> UI_list2_, List<BuiltInCategory> UI_list4_) // Family Instance vs Family Instance
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			// Get Active View
			var activeView = uidoc.ActiveView;

			List<BuiltInCategory> UI_list2 = UI_list2_; // Family instance grupo 1
			List<BuiltInCategory> UI_list4 = UI_list4_; // Family instance grupo 2

			// FAMILY INSTANCES

			// Family Instance
			#region familyinstances BuiltinCategory

			//			BuiltInCategory[] bics_finst = new BuiltInCategory[] 	
			//				{
			//			
			//				    BuiltInCategory.OST_CableTrayFitting,
			//
			//				    BuiltInCategory.OST_ConduitFitting,
			//	
			//				    BuiltInCategory.OST_DuctFitting,
			//				    BuiltInCategory.OST_DuctTerminal,
			//				    BuiltInCategory.OST_ElectricalEquipment,
			//				    BuiltInCategory.OST_ElectricalFixtures,
			//				    BuiltInCategory.OST_LightingDevices,
			//				    BuiltInCategory.OST_LightingFixtures,
			//				    BuiltInCategory.OST_MechanicalEquipment,
			//		
			//				    BuiltInCategory.OST_PipeFitting,
			//				    BuiltInCategory.OST_PlumbingFixtures,
			//				    BuiltInCategory.OST_SpecialityEquipment,
			//				    BuiltInCategory.OST_Sprinklers,
			//		
			//				};
			//			
			//				BuiltInCategory[] bics_finst_2 = new BuiltInCategory[] 	
			//				{
			//			
			//				    BuiltInCategory.OST_CableTrayFitting,
			//
			//				    BuiltInCategory.OST_ConduitFitting,
			//	
			//				    BuiltInCategory.OST_DuctFitting,
			//				    BuiltInCategory.OST_DuctTerminal,
			//				    BuiltInCategory.OST_ElectricalEquipment,
			//				    BuiltInCategory.OST_ElectricalFixtures,
			//				    BuiltInCategory.OST_LightingDevices,
			//				    BuiltInCategory.OST_LightingFixtures,
			//				    BuiltInCategory.OST_MechanicalEquipment,
			//		
			//				    BuiltInCategory.OST_PipeFitting,
			//				    BuiltInCategory.OST_PlumbingFixtures,
			//				    BuiltInCategory.OST_SpecialityEquipment,
			//				    BuiltInCategory.OST_Sprinklers,
			//		
			//				};
			#endregion

			IList<BuiltInCategory> bics_familyIns = UI_list2;
			//			IList<BuiltInCategory> bics_familyIns = bics_finst;

			IList<BuiltInCategory> bics_finst_4 = UI_list4;

			List<Element> clash_familyinstance = new List<Element>();



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

					clash_familyinstance.Add(elem);
				}

			}
			List<Element> clash_yesA = new List<Element>();

			for (int i = 0; i < clash_familyinstance.Count(); i++)
			{
				Element elem = clash_familyinstance[i];

				BoundingBoxXYZ elem_bb = elem.get_BoundingBox(null);

				IList<Element> salida = new List<Element>();


				foreach (BuiltInCategory bic in bics_finst_4)
				{

					Outline outline_2 = new Outline(elem_bb.Min, elem_bb.Max);
					BoundingBoxIntersectsFilter bbfilter_2 = new BoundingBoxIntersectsFilter(outline_2);

					ElementClassFilter famFil = new ElementClassFilter(typeof(FamilyInstance));

					ElementCategoryFilter Categoryfilter = new ElementCategoryFilter(bic);

					LogicalAndFilter InstancesFilter = new LogicalAndFilter(famFil, Categoryfilter);

					FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, doc.ActiveView.Id); // 2 muros que intercepta con la ventana siguiente

					IList<Element> elementss = coll_outline_2.WherePasses(bbfilter_2).WherePasses(InstancesFilter).ToElements();

					//					FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, doc.ActiveView.Id); // 2 muros que intercepta con la ventana siguiente
					//					IList<Element> elements = coll_outline_2.WherePasses(bbfilter_2).OfClass(typeof(FamilyInstance)).ToElements();


					//
					//					
					//					foreach (Element n in elements) 
					//					{
					//						salida.Add(n);
					//					}

					if (elementss.Count() > 0) // clash
					{

						if (!clash_yesA.Contains(elem))
						{


							foreach (Element pp in elementss)
							{
								if (!(pp.Id == elem.Id))
								{

									clash_yesA.Add(elem);

									Parameter param = elem.LookupParameter("Clash Category");
									Parameter paramID = elem.LookupParameter("ID Element");
									string elemcategory = elementss.First().Category.Name.ToString() + " / ID: " + elementss.First().Id.ToString();

									using (Transaction t = new Transaction(doc, "Clash Category"))
									{
										t.Start();

										param.Set(elemcategory);

										t.Commit();
									}
								}
							}

						}

					}


				}




			}



			#region borrador

			//	 		string numero_ductos = clashID_familyinstance.Count().ToString();
			//	 		string mensaje = "Iteraciones de elementos : " + numero_ductos + "\n\n" + Environment.NewLine;
			//	 		
			//	 		
			//			foreach (ElementId eID in clashID_familyinstance) 
			//	 		{
			//	 
			//	 			//ElementId eID = e.Id;
			//	 			Element e = doc.GetElement(eID);
			//				//LocationPoint p = e.Location as LocationPoint;
			//	 			
			//				//XYZ xyz = new XYZ(p.Point.X, p.Point.Y,p.Point.Z);
			//				
			//				//XYZ p = so.ComputeCentroid();
			//				//FamilyInstance efi = e as FamilyInstance;
			//				
			//				GeometryElement geomElement = e.get_Geometry(new Options() );
			//				
			//				GeometryInstance geomFinstace = geomElement.First() as GeometryInstance;
			//
			//				GeometryElement gSymbol = geomFinstace.GetSymbolGeometry();
			//				
			//				//Solid solid = gSymbol.First() as Solid;
			//	 
			//	 			Solid solid = null;
			//	 			foreach(GeometryObject geomObj in gSymbol)
			//	 			{
			//	 				solid = geomObj as Solid;
			//	 				if( solid != null ) break;
			//	 			}
			//
			////	 			IList<BuiltInCategory> bics_fi2 = UI_list4;
			//	 			IList<BuiltInCategory> bics_fi2 = bics_finst;
			//	 			
			//	 			
			//	 			foreach (BuiltInCategory bic in bics_fi2) 
			//	 			{
			//					// Find intersections between family instances and a selected element
			//		 			// category Mechanical Euqipment
			//		 			ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
			//		 			// Create a category filter for Mechanical Euqipment
			//		 			ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(bic);
			//		 
			//		 			LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);
			//		 
			//		 			ICollection<ElementId> collectoreID = new List<ElementId>();
			//		 			if (collectoreID.Contains(e.Id)==false)
			//		 			{
			//						collectoreID.Add(eID);
			//					}
			////		 			
			////		 
			//		 			ExclusionFilter filter = new ExclusionFilter(collectoreID);
			//					FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id);
			//		 			
			//					if (bic == BuiltInCategory.OST_CableTray) 
			//		 			{
			//						collector.OfClass(typeof(CableTray));
			//					}
			//		 			if (bic == BuiltInCategory.OST_DuctCurves) 
			//		 			{
			//						collector.OfClass(typeof(Duct));
			//					}
			//		 			if (bic == BuiltInCategory.OST_PipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(Pipe));
			//					}
			//		 			if (bic == BuiltInCategory.OST_Conduit) 
			//		 			{
			//						collector.OfClass(typeof(Conduit));
			//					}
			//		 			if (bic == BuiltInCategory.OST_FlexPipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(FlexPipe));
			//					}
			//		 			if (bic == BuiltInCategory.OST_FlexPipeCurves) 
			//		 			{
			//						collector.OfClass(typeof(FlexPipe));
			//					}
			//
			//		 			collector.WherePasses(DU2InstancesFilter);
			//		 			collector.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
			//		 			collector.WherePasses(filter);
			//		 			
			//		 			if (collector.Count() > 0) 
			//			 		{
			//		 				if (clash_yesA.Contains(e)==false)
			//			 			{
			//							clash_yesA.Add(e);
			//						}
			//			        }
			//			 		
			//			 		foreach (Element elem in collector) 
			//			 		{
			//		 				if (clash_yesA.Contains(elem)==false)
			//			 			{
			//							clash_yesA.Add(elem);
			//						}
			//					}
			//		 			mensaje = mensaje + Environment.NewLine + collector.Count().ToString() + " Family Instance intersect with the selected element ("
			//			 																	+ e.Name.ToString() 
			//			 																	+ e.Category.Name.ToString() + " id:"
			//																 				+ eID.ToString() + ")"
			//																				+ Environment.NewLine;
			//				}
			//	 		}
			#endregion

			string msg = "";
			foreach (Element elem in clash_yesA)
			{
				msg += "Nombre : " + elem.Name.ToString() + " ID : " + elem.Id.ToString() + "Categoria : " + elem.Category.Name.ToString() + Environment.NewLine;
				// set clash YES
				Parameter param = elem.LookupParameter("Clash");
				string clash = "YES";
				using (Transaction t = new Transaction(doc, "Clash YES"))
				{
					t.Start();
					if (!(param.AsString() == "YES"))
					{
						param.Set(clash);
					}

					t.Commit();
				}
			}

		} // Family Instance vs Family Instance
		
		

	}
}
