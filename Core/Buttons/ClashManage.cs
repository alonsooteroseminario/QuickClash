using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using forms = System.Windows.Forms;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class ClashManage : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            List<string> lista1 = new List<string>();
            List<string> lista2 = new List<string>();

            List<string> lista3 = new List<string>();
            List<string> lista4 = new List<string>();

            List<bool> lista_checkBox_1 = new List<bool>();
            List<bool> lista_checkBox_3 = new List<bool>();

            using (var form = new Form2())
            {
                form.ShowDialog();

                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }
                if (form.DialogResult == forms.DialogResult.OK)
                {
                    bool checkBox_1 = form.CheckBox_1;
                    lista_checkBox_1.Add(checkBox_1);

                    bool checkBox_3 = form.CheckBox_3;
                    lista_checkBox_3.Add(checkBox_3);

                    foreach (string s in form.CheckedItems1)
                    {
                        if (s == "CableTray" || s == "Conduit" || s == "Duct" || s == "Pipe" || s == "FlexDuct" || s == "FlexPipe")
                        {
                            lista1.Add(s);
                        }

                    }

                    foreach (string s in form.CheckedItems1)
                    {
                        if (s == "CableTrayFitting" || s == "ConduitFitting" || s == "DuctFitting" || s == "DuctTerminal" || s == "ElectricalEquipment" ||
                            s == "ElectricalFixtures" || s == "LightingDevices" || s == "LightingFixtures" || s == "MechanicalEquipment" || s == "PipeFitting" ||
                            s == "PlumbingFixtures" || s == "SpecialityEquipment" || s == "Sprinklers")
                        {
                            lista2.Add(s);
                        }
                    }

                    foreach (string s in form.CheckedItems2)
                    {
                        if (s == "CableTray" || s == "Conduit" || s == "Duct" || s == "Pipe" || s == "FlexDuct" || s == "FlexPipe")
                        {
                            lista3.Add(s);
                        }

                    }
                    foreach (string s in form.CheckedItems2)
                    {
                        if (s == "CableTrayFitting" || s == "ConduitFitting" || s == "DuctFitting" || s == "DuctTerminal" || s == "ElectricalEquipment" ||
                            s == "ElectricalFixtures" || s == "LightingDevices" || s == "LightingFixtures" || s == "MechanicalEquipment" || s == "PipeFitting" ||
                            s == "PlumbingFixtures" || s == "SpecialityEquipment" || s == "Sprinklers")
                        {
                            lista4.Add(s);
                        }
                    }

                }

            }

            bool checkBox_1s = lista_checkBox_1.First();
            bool checkBox_3s = lista_checkBox_3.First();

            List<BuiltInCategory> UI_list1 = new List<BuiltInCategory>();
            List<BuiltInCategory> UI_list2 = new List<BuiltInCategory>();

            List<BuiltInCategory> UI_list3 = new List<BuiltInCategory>();
            List<BuiltInCategory> UI_list4 = new List<BuiltInCategory>();

            List<BuiltInCategory> bics = GetLists.BuiltCategories(false);

            foreach (BuiltInCategory bic in bics)
            {
                foreach (string s in lista1)
                {
                    string sT = "OST_" + s;
                    if (bic.ToString().Contains(sT))
                    {
                        if (!UI_list1.Contains(bic))
                        {
                            UI_list1.Add(bic);
                        }
                    }
                }
                foreach (string s in lista3)
                {
                    string sT = "OST_" + s;
                    if (bic.ToString().Contains(sT))
                    {
                        if (!UI_list3.Contains(bic))
                        {
                            UI_list3.Add(bic);
                        }
                    }
                }
            }
            List<BuiltInCategory> bics_finst = GetLists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_finst)
            {
                foreach (string s in lista2)
                {
                    string sT = "OST_" + s;
                    if (bic.ToString().Contains(sT))
                    {
                        if (!UI_list2.Contains(bic))
                        {
                            UI_list2.Add(bic);
                        }
                    }
                }
                foreach (string s in lista4)
                {
                    string sT = "OST_" + s;
                    if (bic.ToString().Contains(sT))
                    {
                        if (!UI_list4.Contains(bic))
                        {
                            UI_list4.Add(bic);
                        }
                    }
                }
            }
            if (checkBox_1s && !checkBox_3s)
            {
                Intersect.MultipleElementsToMultipleCategory_UI(commandData, UI_list1, UI_list3, true);
                Intersect.MultipleElementsToMultipleFamilyInstances_UI(commandData, UI_list1, UI_list4, true);
                Intersect.MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI(commandData, UI_list2, UI_list4, true);

            }
            else if (checkBox_3s && !checkBox_1s)
            {
                Intersect.MultipleElementsToMultipleCategory_UI(commandData, UI_list1, UI_list3, false);
                Intersect.MultipleElementsToMultipleFamilyInstances_UI(commandData, UI_list1, UI_list4, false);
                Intersect.MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI(commandData, UI_list2, UI_list4, false);
            }
            return Result.Succeeded;
        }
        public Result OnStartup(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;
        }
    }
}
