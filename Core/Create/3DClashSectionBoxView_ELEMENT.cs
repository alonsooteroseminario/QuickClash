using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

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
            List<Element> clash_elements = new List<Element>();
            foreach (Element e in GetAllClashElements_OnlyActiveView.Do(commandData))
            {
                clash_elements.Add(e);
            }
            if (clash_elements.Count() == 0)
            {
                TaskDialog.Show("Dynoscript", "No se encontraron Elementos con Clash en la Vista Activa!");
                return;
            }
            else
            {
                ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
                                                 OfClass(typeof(ViewFamilyType)).
                                                 Cast<ViewFamilyType>()
                                                 where v.ViewFamily == ViewFamily.ThreeDimensional
                                                 select v).First();
                List<View3D> tresDclashview = new List<View3D>();
                using (Transaction t = new Transaction(doc, "Create clash 3d view"))
                {
                    int ctr = 0;
                    foreach (Element elem in clash_elements)
                    {
                        t.Start();
                        View3D clashview = View3D.CreateIsometric(doc, viewFamilyType.Id);
                        Parameter param = elem.LookupParameter("Clash Category");
                        string param_string = param.AsString();
                        string param_string2 = param_string.Replace(':', '_');
                        clashview.Name = "COORD - Section Box  " + elem.Name.ToString() + " / "
                                                                + "ID  " + elem.Id.ToString() + " / "
                                                                + " Clash Category " + param_string2;
                        t.SetName("Create view " + clashview.Name);
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
                        BoundingBoxXYZ myBox = new BoundingBoxXYZ
                        {
                            Min = Min,
                            Max = Max
                        };
                        clashview.SetSectionBox(myBox);
                        clashview.DisplayStyle = DisplayStyle.Shading;
                        clashview.DetailLevel = ViewDetailLevel.Fine;
                        t.Commit();
                        tresDclashview.Add(clashview);
                        ctr++;
                    }
                    uidoc.ActiveView = tresDclashview.First();
                    ClashFilterMultipleElementsInMultipleViews_UI.Do(commandData, tresDclashview);
                    ClashSOLVEDFilterMultipleElementsInMultipleViews_UI.Do(commandData, tresDclashview);
                }
            }
        }
    }
}
