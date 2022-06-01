using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class SetIDValue
    {
        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void Do(ExternalCommandData commandData, string activeViewVerified)
        {
            if (activeViewVerified == "ActiveView")
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uidoc = uiApp.ActiveUIDocument;
                Document doc = uiApp.ActiveUIDocument.Document;
                var activeView = uidoc.ActiveView;
                IList<Element> ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
                IList<Element> pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
                IList<Element> conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_Conduit, "conduits");
                IList<Element> cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
                IList<Element> flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
                IList<Element> flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
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
                    SetParameter.Id(commandData, elems);
                }
                List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
                foreach (BuiltInCategory bic in bics_familyIns)
                {
                    ElementClassFilter MEelemFilter = new ElementClassFilter(typeof(FamilyInstance));
                    ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
                    LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(MEelemFilter, MECategoryfilter);
                    FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
                    IList<Element> familyInstance = MEcoll.WherePasses(MEInstancesFilter).ToElements();
                    SetParameter.Id(commandData, familyInstance);
                }
            }
            else if (activeViewVerified == "AllProject")
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

                SetParameter.Id(commandData, ducts);
                SetParameter.Id(commandData, pipes);
                SetParameter.Id(commandData, conduits);
                SetParameter.Id(commandData, cabletrays);
                SetParameter.Id(commandData, flexducts);
                SetParameter.Id(commandData, flexpipes);
                SetParameter.Id(commandData, mechanicalequipment);
                SetParameter.Id(commandData, ducts_fittings);
                SetParameter.Id(commandData, conduit_fittings);
                SetParameter.Id(commandData, pipe_fittings);
                SetParameter.Id(commandData, air_terminal);
                SetParameter.Id(commandData, cabletray_fittings);
                SetParameter.Id(commandData, electrical_equipment);
                SetParameter.Id(commandData, electrical_fixtures);
                SetParameter.Id(commandData, ligthting_devices);
                SetParameter.Id(commandData, ligthting_fixtures);
                SetParameter.Id(commandData, plumbing_fixtures);
                SetParameter.Id(commandData, sprinklers);
            }
        }
    }
}