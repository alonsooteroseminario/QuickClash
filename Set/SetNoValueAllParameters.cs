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

            IList<Element> mechanicalequipment = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_MechanicalEquipment, "mechanicalequipment");
            IList<Element> ducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");


            // get elements with "clash" parameter value == "YES"
            IList<Element> ductsclash = new List<Element>();
            // get elements with "clash" parameter value == "NO"
            IList<Element> ductsclash_no = new List<Element>();

            foreach (Element elem in ducts)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    ductsclash.Add(elem);
                }
                else
                {
                    ductsclash_no.Add(elem);
                }
            }
            foreach (Element elem in pipes)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    ductsclash.Add(elem);
                }
                else
                {
                    ductsclash_no.Add(elem);
                }
            }
            foreach (Element elem in conduits)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    ductsclash.Add(elem);
                }
                else
                {
                    ductsclash_no.Add(elem);
                }
            }
            foreach (Element elem in cabletrays)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    ductsclash.Add(elem);
                }
                else
                {
                    ductsclash_no.Add(elem);
                }
            }
            foreach (Element elem in flexducts)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    ductsclash.Add(elem);
                }
                else
                {
                    ductsclash_no.Add(elem);
                }
            }
            foreach (Element elem in flexpipes)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    ductsclash.Add(elem);
                }
                else
                {
                    ductsclash_no.Add(elem);
                }
            }

            foreach (Element e in ductsclash)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    param2.Set(0);
                    param3.Set(param_value);
                    param4.Set(param_value);
                    t.Commit();
                }
            }

            foreach (Element e in ductsclash_no)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    param2.Set(0);
                    param3.Set(param_value);
                    param4.Set(param_value);
                    t.Commit();
                }
            }

            // FAMILY INSTANCES

            BuiltInCategory[] bics_familyIns = new BuiltInCategory[]
                {
					    //BuiltInCategory.OST_CableTray,
					    BuiltInCategory.OST_CableTrayFitting,
					    //BuiltInCategory.OST_Conduit,
					    BuiltInCategory.OST_ConduitFitting,
					    //BuiltInCategory.OST_DuctCurves,
					    BuiltInCategory.OST_DuctFitting,
                        BuiltInCategory.OST_DuctTerminal,
                        BuiltInCategory.OST_ElectricalEquipment,
                        BuiltInCategory.OST_ElectricalFixtures,
                        BuiltInCategory.OST_LightingDevices,
                        BuiltInCategory.OST_LightingFixtures,
                        BuiltInCategory.OST_MechanicalEquipment,
					    //BuiltInCategory.OST_PipeCurves,
					    BuiltInCategory.OST_FlexDuctCurves,
                        BuiltInCategory.OST_FlexPipeCurves,
                        BuiltInCategory.OST_PipeFitting,
                        BuiltInCategory.OST_PlumbingFixtures,
                        BuiltInCategory.OST_SpecialityEquipment,
                        BuiltInCategory.OST_Sprinklers,
					//BuiltInCategory.OST_Wire,
				};

            // get elements with "clash" parameter value == "YES"
            IList<Element> ductfittingsclash = new List<Element>();
            // get elements with "clash" parameter value == "NO"
            IList<Element> ductfittingsclash_no = new List<Element>();

            foreach (BuiltInCategory bic in bics_familyIns)
            {
                // category ducts fittings
                ElementClassFilter DUFelemFilter = new ElementClassFilter(typeof(FamilyInstance));
                // Create a category filter for Ducts
                ElementCategoryFilter DUFCategoryfilter = new ElementCategoryFilter(bic);
                // Create a logic And filter for all MechanicalEquipment Family
                LogicalAndFilter DUFInstancesFilter = new LogicalAndFilter(DUFelemFilter, DUFCategoryfilter);
                // Apply the filter to the elements in the active document
                FilteredElementCollector DUFcoll = new FilteredElementCollector(doc);
                IList<Element> ductfittings = DUFcoll.WherePasses(DUFInstancesFilter).ToElements();

                foreach (Element elem in ductfittings)
                {
                    if (elem.LookupParameter("Clash").AsString() == "YES")
                    {
                        ductfittingsclash.Add(elem);
                    }
                    else
                    {
                        ductfittingsclash_no.Add(elem);
                    }
                }

            }

            foreach (Element e in ductfittingsclash)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    param2.Set(0);
                    param3.Set(param_value);
                    param4.Set(param_value);
                    t.Commit();
                }
            }

            foreach (Element e in ductfittingsclash_no)
            {
                Parameter param = e.LookupParameter("Clash");
                Parameter param2 = e.LookupParameter("Clash Solved");
                Parameter param3 = e.LookupParameter("Clash Comments");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                string param_value = "";

                using (Transaction t = new Transaction(doc, "Set No value to Clash elements in Active View"))
                {
                    t.Start();
                    param.Set(param_value);
                    param2.Set(0);
                    param3.Set(param_value);
                    param4.Set(param_value);
                    t.Commit();
                }
            }
        }
    }
}
