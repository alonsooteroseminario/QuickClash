using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class SetEmptyYesNoParameters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param></param>
        public static void Do(ExternalCommandData commandData)
        {


            IList<Element> ducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");


            IList<Element> mechanicalequipment = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_MechanicalEquipment, "mechanicalequipment");
            IList<Element> ducts_fittings = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctFitting, "family_instances_all");
            IList<Element> conduit_fittings = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_ConduitFitting, "family_instances_all");
            IList<Element> pipe_fittings = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeFitting, "family_instances_all");
            IList<Element> air_terminal = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctTerminal, "family_instances_all");
            IList<Element> cabletray_fittings = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTrayFitting, "family_instances_all");
            IList<Element> electrical_equipment = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_ElectricalEquipment, "family_instances_all");
            IList<Element> electrical_fixtures = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_ElectricalFixtures, "family_instances_all");
            IList<Element> ligthting_devices = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_LightingDevices, "family_instances_all");
            IList<Element> ligthting_fixtures = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_LightingFixtures, "family_instances_all");
            IList<Element> plumbing_fixtures = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PlumbingFixtures, "family_instances_all");
            IList<Element> sprinklers = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Sprinklers, "family_instances_all");


            SetParameter.Do(commandData, ducts);
            SetParameter.Do(commandData, pipes);
            SetParameter.Do(commandData, conduits);
            SetParameter.Do(commandData, cabletrays);
            SetParameter.Do(commandData, flexducts);
            SetParameter.Do(commandData, flexpipes);
            SetParameter.Do(commandData, mechanicalequipment);
            SetParameter.Do(commandData, ducts_fittings);
            SetParameter.Do(commandData, conduit_fittings);
            SetParameter.Do(commandData, pipe_fittings);
            SetParameter.Do(commandData, air_terminal);
            SetParameter.Do(commandData, cabletray_fittings);
            SetParameter.Do(commandData, electrical_equipment);
            SetParameter.Do(commandData, electrical_fixtures);
            SetParameter.Do(commandData, ligthting_devices);
            SetParameter.Do(commandData, ligthting_fixtures);
            SetParameter.Do(commandData, plumbing_fixtures);
            SetParameter.Do(commandData, sprinklers);

        }
    }
}
