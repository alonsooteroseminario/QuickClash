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

			SetParameter.Id(commandData, ducts);
			SetParameter.Id(commandData, pipes);
			SetParameter.Id(commandData, conduits);
			SetParameter.Id(commandData, cabletrays);
			SetParameter.Id(commandData, flexducts);
			SetParameter.Id(commandData, flexpipes);

			IList<Element> mechanicalequipment = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_MechanicalEquipment, "mechanicalequipment");
			SetParameter.Id(commandData, mechanicalequipment);

		}
	}
}
