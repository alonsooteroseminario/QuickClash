using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class SetIDValue_ActiveView
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
            IList<Element> ducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
            foreach (Element e in ducts)
            {
                Parameter param = e.LookupParameter("ID Element");

                Autodesk.Revit.DB.ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    t.Start();
                    param.Set(idString);
                    t.Commit();
                }
            }
            foreach (Element e in pipes)
            {
                Parameter param = e.LookupParameter("ID Element");

                Autodesk.Revit.DB.ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    t.Start();
                    param.Set(idString);
                    t.Commit();
                }
            }
            foreach (Element e in conduits)
            {
                Parameter param = e.LookupParameter("ID Element");

                Autodesk.Revit.DB.ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    t.Start();
                    param.Set(idString);
                    t.Commit();
                }
            }
            foreach (Element e in cabletrays)
            {
                Parameter param = e.LookupParameter("ID Element");

                Autodesk.Revit.DB.ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    t.Start();
                    param.Set(idString);
                    t.Commit();
                }
            }
            foreach (Element e in flexducts)
            {
                Parameter param = e.LookupParameter("ID Element");

                Autodesk.Revit.DB.ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    t.Start();
                    param.Set(idString);
                    t.Commit();
                }
            }
            foreach (Element e in flexpipes)
            {
                Parameter param = e.LookupParameter("ID Element");

                Autodesk.Revit.DB.ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    t.Start();
                    param.Set(idString);
                    t.Commit();
                }
            }
            // FAMILY INSTANCES
            List<BuiltInCategory> bics_familyIns = Lists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter MEelemFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(MEelemFilter, MECategoryfilter);
                FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
                IList<Element> familyInstance = MEcoll.WherePasses(MEInstancesFilter).ToElements();
                foreach (Element e in familyInstance)
                {
                    Parameter param = e.LookupParameter("ID Element");

                    Autodesk.Revit.DB.ElementId selectedId = e.Id;
                    string idString = selectedId.IntegerValue.ToString();

                    using (Transaction t = new Transaction(doc, "ID Element"))
                    {
                        t.Start();
                        param.Set(idString);
                        t.Commit();
                    }
                }
            }
        }
    }
}
