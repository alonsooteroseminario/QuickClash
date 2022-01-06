﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class CleanClash : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
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
            IList<Element> clash = new List<Element>();
            IList<Element> clash_no = new List<Element>();
            foreach (Element elem in ducts)
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
            foreach (Element elem in pipes)
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
            foreach (Element elem in conduits)
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
            foreach (Element elem in cabletrays)
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
            foreach (Element elem in flexducts)
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
            foreach (Element elem in flexpipes)
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
            foreach (Element e in clash)
            {
                Parameter param = e.LookupParameter("Clash");
                //Parameter param2 = e.LookupParameter("Clash Solved");
                //Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    //param2.Set(1);
                    //param3.Set(param_value);
                    param4.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }
            foreach (Element e in clash_no)
            {
                Parameter param = e.LookupParameter("Clash");
                //Parameter param2 = e.LookupParameter("Clash Comments");
                Parameter param3 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");


                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    //param2.Set(param_value);
                    param3.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }
            // FAMILY INSTANCES
            List<BuiltInCategory> bics_familyIns = Lists.BuiltCategories(true);
            IList<Element> familyInstance_clash = new List<Element>();
            IList<Element> familyInstance_clash_no = new List<Element>();
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter DUFelemFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter DUFCategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter DUFInstancesFilter = new LogicalAndFilter(DUFelemFilter, DUFCategoryfilter);
                FilteredElementCollector DUFcoll = new FilteredElementCollector(doc, activeView.Id);
                IList<Element> ductfittings = DUFcoll.WherePasses(DUFInstancesFilter).ToElements();

                foreach (Element elem in ductfittings)
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
                Parameter param = e.LookupParameter("Clash");
                //Parameter param2 = e.LookupParameter("Clash Solved");
                //Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    //param2.Set(1);
                    //param3.Set(param_value);
                    param4.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }
            foreach (Element e in familyInstance_clash_no)
            {
                Parameter param = e.LookupParameter("Clash");
                //Parameter param2 = e.LookupParameter("Clash Comments");
                Parameter param3 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    //param2.Set(param_value);
                    param3.Set(param_value);
                    param5.Set(param_value);
                    t.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
