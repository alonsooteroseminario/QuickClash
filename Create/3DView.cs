using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class View
    {
        /// <summary>
        /// Devuelve la lista de valores del diccionario ingresado como parámetro.
        /// </summary>
        /// <param>List of BuiltCategories.</param>
        public static void Create(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
                                                 OfClass(typeof(ViewFamilyType)).
                                                 Cast<ViewFamilyType>()
                                             where v.ViewFamily == ViewFamily.ThreeDimensional
                                             select v).First();

            using (Transaction t = new Transaction(doc, "Create COORD view"))
            {
                t.Start();

                View3D COORD = View3D.CreateIsometric(doc, viewFamilyType.Id);

                COORD.DisplayStyle = DisplayStyle.Shading;
                COORD.DetailLevel = ViewDetailLevel.Fine;

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
                    if (ve.Name.Contains("COORD"))
                    {
                        views_COORD.Add(ve);
                    }

                }

                if (views_COORD.Count() == 0)
                {
                    COORD.Name = "COORD";
                }
                else
                {
                    for (int i = 0; i < views_COORD.Count(); i++)
                    {
                        View3D ve = views_COORD[i];
                        if (ve.Name.Contains("COORD" + "  Copy "))
                        {
                            numero = numero + 1;
                        }
                        else
                        {
                            numero = 1;
                        }
                    }
                    COORD.Name = "COORD" + "  Copy " + (numero).ToString();
                }

                List<Element> riv = new List<Element>();
                FilteredElementCollector links = new FilteredElementCollector(doc, COORD.Id);
                ElementCategoryFilter linkFilter = new ElementCategoryFilter(BuiltInCategory.OST_RvtLinks);
                links.WhereElementIsNotElementType();
                links.WherePasses(linkFilter);
                riv.AddRange(links.ToElements());

                t.Commit();
                uidoc.ActiveView = COORD;
            }
        }

        public static View3D Copy(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            View3D viewer = null;

            using (Transaction t = new Transaction(doc, "Duplicate View"))
            {
                t.Start();

                for (int i = 1; i < 2; i++)
                {
                    viewer = (View3D)doc.GetElement(activeView.Duplicate(ViewDuplicateOption.WithDetailing));
                }
                t.Commit();
            }

            uidoc.ActiveView = viewer;

            return (View3D)viewer;
        }

        public static View3D CopySelection(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            // Select element input
            List<Element> lista_SelectElements = new List<Element>();
            IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element, "Select the Element you want to analyze");
            foreach (Reference reference in references)
            {
                Element e = doc.GetElement(reference);
                lista_SelectElements.Add(e);
            }


            View3D viewer = null;

            using (Transaction t = new Transaction(doc, "Duplicate View"))
            {
                t.Start();

                for (int i = 1; i < 2; i++)
                {
                    viewer = (View3D)doc.GetElement(activeView.Duplicate(ViewDuplicateOption.WithDetailing));
                }
                t.Commit();
            }

            //apply section box to selection
            using (Transaction t = new Transaction(doc, "Create Section Box"))
            {
                t.Start();
                double Min_Z = double.MaxValue;
                double Min_X = double.MaxValue;
                double Min_Y = double.MaxValue;

                double Max_X = double.MinValue;
                double Max_Y = double.MinValue;
                double Max_Z = double.MinValue;
                foreach (var elem in lista_SelectElements)
                {
                    

                    BoundingBoxXYZ box = elem.get_BoundingBox(null);
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

                BoundingBoxXYZ myBox = new BoundingBoxXYZ
                {
                    Min = Min,
                    Max = Max
                };


                viewer.SetSectionBox(myBox);

                t.Commit();
            }
            
            uidoc.ActiveView = viewer;
            viewer.DisplayStyle = DisplayStyle.Shading;
            viewer.DetailLevel = ViewDetailLevel.Fine;

            return (View3D)viewer;
        }
    }
}
