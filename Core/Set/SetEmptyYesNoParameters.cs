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
            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");

            IList<Element> mechanicalequipment = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_MechanicalEquipment, "family_instances_all");
            IList<Element> ducts_fittings = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctFitting, "family_instances_all");
            IList<Element> conduit_fittings = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_ConduitFitting, "family_instances_all");
            IList<Element> pipe_fittings = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeFitting, "family_instances_all");
            IList<Element> air_terminal = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctTerminal, "family_instances_all");
            IList<Element> cabletray_fittings = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTrayFitting, "family_instances_all");
            IList<Element> electrical_equipment = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_ElectricalEquipment, "family_instances_all");
            IList<Element> electrical_fixtures = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_ElectricalFixtures, "family_instances_all");
            IList<Element> ligthting_devices = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_LightingDevices, "family_instances_all");
            IList<Element> ligthting_fixtures = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_LightingFixtures, "family_instances_all");
            IList<Element> plumbing_fixtures = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PlumbingFixtures, "family_instances_all");
            IList<Element> sprinklers = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Sprinklers, "family_instances_all");

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