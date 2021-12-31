using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickClash
{
    public static class SetIDValue
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

			IList<Element> mechanicalequipment = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_MechanicalEquipment, "family_instances_all");
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
