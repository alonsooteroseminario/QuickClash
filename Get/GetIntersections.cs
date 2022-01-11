using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace QuickClash
{
    public static class GetIntersections
    {
        public static Dictionary<string, XYZ> Do(Document _doc)
        {
            string coordinate;
            string coordA;
            string coordB;
            string coordC;
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


                    SetComparisonResult result = c.Intersect(c2, out IntersectionResultArray results);

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
