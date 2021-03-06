using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class Intersect
    {
        public static List<Element> Get_allElements(ExternalCommandData commandData)
        {
            List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(false);

            List<Element> allElements = new List<Element>();

            foreach (BuiltInCategory bic in UI_list1)
            {
                if (bic == BuiltInCategory.OST_CableTray)
                {
                    IList<Element> cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "cabletrays");
                    foreach (Element i in cabletrays)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_Conduit)
                {
                    IList<Element> conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "conduits");
                    foreach (Element i in conduits)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_DuctCurves)
                {
                    IList<Element> ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "ducts");
                    foreach (Element i in ducts)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_PipeCurves)
                {
                    IList<Element> pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "pipes");
                    foreach (Element i in pipes)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_FlexDuctCurves)
                {
                    IList<Element> flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexducts");
                    foreach (Element i in flexducts)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_FlexPipeCurves)
                {
                    IList<Element> flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexpipes");
                    foreach (Element i in flexpipes)
                    {
                        allElements.Add(i);
                    }
                }
            }

            return allElements;
        }

        // Element vs Element

        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void MultipleElementsToMultipleCategory(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Autodesk.Revit.DB.View activeView = uidoc.ActiveView;

            List<BuiltInCategory> UI_list3 = GetLists.BuiltCategories(false);

            List<Element> allElements = Get_allElements(commandData);

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
                }

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
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }

        // Element vs. FamilyInstance

        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void MultipleElementsToMultipleFamilyInstances(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Autodesk.Revit.DB.View activeView = uidoc.ActiveView;
            List<BuiltInCategory> UI_list4 = GetLists.BuiltCategories(true);

            List<Element> allElements = Get_allElements(commandData);

            List<Element> clash_yesA = new List<Element>();

            List<Element> clash_yesA_element = new List<Element>();
            List<Element> clash_yesA_familyinstance = new List<Element>();

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
                }

                IList<BuiltInCategory> bics_fi = UI_list4;

                foreach (BuiltInCategory bic in bics_fi)
                {
                    ElementClassFilter DUFilter = new ElementClassFilter(typeof(FamilyInstance));
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
                        if (clash_yesA_element.Contains(e) == false)
                        {
                            clash_yesA_element.Add(e);
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
                        if (clash_yesA_familyinstance.Contains(elem) == false)
                        {
                            clash_yesA_familyinstance.Add(elem);
                            clash_yesA.Add(elem);
                        }
                    }
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }

        //FamilyIntance vs Familyinstance

        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void MultipleFamilyInstanceToMultipleFamilyInstances_BBox(ExternalCommandData commandData) // Family Instance vs Family Instance
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uiapp.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View activeView = uidoc.ActiveView;
            List<BuiltInCategory> bics_finst = GetLists.BuiltCategories(true);
            List<BuiltInCategory> bics_finst_2 = GetLists.BuiltCategories(true);

            List<Element> clash_familyinstance = new List<Element>();

            foreach (BuiltInCategory bic in bics_finst)
            {
                IList<Element> family_instances_all = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");
                foreach (Element elem in family_instances_all)
                {
                    clash_familyinstance.Add(elem);
                }
            }
            List<Element> clash_yesA = new List<Element>();

            for (int i = 0; i < clash_familyinstance.Count(); i++)
            {
                Element elem = clash_familyinstance[i];
                BoundingBoxXYZ elem_bb = elem.get_BoundingBox(null);

                foreach (BuiltInCategory bic in bics_finst_2)
                {
                    Outline outline_2 = new Outline(elem_bb.Min, elem_bb.Max);
                    BoundingBoxIntersectsFilter bbfilter_2 = new BoundingBoxIntersectsFilter(outline_2);
                    ElementClassFilter famFil = new ElementClassFilter(typeof(FamilyInstance));
                    ElementCategoryFilter Categoryfilter = new ElementCategoryFilter(bic);
                    LogicalAndFilter InstancesFilter = new LogicalAndFilter(famFil, Categoryfilter);
                    FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, activeView.Id);
                    IList<Element> elementss = coll_outline_2.WherePasses(bbfilter_2).WherePasses(InstancesFilter).ToElements();

                    if (elementss.Count() > 0)
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
                                        _ = t.Start();
                                        _ = param.Set(elemcategory);
                                        _ = t.Commit();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }

        //Elements vs Links Elements
        public static void MultipleElementsToLinksElements(ExternalCommandData commandData, List<Element> lista_links)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<BuiltInCategory> UI_list3 = GetLists.BuiltCategories(false);

            List<Element> allElements = Get_allElements(commandData);

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
                }

                ICollection<ElementId> collectoreID = new List<ElementId>
                {
                    eID
                };

                foreach (RevitLinkInstance link in lista_links)
                {
                    foreach (BuiltInCategory bic in UI_list3)
                    {
                        if (bic == BuiltInCategory.OST_CableTray)
                        {
                            ElementClassFilter DUFilter4 = new ElementClassFilter(typeof(CableTray));
                            ElementCategoryFilter DU2Categoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
                            LogicalAndFilter DU2InstancesFilter4 = new LogicalAndFilter(DUFilter4, DU2Categoryfilter4);
                            ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector4 = new FilteredElementCollector(link.GetLinkDocument());
                            _ = collector4.OfClass(typeof(CableTray));
                            _ = collector4.WherePasses(DU2InstancesFilter4);
                            _ = collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements();
                            _ = collector4.WherePasses(filter4);
                            if (collector4.Count() > 0)
                            {
                                Parameter param = e.LookupParameter("Clash Category");
                                Parameter paramID = e.LookupParameter("ID Element");
                                string elemcategory = collector4.First().Category.Name.ToString() + " / ID: " + collector4.First().Id.ToString();
                                using (Transaction t = new Transaction(doc, "Clash Category"))
                                {
                                    _ = t.Start();
                                    _ = param.Set(elemcategory);
                                    _ = t.Commit();
                                }
                                if (!clash_yesA.Contains(e))
                                {
                                    clash_yesA.Add(e);
                                }
                            }
                        }
                        if (bic == BuiltInCategory.OST_Conduit)
                        {
                            ElementClassFilter DUFilter3 = new ElementClassFilter(typeof(Conduit));
                            ElementCategoryFilter DU2Categoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
                            LogicalAndFilter DU2InstancesFilter3 = new LogicalAndFilter(DUFilter3, DU2Categoryfilter3);
                            ExclusionFilter filter3 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector3 = new FilteredElementCollector(link.GetLinkDocument());
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
                        }
                        if (bic == BuiltInCategory.OST_DuctCurves)
                        {
                            ElementClassFilter DUFilter = new ElementClassFilter(typeof(Duct));
                            ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
                            LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);
                            ExclusionFilter filter = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector = new FilteredElementCollector(link.GetLinkDocument());
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
                        }
                        if (bic == BuiltInCategory.OST_PipeCurves)
                        {
                            ElementClassFilter DUFilter2 = new ElementClassFilter(typeof(Pipe));
                            ElementCategoryFilter DU2Categoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
                            LogicalAndFilter DU2InstancesFilter2 = new LogicalAndFilter(DUFilter2, DU2Categoryfilter2);
                            ExclusionFilter filter2 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector2 = new FilteredElementCollector(link.GetLinkDocument());
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
                        }
                        if (bic == BuiltInCategory.OST_FlexDuctCurves)
                        {
                            ElementClassFilter DUFilter5 = new ElementClassFilter(typeof(FlexDuct));
                            ElementCategoryFilter DU2Categoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
                            LogicalAndFilter DU2InstancesFilter5 = new LogicalAndFilter(DUFilter5, DU2Categoryfilter5);
                            ExclusionFilter filter5 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector5 = new FilteredElementCollector(link.GetLinkDocument());
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
                        }
                        if (bic == BuiltInCategory.OST_FlexPipeCurves)
                        {
                            ElementClassFilter DUFilter6 = new ElementClassFilter(typeof(FlexPipe));
                            ElementCategoryFilter DU2Categoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
                            LogicalAndFilter DU2InstancesFilter6 = new LogicalAndFilter(DUFilter6, DU2Categoryfilter6);
                            ExclusionFilter filter6 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector6 = new FilteredElementCollector(link.GetLinkDocument());
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
                        }
                    }
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }

        public static void MultipleElementsToLinksFamilyInstance(ExternalCommandData commandData, List<Element> lista_links)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Element> allElements = Get_allElements(commandData);

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
                }

                ICollection<ElementId> collectoreID = new List<ElementId>();
                collectoreID.Add(eID);

                foreach (RevitLinkInstance link in lista_links)
                {
                    Document docLink = link.GetLinkDocument();
                    ElementId activeLinkViewId = link.Id;
                    ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
                    FilteredElementCollector collector4 = new FilteredElementCollector(docLink);
                    _ = collector4.OfClass(typeof(FamilyInstance));
                    _ = collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements();
                    _ = collector4.WherePasses(filter4);

                    if (collector4.Count() > 0)
                    {
                        Parameter param = e.LookupParameter("Clash Category");
                        Parameter paramID = e.LookupParameter("ID Element");
                        string elemcategory = collector4.First().Category.Name.ToString() + " / ID: " + collector4.First().Id.ToString();
                        using (Transaction t = new Transaction(doc, "Clash Category"))
                        {
                            _ = t.Start();
                            _ = param.Set(elemcategory);
                            _ = t.Commit();
                        }
                        if (!clash_yesA.Contains(e))
                        {
                            clash_yesA.Add(e);
                        }
                    }
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }

        public static void MultipleFamilyInstanceToLinksElements(ExternalCommandData commandData, List<Element> lista_links)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(true);
            List<BuiltInCategory> UI_list3 = GetLists.BuiltCategories(false);

            List<Element> allElements = new List<Element>();
            List<Element> familyinstance = new List<Element>();

            foreach (BuiltInCategory bic in UI_list1)
            {
                IList<Element> familyInstances = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");
                foreach (Element item in familyInstances)
                {
                    familyinstance.Add(item);
                }
            }

            foreach (Element i in familyinstance)
            {
                if (!allElements.Contains(i))
                {
                    allElements.Add(i);
                }
            }

            List<Element> clash_yesA = new List<Element>();

            foreach (Element e in allElements)
            {
                ElementId eID = e.Id;

                ICollection<ElementId> collectoreID = new List<ElementId>
                {
                    eID
                };

                foreach (RevitLinkInstance link in lista_links)
                {
                    foreach (BuiltInCategory bic in UI_list3)
                    {
                        if (bic == BuiltInCategory.OST_CableTray)
                        {
                            ElementClassFilter DUFilter4 = new ElementClassFilter(typeof(CableTray));
                            ElementCategoryFilter DU2Categoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
                            LogicalAndFilter DU2InstancesFilter4 = new LogicalAndFilter(DUFilter4, DU2Categoryfilter4);
                            ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector4 = new FilteredElementCollector(link.GetLinkDocument());
                            collector4.OfClass(typeof(CableTray));
                            collector4.WherePasses(DU2InstancesFilter4);
                            //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                            collector4.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                        }
                        if (bic == BuiltInCategory.OST_Conduit)
                        {
                            ElementClassFilter DUFilter3 = new ElementClassFilter(typeof(Conduit));
                            ElementCategoryFilter DU2Categoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
                            LogicalAndFilter DU2InstancesFilter3 = new LogicalAndFilter(DUFilter3, DU2Categoryfilter3);
                            ExclusionFilter filter3 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector3 = new FilteredElementCollector(link.GetLinkDocument());
                            collector3.OfClass(typeof(Conduit));
                            collector3.WherePasses(DU2InstancesFilter3);
                            //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                            collector3.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                        }
                        if (bic == BuiltInCategory.OST_DuctCurves)
                        {
                            ElementClassFilter DUFilter = new ElementClassFilter(typeof(Duct));
                            ElementCategoryFilter DU2Categoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
                            LogicalAndFilter DU2InstancesFilter = new LogicalAndFilter(DUFilter, DU2Categoryfilter);
                            ExclusionFilter filter = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector = new FilteredElementCollector(link.GetLinkDocument());
                            collector.OfClass(typeof(Duct));
                            collector.WherePasses(DU2InstancesFilter);
                            //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                            collector.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                        }
                        if (bic == BuiltInCategory.OST_PipeCurves)
                        {
                            ElementClassFilter DUFilter2 = new ElementClassFilter(typeof(Pipe));
                            ElementCategoryFilter DU2Categoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
                            LogicalAndFilter DU2InstancesFilter2 = new LogicalAndFilter(DUFilter2, DU2Categoryfilter2);
                            ExclusionFilter filter2 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector2 = new FilteredElementCollector(link.GetLinkDocument());
                            collector2.OfClass(typeof(Pipe));
                            collector2.WherePasses(DU2InstancesFilter2);
                            //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                            collector2.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                        }
                        if (bic == BuiltInCategory.OST_FlexDuctCurves)
                        {
                            ElementClassFilter DUFilter5 = new ElementClassFilter(typeof(FlexDuct));
                            ElementCategoryFilter DU2Categoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
                            LogicalAndFilter DU2InstancesFilter5 = new LogicalAndFilter(DUFilter5, DU2Categoryfilter5);
                            ExclusionFilter filter5 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector5 = new FilteredElementCollector(link.GetLinkDocument());
                            collector5.OfClass(typeof(FlexDuct));
                            collector5.WherePasses(DU2InstancesFilter5);
                            //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                            collector5.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                        }
                        if (bic == BuiltInCategory.OST_FlexPipeCurves)
                        {
                            ElementClassFilter DUFilter6 = new ElementClassFilter(typeof(FlexPipe));
                            ElementCategoryFilter DU2Categoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);
                            LogicalAndFilter DU2InstancesFilter6 = new LogicalAndFilter(DUFilter6, DU2Categoryfilter6);
                            ExclusionFilter filter6 = new ExclusionFilter(collectoreID);
                            FilteredElementCollector collector6 = new FilteredElementCollector(link.GetLinkDocument());
                            collector6.OfClass(typeof(FlexPipe));
                            collector6.WherePasses(DU2InstancesFilter6);
                            //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                            collector6.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                        }
                    }
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }

        public static void MultipleFamilyInstanceToLinksFamilyInstance(ExternalCommandData commandData, List<Element> lista_links)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(true);

            List<Element> allElements = new List<Element>();
            List<Element> familyinstance = new List<Element>();

            foreach (BuiltInCategory bic in UI_list1)
            {
                IList<Element> familyInstances = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");
                foreach (Element item in familyInstances)
                {
                    familyinstance.Add(item);
                }
            }

            foreach (Element i in familyinstance)
            {
                if (!allElements.Contains(i))
                {
                    allElements.Add(i);
                }
            }

            List<Element> clash_yesA = new List<Element>();

            foreach (Element e in allElements)
            {
                ElementId eID = e.Id;
                ICollection<ElementId> collectoreID = new List<ElementId>
                {
                    eID
                };

                foreach (RevitLinkInstance link in lista_links)
                {
                    var docLink = link.GetLinkDocument();
                    var activeLinkViewId = link.Id;
                    ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
                    FilteredElementCollector collector4 = new FilteredElementCollector(docLink);
                    collector4.OfClass(typeof(FamilyInstance));
                    //collector4.WherePasses(new ElementIntersectsSolidFilter(solid)).ToElements(); // Apply intersection filter to find matches
                    collector4.WherePasses(new ElementIntersectsElementFilter(e)).ToElements(); // Apply intersection filter to find matches
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
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    _ = t.Start();
                    _ = param.Set("YES");
                    _ = t.Commit();
                }
            }
        }
    }
}