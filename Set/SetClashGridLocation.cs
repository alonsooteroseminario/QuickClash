﻿using Autodesk.Revit.ApplicationServices;
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
		public static void DoActiveView(ExternalCommandData commandData)
		{
			UIApplication uiApp = commandData.Application;
			Document doc = uiApp.ActiveUIDocument.Document;

			IList<Element> clash = new List<Element>();
			IList<Element> clash_no = new List<Element>();
			IList<Element> clash_familyinstance = new List<Element>();
			IList<ElementId> clashID_elements = new List<ElementId>();
			IList<ElementId> clashID_familyinstance = new List<ElementId>();

			IList<Element> ducts = Get.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
			IList<Element> pipes = Get.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
			IList<Element> conduits = Get.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_Conduit, "conduits");
			IList<Element> cabletrays = Get.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
			IList<Element> flexducts = Get.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
			IList<Element> flexpipes = Get.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");

			// ELEMENTS

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

			IList < BuiltInCategory > bics_familyIns = Lists.BuiltCategories(true);
			foreach (BuiltInCategory bic in bics_familyIns)
			{
				IList<Element> familyInstance = Get.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");

				foreach (Element elem in familyInstance)
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

		public static void DoAllDocument(ExternalCommandData commandData)
		{
			UIApplication uiApp = commandData.Application;
			Document doc = uiApp.ActiveUIDocument.Document;

			IList<Element> clash = new List<Element>();
			IList<Element> clash_no = new List<Element>();
			IList<Element> clash_familyinstance = new List<Element>();
			IList<ElementId> clashID_elements = new List<ElementId>();
			IList<ElementId> clashID_familyinstance = new List<ElementId>();

			IList<Element> ducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
			IList<Element> pipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
			IList<Element> conduits = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
			IList<Element> cabletrays = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
			IList<Element> flexducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
			IList<Element> flexpipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");

			// ELEMENTS

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

			IList<BuiltInCategory> bics_familyIns = Lists.BuiltCategories(true);
			foreach (BuiltInCategory bic in bics_familyIns)
			{
				IList<Element> familyInstance = Get.ElementsByBuiltCategory(commandData, bic, "family_instances_all");

				foreach (Element elem in familyInstance)
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

		public static void UI(ExternalCommandData commandData, IList<Element> clash_, IList<Element> clash_familyinstance_)
        {
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

			List<ElementId> clashID_elements = new List<ElementId>();
			List<ElementId> clashID_familyinstance = new List<ElementId>();

			IList<Element> clash = clash_;
			IList<Element> clash_familyinstance = clash_familyinstance_;

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
				Parameter param = e.LookupParameter("Clash Grid Location");
				string param_value = intersection;

				using (Transaction t = new Transaction(doc, "Set Clash grid location to element"))
				{
					t.Start();
					param.Set(param_value);
					t.Commit();
				}
			}
		}

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


	}
}
