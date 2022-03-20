using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

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

            IList<Element> ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
            List<IList<Element>> elements = new List<IList<Element>>
            {
                ducts,
                pipes,
                conduits,
                cabletrays,
                flexducts,
                flexpipes
            };
            foreach (IList<Element> elems in elements)
            {
                foreach (var elem in elems)
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
            foreach (Element elem in clash)
            {
                clashID_elements.Add(elem.Id);
            }
            IList<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                IList<Element> familyInstance = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");

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
            Dictionary<string, XYZ> intersectionPoints = GetIntersections.Do(doc);
            string intersection = "??";
            foreach (ElementId eid in clashID_elements)
            {
                Element e = doc.GetElement(eid);
                Options op = new Options();
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

        public static void DoAllDocument(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;

            IList<Element> clash = new List<Element>();
            IList<Element> clash_no = new List<Element>();
            IList<Element> clash_familyinstance = new List<Element>();
            IList<ElementId> clashID_elements = new List<ElementId>();
            IList<ElementId> clashID_familyinstance = new List<ElementId>();

            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
            List<IList<Element>> elements = new List<IList<Element>>
            {
                ducts,
                pipes,
                conduits,
                cabletrays,
                flexducts,
                flexpipes
            };
            foreach (IList<Element> elems in elements)
            {
                foreach (var elem in elems)
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
            foreach (Element elem in clash)
            {
                clashID_elements.Add(elem.Id);
            }
            IList<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                IList<Element> familyInstance = GetElements.ElementsByBuiltCategory(commandData, bic, "family_instances_all");
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
            Dictionary<string, XYZ> intersectionPoints = GetIntersections.Do(doc);
            string intersection = "??";
            foreach (ElementId eid in clashID_elements)
            {
                Element e = doc.GetElement(eid);
                Options op = new Options();
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
            Dictionary<string, XYZ> intersectionPoints = GetIntersections.Do(doc);
            string intersection = "??";
            foreach (ElementId eid in clashID_elements)
            {
                Element e = doc.GetElement(eid);
                Options op = new Options();
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
    }
}
