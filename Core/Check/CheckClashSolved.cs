using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class CheckClashSolved
    {
        public static void Do(ExternalCommandData commandData, List<Element> clash_no_)
        {

            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
            var activeView = uidoc.ActiveView;

            List<Element> clashsolved_yes = new List<Element>();
            List<Element> clashsolved_no = new List<Element>();
            List<Element> clash_yes = new List<Element>();
            List<Element> clash_no = clash_no_;

            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
                FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
                IList<Element> familyInstance = MEcoll.WherePasses(MEInstancesFilter).ToElements();
                foreach (Element e in familyInstance)
                {
                    Parameter param = e.LookupParameter("Clash Solved");
                    Parameter param2 = e.LookupParameter("Clash");
                    using (Transaction t = new Transaction(doc, "parametersME"))
                    {
                        t.Start();
                        if (param.AsInteger() == 1 && param2.AsString() == "YES")
                        {
                            param2.Set("");
                            clashsolved_yes.Add(e);
                        }
                        else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
                        {
                            clashsolved_yes.Add(e);
                        }
                        else
                        {
                            clashsolved_no.Add(e);
                        }
                        if (param2.AsString() == "YES")
                        {
                            clash_yes.Add(e);
                        }
                        t.Commit();
                    }
                }
            }

            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
            List<IList<Element>> list_elements = new List<IList<Element>>
            {
                ducts,
                pipes,
                conduits,
                cabletrays,
                flexducts,
                flexpipes
            };
            foreach (IList<Element> elems in list_elements)
            {
                foreach (Element e in elems)
                {
                    Parameter param = e.LookupParameter("Clash Solved");
                    Parameter param2 = e.LookupParameter("Clash");

                    using (Transaction t = new Transaction(doc, "parameters elems"))
                    {
                        t.Start();
                        if (param.AsInteger() == 1 && param2.AsString() == "YES")
                        {
                            param2.Set("");
                            clashsolved_yes.Add(e);
                        }
                        else if (param.AsInteger() == 1 && !(param2.AsString() == "YES"))
                        {
                            clashsolved_yes.Add(e);
                        }
                        else
                        {
                            clashsolved_no.Add(e);
                        }
                        if (param2.AsString() == "YES")
                        {
                            clash_yes.Add(e);
                        }
                        t.Commit();
                    }
                }
            }

            foreach (Element elem in clash_no)
            {
                Parameter param2 = elem.LookupParameter("Clash");
                string vacio = "";
                using (Transaction t = new Transaction(doc, "Set CLASH = vacio "))
                {

                    t.Start();

                    param2.Set(vacio);

                    t.Commit();
                }
            }

            if (clash_yes.Count() < 1)
            {
                TaskDialog.Show("Dynoscript", "NO HAY INTERFERENCIAS!! en esta vista activa \n\n Muy bien! :)");
                using (Transaction t = new Transaction(doc, "Cambiar nombre Comentado"))
                {
                    t.Start();
                    activeView.Name = activeView.Name.ToString() + " - RESUELTO";
                    t.Commit();
                }

            }
        }
    }
}
