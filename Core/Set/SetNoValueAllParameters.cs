using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class SetNoValueAllParameters
    {
        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void Do(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");

            IList<Element> clash = new List<Element>();
            IList<Element> clash_no = new List<Element>();

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
                foreach (Element item in elems)
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
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    _ = t.Start();
                    _ = param.Set(param_value);
                    _ = param2.Set(0);
                    _ = param3.Set(param_value);
                    _ = param4.Set(param_value);
                    _ = t.Commit();
                }
            }

            foreach (Element e in clash_no)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    _ = t.Start();
                    _ = param.Set(param_value);
                    _ = param2.Set(0);
                    _ = param3.Set(param_value);
                    _ = param4.Set(param_value);
                    _ = t.Commit();
                }
            }

            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);

            IList<Element> familyInstanceclash = new List<Element>();
            IList<Element> familyInstanceclash_no = new List<Element>();

            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter DUFelemFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter DUFCategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter DUFInstancesFilter = new LogicalAndFilter(DUFelemFilter, DUFCategoryfilter);
                FilteredElementCollector DUFcoll = new FilteredElementCollector(doc);
                IList<Element> familyInstance = DUFcoll.WherePasses(DUFInstancesFilter).ToElements();

                foreach (Element elem in familyInstance)
                {
                    if (elem.LookupParameter("Clash").AsString() == "YES")
                    {
                        familyInstanceclash.Add(elem);
                    }
                    else
                    {
                        familyInstanceclash_no.Add(elem);
                    }
                }
            }

            foreach (Element e in familyInstanceclash)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    _ = t.Start();
                    _ = param.Set(param_value);
                    _ = param2.Set(0);
                    _ = param3.Set(param_value);
                    _ = param4.Set(param_value);
                    _ = t.Commit();
                }
            }

            foreach (Element e in familyInstanceclash_no)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    _ = t.Start();
                    _ = param.Set(param_value);
                    _ = param2.Set(0);
                    _ = param3.Set(param_value);
                    _ = param4.Set(param_value);
                    _ = t.Commit();
                }
            }
        }
    }
}