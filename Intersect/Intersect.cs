using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class Intersect
    {
        // Element vs Element

        /// <summary>
        /// 'true' para solo Active View y 'false' para todo el Documento
        /// </summary>
        /// <param>List of BuiltCategories.</param>
        public static void MultipleElementsToMultipleCategory_UI(ExternalCommandData commandData, List<BuiltInCategory> UI_list1_, List<BuiltInCategory> UI_list3_, bool ActiveViewBoolean)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            List<BuiltInCategory> UI_list1 = UI_list1_;
            List<BuiltInCategory> UI_list3 = UI_list3_;

            List<Element> allElements = new List<Element>();

            foreach (BuiltInCategory bic in UI_list1)
            {
                if (bic == BuiltInCategory.OST_CableTray)
                {
                    IList<Element> cabletrays = null;
                    if (ActiveViewBoolean)
                    {
                        cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "cabletrays");
                    }
                    else
                    {
                        cabletrays = GetElements.ElementsByBuiltCategory(commandData, bic, "cabletrays");
                    }
                    foreach (Element i in cabletrays)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_Conduit)
                {
                    IList<Element> conduits = null;
                    if (ActiveViewBoolean)
                    {
                        conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "conduits");
                    }
                    else
                    {
                        conduits = GetElements.ElementsByBuiltCategory(commandData, bic, "conduits");
                    }
                    foreach (Element i in conduits)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_DuctCurves)
                {
                    IList<Element> ducts = null;
                    if (ActiveViewBoolean)
                    {
                        ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "ducts");
                    }
                    else
                    {
                        ducts = GetElements.ElementsByBuiltCategory(commandData, bic, "ducts");
                    }
                    foreach (Element i in ducts)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_PipeCurves)
                {
                    IList<Element> pipes = null;
                    if (ActiveViewBoolean)
                    {
                        pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "pipes");
                    }
                    else
                    {
                        pipes = GetElements.ElementsByBuiltCategory(commandData, bic, "pipes");
                    }
                    foreach (Element i in pipes)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_FlexDuctCurves)
                {
                    IList<Element> flexducts = null;
                    if (ActiveViewBoolean)
                    {
                        flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexducts");
                    }
                    else
                    {
                        flexducts = GetElements.ElementsByBuiltCategory(commandData, bic, "flexducts");
                    }
                    foreach (Element i in flexducts)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_FlexPipeCurves)
                {
                    IList<Element> flexpipes = null;
                    if (ActiveViewBoolean)
                    {
                        flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexpipes");
                    }
                    else
                    {
                        flexpipes = GetElements.ElementsByBuiltCategory(commandData, bic, "flexpipes");
                    }
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
                        FilteredElementCollector collector4 = null;
                        if (ActiveViewBoolean)
                        {
                            collector4 = new FilteredElementCollector(doc, activeView.Id);
                        }
                        else
                        {
                            collector4 = new FilteredElementCollector(doc);
                        }
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
                        FilteredElementCollector collector3 = null;
                        if (ActiveViewBoolean)
                        {
                            collector3 = new FilteredElementCollector(doc, activeView.Id);
                        }
                        else
                        {
                            collector3 = new FilteredElementCollector(doc);
                        }
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
                        FilteredElementCollector collector = null;
                        if (ActiveViewBoolean)
                        {
                            collector = new FilteredElementCollector(doc, activeView.Id);
                        }
                        else
                        {
                            collector = new FilteredElementCollector(doc);
                        }
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
                        FilteredElementCollector collector2 = null;
                        if (ActiveViewBoolean)
                        {
                            collector2 = new FilteredElementCollector(doc, activeView.Id);
                        }
                        else
                        {
                            collector2 = new FilteredElementCollector(doc);
                        }
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
                        FilteredElementCollector collector5 = null;
                        if (ActiveViewBoolean)
                        {
                            collector5 = new FilteredElementCollector(doc, activeView.Id);
                        }
                        else
                        {
                            collector5 = new FilteredElementCollector(doc);
                        }
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
                        FilteredElementCollector collector6 = null;
                        if (ActiveViewBoolean)
                        {
                            collector6 = new FilteredElementCollector(doc, activeView.Id);
                        }
                        else
                        {
                            collector6 = new FilteredElementCollector(doc);
                        }
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
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }
            SetClashGridLocation.DoActiveView(commandData);

        } // Elem vs Elem // Only Active View
        public static void MultipleElementsToMultipleCategory(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(false);
            List<BuiltInCategory> UI_list3 = GetLists.BuiltCategories(false);

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
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }

            SetClashGridLocation.DoAllDocument(commandData);
        }


        // Element vs. FamilyInstance

        /// <summary>
        /// 'true' para solo Active View y 'false' para todo el Documento
        /// </summary>
        /// <param>List of BuiltCategories.</param>
        public static void MultipleElementsToMultipleFamilyInstances_UI(ExternalCommandData commandData, List<BuiltInCategory> UI_list1_, List<BuiltInCategory> UI_list4_, bool ActiveViewBoolean)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            List<BuiltInCategory> UI_list1 = UI_list1_;
            List<BuiltInCategory> UI_list4 = UI_list4_;

            List<Element> allElements = new List<Element>();

            foreach (BuiltInCategory bic in UI_list1)
            {
                if (bic == BuiltInCategory.OST_CableTray)
                {
                    IList<Element> cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "cabletrays");
                    if (!ActiveViewBoolean)
                    {
                        cabletrays = GetElements.ElementsByBuiltCategory(commandData, bic, "cabletrays");
                    }
                    foreach (Element i in cabletrays)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_Conduit)
                {
                    IList<Element> conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "conduits");
                    if (!ActiveViewBoolean)
                    {
                        conduits = GetElements.ElementsByBuiltCategory(commandData, bic, "conduits");
                    }
                    foreach (Element i in conduits)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_DuctCurves)
                {
                    IList<Element> ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "ducts");
                    if (!ActiveViewBoolean)
                    {
                        ducts = GetElements.ElementsByBuiltCategory(commandData, bic, "ducts");
                    }
                    foreach (Element i in ducts)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_PipeCurves)
                {
                    IList<Element> pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "pipes");
                    if (!ActiveViewBoolean)
                    {
                        pipes = GetElements.ElementsByBuiltCategory(commandData, bic, "pipes");
                    }
                    foreach (Element i in pipes)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_FlexDuctCurves)
                {
                    IList<Element> flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexducts");
                    if (!ActiveViewBoolean)
                    {
                        flexducts = GetElements.ElementsByBuiltCategory(commandData, bic, "flexducts");
                    }
                    foreach (Element i in flexducts)
                    {
                        allElements.Add(i);
                    }
                }
                if (bic == BuiltInCategory.OST_FlexPipeCurves)
                {
                    IList<Element> flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexpipes");
                    if (!ActiveViewBoolean)
                    {
                        flexpipes = GetElements.ElementsByBuiltCategory(commandData, bic, "flexpipes");
                    }
                    foreach (Element i in flexpipes)
                    {
                        allElements.Add(i);
                    }
                }
            }

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
                    if (solid != null) break;
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
                    if (!ActiveViewBoolean)
                    {
                        collector = new FilteredElementCollector(doc);
                    }
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
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }
            SetClashGridLocation.UI(commandData, clash_yesA_element, clash_yesA_familyinstance);

        } // Elem vs FamilyInstance // Only Active View
        public static void MultipleElementsToMultipleFamilyInstances(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;
            List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(false);
            List<BuiltInCategory> UI_list4 = GetLists.BuiltCategories(true);

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
                    if (solid != null) break;
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
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }

            SetClashGridLocation.UI(commandData, clash_yesA_element, clash_yesA_familyinstance);

        }


        //FamilyIntance vs Familyinstance

        /// <summary>
        /// 'true' para solo Active View y 'false' para todo el Documento
        /// </summary>
        /// <param>List of BuiltCategories.</param>
        public static void MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI(ExternalCommandData commandData, List<BuiltInCategory> UI_list2_, List<BuiltInCategory> UI_list4_, bool ActiveViewBoolean) // Family Instance vs Family Instance // Only Active View
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            List<BuiltInCategory> bics_finst = UI_list2_;
            List<BuiltInCategory> bics_finst_2 = UI_list4_;

            List<Element> clash_familyinstance = new List<Element>();

            foreach (BuiltInCategory bic in bics_finst)
            {
                IList<Element> family_instances_all = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");
                if (!ActiveViewBoolean)
                {
                    family_instances_all = GetElements.ElementsByBuiltCategory(commandData, bic, "family_instances_all");
                }
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
                    FilteredElementCollector coll_outline_2 = new FilteredElementCollector(doc, doc.ActiveView.Id);
                    if (!ActiveViewBoolean)
                    {
                        coll_outline_2 = new FilteredElementCollector(doc);
                    }
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

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }

        }
        public static void MultipleFamilyInstanceToMultipleFamilyInstances_BBox(ExternalCommandData commandData) // Family Instance vs Family Instance
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uiapp.ActiveUIDocument.Document;
            var activeView = uidoc.ActiveView;
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

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }
        }


        //Elements vs Links Elements
        public static void MultipleElementsToLinks(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(false);
            List<BuiltInCategory> UI_list3 = GetLists.BuiltCategories(false);

            IList<Element> linkInstances = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)).ToElements();

            List<Element> lista_links = new List<Element>();

            foreach (RevitLinkInstance link in linkInstances)
            {
                lista_links.Add(link);
            }


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

                    ExclusionFilter filter4 = new ExclusionFilter(collectoreID);
                    FilteredElementCollector collector4 = new FilteredElementCollector(link.GetLinkDocument());

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
                }
            }

            foreach (Element elem in clash_yesA)
            {
                Parameter param = elem.LookupParameter("Clash");
                using (Transaction t = new Transaction(doc, "Clash YES"))
                {
                    t.Start();
                    param.Set("YES");
                    t.Commit();
                }
            }

            //SetClashGridLocation.DoAllDocument(commandData);
        }

        public static void MultipleFamilyInstanceToLinks(ExternalCommandData commandData)
        {

        }
        //FamilyIntance vs Links Elements




    }
}
