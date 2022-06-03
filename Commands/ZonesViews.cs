using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class ZonesViews : IExternalCommand
    {
        public static Window RevitCommandWindow { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var activeView = uidoc.ActiveView;

            Dictionary<string, XYZ> intersectionPoints = GetIntersections.Do(doc);

            List<View3D> tresDclashview = new List<View3D>();

            List<Element> clash_elements = new List<Element>();

            foreach (Element e in GetAllClashElements.Get(commandData))
            {
                clash_elements.Add(e);
            }

            try
            {
                foreach (KeyValuePair<string, XYZ> kp in intersectionPoints)
                {
                    List<Element> clash_elements_zone = new List<Element>();
                    List<Element> clash_elements_zone_no = new List<Element>();
                    List<ElementId> clash_elements_zone_ids = new List<ElementId>();

                    string intersc = kp.Value.ToString();

                    string intersec_key = kp.Key.ToString();

                    foreach (Element elem in clash_elements)
                    {
                        if (elem.LookupParameter("Clash Grid Location").AsString() == intersec_key)
                        {
                            clash_elements_zone.Add(elem);
                        }
                        else
                        {
                            clash_elements_zone_no.Add(elem);
                        }
                    }

                    if (clash_elements_zone.Count() > 1)
                    {
                        ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
                                                         OfClass(typeof(ViewFamilyType)).
                                                         Cast<ViewFamilyType>()
                                                         where v.ViewFamily == ViewFamily.ThreeDimensional
                                                         select v).First();

                        using (Transaction t = new Transaction(doc, "Create clash 3d view"))
                        {
                            t.Start();

                            View3D clashview = View3D.CreateIsometric(doc, viewFamilyType.Id);

                            foreach (Element elem in clash_elements_zone)
                            {
                                clash_elements_zone_ids.Add(elem.Id);
                            }

                            double Min_Z1 = double.MaxValue;

                            double Min_X1 = double.MaxValue;
                            double Min_Y1 = double.MaxValue;

                            double Max_X1 = double.MinValue;
                            double Max_Y1 = double.MinValue;
                            double Max_Z1 = double.MinValue;

                            List<double> lista_Min_Z1 = new List<double>();
                            List<double> lista_Min_X1 = new List<double>();
                            List<double> lista_Min_Y1 = new List<double>();

                            List<double> lista_Max_X1 = new List<double>();
                            List<double> lista_Max_Y1 = new List<double>();
                            List<double> lista_Max_Z1 = new List<double>();

                            foreach (ElementId id in clash_elements_zone_ids)
                            {
                                Element elm = doc.GetElement(id);

                                BoundingBoxXYZ box = elm.get_BoundingBox(null);

                                if (box.Max.X > Max_X1)
                                {
                                    Max_X1 = box.Max.X;
                                    lista_Max_X1.Add(Max_X1);
                                }
                                if (box.Max.Y > Max_Y1)
                                {
                                    Max_Y1 = box.Max.Y;
                                    lista_Max_Y1.Add(Max_Y1);
                                }
                                if (box.Max.Z > Max_Z1)
                                {
                                    Max_Z1 = box.Max.Z;
                                    lista_Max_Z1.Add(Max_Z1);
                                }

                                if (box.Min.X < Min_X1)
                                {
                                    Min_X1 = box.Min.X;
                                    lista_Min_X1.Add(Min_X1);
                                }
                                if (box.Min.Y < Min_Y1)
                                {
                                    Min_Y1 = box.Min.Y;
                                    lista_Min_Y1.Add(Min_Y1);
                                }
                                if (box.Min.Z < Min_Z1)
                                {
                                    Min_Z1 = box.Min.Z;
                                    lista_Min_Z1.Add(Min_Z1);
                                }
                            }

                            lista_Max_X1.Sort();
                            lista_Max_Y1.Sort();
                            lista_Max_Z1.Sort();
                            lista_Min_X1.Sort();
                            lista_Min_Y1.Sort();
                            lista_Min_Z1.Sort();

                            lista_Max_X1.Reverse();
                            lista_Max_Y1.Reverse();
                            lista_Max_Z1.Reverse();

                            XYZ Max1 = new XYZ(lista_Max_X1.First(), lista_Max_Y1.First(), lista_Max_Z1.First());
                            XYZ Min1 = new XYZ(lista_Min_X1.First(), lista_Min_Y1.First(), lista_Min_Z1.First());

                            BoundingBoxXYZ bb_zone = new BoundingBoxXYZ();

                            bb_zone.Min = Min1;
                            bb_zone.Max = Max1;

                            BoundingBoxXYZ elem_bb = bb_zone;

                            double offset = 2;

                            var SectionBox = clashview.GetSectionBox();
                            var vMax = SectionBox.Max + SectionBox.Transform.Origin;
                            var vMin = SectionBox.Min + SectionBox.Transform.Origin;
                            var bbMax = elem_bb.Max;
                            var bbMin = elem_bb.Min;

                            double Max_X = elem_bb.Max.X;

                            double Max_Y = elem_bb.Max.Y;

                            double Max_Z = elem_bb.Max.Z;

                            double Min_X = elem_bb.Min.X;

                            double Min_Y = elem_bb.Min.Y;

                            double Min_Z = elem_bb.Min.Z;

                            XYZ Max = new XYZ(Max_X + offset, Max_Y + offset, Max_Z + offset);
                            XYZ Min = new XYZ(Min_X - offset, Min_Y - offset, Min_Z - offset);

                            BoundingBoxXYZ myBox = new BoundingBoxXYZ();

                            myBox.Min = Min;
                            myBox.Max = Max;

                            clashview.SetSectionBox(myBox);

                            clashview.DisplayStyle = DisplayStyle.Shading;
                            clashview.DetailLevel = ViewDetailLevel.Fine;

                            string number = (tresDclashview.Count() + 1).ToString();

                            clashview.Name = number + ".- Zone    " + intersec_key + "     " + " Section Box    ";

                            t.SetName("Create view " + clashview.Name);

                            clashview.SetSectionBox(myBox);
                            tresDclashview.Add(clashview);
                            t.Commit();
                        }
                    }
                }
                uidoc.ActiveView = tresDclashview.First();
            }
            catch (Exception ex)
            {
                throw;
            }

            return Result.Succeeded;
        }
    }
}