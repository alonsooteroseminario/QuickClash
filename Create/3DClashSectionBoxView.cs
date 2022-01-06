using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class ClashSectionBoxView
    {

        public static void Do(ExternalCommandData commandData, string view_name)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            using (Transaction t = new Transaction(doc, "Create clash 3d view"))
            {
                t.Start();
                double Min_X = double.MaxValue;
                double Min_Y = double.MaxValue;
                double Min_Z = double.MaxValue;
                double Max_X = double.MinValue;
                double Max_Y = double.MinValue;
                double Max_Z = double.MinValue;
                List<ElementId> ids = new List<ElementId>();
                IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element);
                foreach (Reference reference in references)
                {
                    Element e = doc.GetElement(reference);
                    ElementId eId = e.Id;
                    ids.Add(eId);
                }
                foreach (ElementId id in ids)
                {
                    Element elm = doc.GetElement(id);
                    BoundingBoxXYZ box = elm.get_BoundingBox(activeView);
                    if (box.Max.X > Max_X)
                    {
                        Max_X = box.Max.X;
                    }
                    if (box.Max.Y > Max_Y)
                    {
                        Max_Y = box.Max.Y;
                    }
                    if (box.Max.Z > Max_Z)
                    {
                        Max_Z = box.Max.Z;
                    }
                    if (box.Min.X < Min_X)
                    {
                        Min_X = box.Min.X;
                    }
                    if (box.Min.Y < Min_Y)
                    {
                        Min_Y = box.Min.Y;
                    }
                    if (box.Min.Z < Min_Z)
                    {
                        Min_Z = box.Min.Z;
                    }
                }
                XYZ Max = new XYZ(Max_X, Max_Y, Max_Z);
                XYZ Min = new XYZ(Min_X, Min_Y, Min_Z);
                BoundingBoxXYZ myBox = new BoundingBoxXYZ();
                myBox.Min = Min;
                myBox.Max = Max;
                ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
                                             OfClass(typeof(ViewFamilyType)).
                                             Cast<ViewFamilyType>()
                                                 where v.ViewFamily == ViewFamily.ThreeDimensional
                                                 select v).First();
                View3D dupleView = View3D.CreateIsometric(doc, viewFamilyType.Id);
                FilteredElementCollector DUFcoll = new FilteredElementCollector(doc).OfClass(typeof(View3D));
                List<View3D> views = new List<View3D>();
                List<View3D> views_COORD = new List<View3D>();
                int numero = 1;
                foreach (View3D ve in DUFcoll)
                {
                    views.Add(ve);
                }
                for (int i = 0; i < views.Count(); i++)
                {
                    View3D ve = views[i];
                    if (ve.Name.Contains(view_name))
                    {
                        views_COORD.Add(ve);

                    }
                }
                for (int i = 0; i < views_COORD.Count(); i++)
                {
                    View3D ve = views_COORD[i];
                    if (ve.Name.Contains(view_name + "  Copy "))
                    {
                        numero++;
                    }
                    else
                    {
                        numero = 1;
                    }
                }
                dupleView.Name = view_name + "  Copy " + (numero).ToString();
                (dupleView as View3D).SetSectionBox(myBox);
                dupleView.DisplayStyle = DisplayStyle.Shading;
                dupleView.DetailLevel = ViewDetailLevel.Fine;
                List<Element> riv = new List<Element>();
                FilteredElementCollector links = new FilteredElementCollector(doc, dupleView.Id);
                ElementCategoryFilter linkFilter = new ElementCategoryFilter(BuiltInCategory.OST_RvtLinks);
                links.WhereElementIsNotElementType();
                links.WherePasses(linkFilter);
                riv.AddRange(links.ToElements());
                t.Commit();
                uidoc.ActiveView = dupleView;
                ClashFilterMultipleElementsInView_UI.Do(commandData, dupleView);
                ClashSOLVEDFilterMultipleElementsInView_UI.Do(commandData, dupleView);
            }
        }
    }
}
