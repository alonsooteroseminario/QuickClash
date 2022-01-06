using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class ClashSectionBoxView_LEVELS
    {

        public static void Do(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;
            IList<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(l => l.Elevation).ToList();
            ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
                                             OfClass(typeof(ViewFamilyType)).
                                             Cast<ViewFamilyType>()
                                             where v.ViewFamily == ViewFamily.ThreeDimensional
                                             select v).First();
            List<View3D> lista3dview = new List<View3D>();
            using (Transaction t = new Transaction(doc, "Create view"))
            {
                int ctr = 0;
                foreach (Level level in levels)
                {
                    t.Start();
                    View3D view = View3D.CreateIsometric(doc, viewFamilyType.Id);
                    view.Name = "COORD - Nivel " + level.Name;
                    t.SetName("Create view " + view.Name);
                    BoundingBoxXYZ boundingBoxXYZ = new BoundingBoxXYZ();
                    boundingBoxXYZ.Min = new XYZ(-50, -100, level.Elevation);
                    double zOffset = 0;
                    if (levels.Count > ctr + 1)
                        zOffset = levels.ElementAt(ctr + 1).Elevation;
                    else
                        zOffset = level.Elevation + 10;
                    boundingBoxXYZ.Max = new XYZ(200, 125, zOffset);
                    (view as View3D).SetSectionBox(boundingBoxXYZ);
                    lista3dview.Add(view);

                    if (!view.IsTemplate)
                    {
                        view.DisplayStyle = DisplayStyle.Shading;
                        view.DetailLevel = ViewDetailLevel.Fine;
                    }
                    t.Commit();
                    uidoc.ActiveView = view;
                    ctr++;
                }
            }
            foreach (View3D view in lista3dview)
            {
                ClashFilterMultipleElementsInView_UI.Do(commandData, view);
                ClashSOLVEDFilterMultipleElementsInView_UI.Do(commandData, view);
            }
        }
    }
}
