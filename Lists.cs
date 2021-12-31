using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickClash
{
    public static class Lists
    {
		/// <summary>
		/// Devuelve la lista de valores del diccionario ingresado como parámetro, 'false' para Elementos y 'true' para Family Instance 
		/// </summary>
		/// <param>List of BuiltCategories.</param>
		public static List<BuiltInCategory> BuiltCategories(bool familyInstance)
        {
            if (!familyInstance)
            {
				List<BuiltInCategory> lista = new List<BuiltInCategory>()
				{
						BuiltInCategory.OST_CableTray,
						//BuiltInCategory.OST_CableTrayFitting,
						BuiltInCategory.OST_Conduit,
						//BuiltInCategory.OST_ConduitFitting,
						BuiltInCategory.OST_DuctCurves,
						//BuiltInCategory.OST_DuctFitting,
						//BuiltInCategory.OST_DuctTerminal,
						//BuiltInCategory.OST_ElectricalEquipment,
						//BuiltInCategory.OST_ElectricalFixtures,
						//BuiltInCategory.OST_LightingDevices,
						//BuiltInCategory.OST_LightingFixtures,
						//BuiltInCategory.OST_MechanicalEquipment,
						BuiltInCategory.OST_PipeCurves,
						BuiltInCategory.OST_FlexDuctCurves,
						BuiltInCategory.OST_FlexPipeCurves,
						//BuiltInCategory.OST_PipeFitting,
						//BuiltInCategory.OST_PlumbingFixtures,
						//BuiltInCategory.OST_SpecialityEquipment,
						//BuiltInCategory.OST_Sprinklers,
						//BuiltInCategory.OST_Wire,

				};
				return lista;
			}
			else
            {
				List<BuiltInCategory> bics_familyIns = new List<BuiltInCategory>() { 
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
				    //BuiltInCategory.OST_FlexDuctCurves,
				    //BuiltInCategory.OST_FlexPipeCurves,
				    BuiltInCategory.OST_PipeFitting,
					BuiltInCategory.OST_PlumbingFixtures,
					BuiltInCategory.OST_SpecialityEquipment,
					BuiltInCategory.OST_Sprinklers,
					//BuiltInCategory.OST_Wire,
				};
				return bics_familyIns;
			}
		}
		public static List<string> Params()
        {
			List<string> listParam = new List<string>()
					{
						"Clash",
						"Clash Category",
						"Clash Comments",
						"Clash Grid Location",
						"Clash Solved",
						"Done",
						"ID Element",
						"Percent Done",
						"Zone"
					};
			return listParam;
		}



    }
}
