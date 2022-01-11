using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class GetAllClashElements_OnlyActiveView
    {
        public static List<Element> Do(ExternalCommandData commandData)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
            var activeView = uidoc.ActiveView;
            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            List<Element> clash = new List<Element>();
            List<Element> clash_no = new List<Element>();
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
                FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
                IList<Element> familyInstance = MEcoll.WherePasses(MEInstancesFilter).ToElements();
                foreach (Element elem in familyInstance)
                {
                    if (elem.LookupParameter("Clash").AsString() == "YES")
                    {
                        clash.Add(elem);
                    }
                    else
                    {
                        clash_no.Add(elem);
                    }
                }

            }
            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
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
                foreach (var elem in elems)
                {
                    if (elem.LookupParameter("Clash").AsString() == "YES")
                    {
                        clash.Add(elem);
                    }
                    else
                    {
                        clash_no.Add(elem);
                    }
                }
            }
            return clash;
        }
    }
}
