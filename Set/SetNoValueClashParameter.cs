using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class SetNoValueClashParameter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param></param>
        public static void Do(ExternalCommandData commandData)
        {

            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
            var activeView = uidoc.ActiveView;

            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");

            IList<Element> clash = new List<Element>();
            IList<Element> clash_no = new List<Element>();

            List<IList<Element>> elements = new List<IList<Element>>();
            elements.Add(ducts);
            elements.Add(pipes);
            elements.Add(conduits);
            elements.Add(cabletrays);
            elements.Add(flexducts);
            elements.Add(flexpipes);
            foreach (IList<Element> elems in elements)
            {
                foreach (var item in elems)
                {
                    if (item.LookupParameter("Clash").AsString() == "YES")
                    {
                        clash.Add(item);
                    }
                    else
                    {
                        clash_no.Add(item);
                    }
                }
            }

            foreach (Element e in clash)
            {
                Parameter clashParam = e.LookupParameter("Clash");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    clashParam.Set("NO");
                    param4.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }
            foreach (Element e in clash_no)
            {
                Parameter param3 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param3.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }

            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);

            IList<Element> familyInstance_clash = new List<Element>();
            IList<Element> familyInstance_clash_no = new List<Element>();

            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter DUFelemFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter DUFCategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter DUFInstancesFilter = new LogicalAndFilter(DUFelemFilter, DUFCategoryfilter);
                FilteredElementCollector DUFcoll = new FilteredElementCollector(doc, activeView.Id);
                IList<Element> family_instance = DUFcoll.WherePasses(DUFInstancesFilter).ToElements();

                foreach (Element elem in family_instance)
                {
                    if (elem.LookupParameter("Clash").AsString() == "YES")
                    {
                        familyInstance_clash.Add(elem);
                    }
                    else
                    {
                        familyInstance_clash_no.Add(elem);
                    }
                }

            }

            foreach (Element e in familyInstance_clash)
            {
                Parameter clashParam = e.LookupParameter("Clash");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    clashParam.Set("NO");
                    param4.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }

            foreach (Element e in familyInstance_clash_no)
            {
                Parameter param3 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param3.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }
        }
    }
}
