using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using forms = System.Windows.Forms;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class ClashManage : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
			List<string> lista1 = new List<string>(); // seleccion "s" Element
			List<string> lista2 = new List<string>(); // seleccion "s" Family Instance

			List<string> lista3 = new List<string>(); // seleccion "s" Element
			List<string> lista4 = new List<string>(); // seleccion "s" Family Instance

			List<bool> lista_checkBox_1 = new List<bool>();
			List<bool> lista_checkBox_3 = new List<bool>();

			using (var form = new Form2())
			{
				form.ShowDialog();

				if (form.DialogResult == forms.DialogResult.Cancel) // Boton Cancel
				{
					return Result.Cancelled;
				}

				if (form.DialogResult == forms.DialogResult.OK) // OK
				{
					bool checkBox_1 = form.checkBox_1; // solo vista activa
					lista_checkBox_1.Add(checkBox_1);

					bool checkBox_3 = form.checkBox_3; // todo documento
					lista_checkBox_3.Add(checkBox_3);

					foreach (string s in form.checkedItems1)
					{
						//TaskDialog.Show("Selected", s);
						if (s == "CableTray" || s == "Conduit" || s == "Duct" || s == "Pipe" || s == "FlexDuct" || s == "FlexPipe")
						{
							lista1.Add(s); // Elements del grupo 1
						}

					}

					foreach (string s in form.checkedItems1)
					{
						//TaskDialog.Show("Selected", s);
						if (s == "CableTrayFitting" || s == "ConduitFitting" || s == "DuctFitting" || s == "DuctTerminal" || s == "ElectricalEquipment" ||
							s == "ElectricalFixtures" || s == "LightingDevices" || s == "LightingFixtures" || s == "MechanicalEquipment" || s == "PipeFitting" ||
							s == "PlumbingFixtures" || s == "SpecialityEquipment" || s == "Sprinklers")
						{
							lista2.Add(s); // Family Instance dele grupo 1
						}
					}

					foreach (string s in form.checkedItems2)
					{
						//TaskDialog.Show("Selected", s);
						if (s == "CableTray" || s == "Conduit" || s == "Duct" || s == "Pipe" || s == "FlexDuct" || s == "FlexPipe")
						{
							lista3.Add(s); // Elements del grupo 2
						}

					}
					foreach (string s in form.checkedItems2)
					{
						//TaskDialog.Show("Selected", s);
						if (s == "CableTrayFitting" || s == "ConduitFitting" || s == "DuctFitting" || s == "DuctTerminal" || s == "ElectricalEquipment" ||
							s == "ElectricalFixtures" || s == "LightingDevices" || s == "LightingFixtures" || s == "MechanicalEquipment" || s == "PipeFitting" ||
							s == "PlumbingFixtures" || s == "SpecialityEquipment" || s == "Sprinklers")
						{
							lista4.Add(s); // Family Instance dele grupo 2
						}
					}

				}

			}

			bool checkBox_1s = lista_checkBox_1.First();
			bool checkBox_3s = lista_checkBox_3.First();

			List<BuiltInCategory> UI_list1 = new List<BuiltInCategory>(); // Element	
			List<BuiltInCategory> UI_list2 = new List<BuiltInCategory>(); // Family Instance

			List<BuiltInCategory> UI_list3 = new List<BuiltInCategory>(); // Element
			List<BuiltInCategory> UI_list4 = new List<BuiltInCategory>(); // Family Instance

			// Elements
			List<BuiltInCategory> bics = Lists.BuiltCategories(false);

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

			// Family Instance
			List<BuiltInCategory> bics_finst = Lists.BuiltCategories(true);

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

			string a1 = "Numero total UI_list1 = " + UI_list1.Count().ToString() + Environment.NewLine;
			string a2 = "Numero total UI_list2 = " + UI_list2.Count().ToString() + Environment.NewLine;

			string a3 = "Numero total UI_list3 = " + UI_list3.Count().ToString() + Environment.NewLine;
			string a4 = "Numero total UI_list4 = " + UI_list4.Count().ToString() + Environment.NewLine;

			foreach (BuiltInCategory e in UI_list1)
			{
				a1 = a1 + e.ToString() + " : " + Environment.NewLine;
			}
			foreach (BuiltInCategory e in UI_list2)
			{
				a2 = a2 + e.ToString() + " : " + Environment.NewLine;
			}
			foreach (BuiltInCategory e in UI_list3)
			{
				a3 = a3 + e.ToString() + " : " + Environment.NewLine;
			}
			foreach (BuiltInCategory e in UI_list4)
			{
				a4 = a4 + e.ToString() + " : " + Environment.NewLine;
			}

			if (checkBox_1s && !checkBox_3s) // TRUE Solo Vista Activa
			{
				Intersect.MultipleElementsToMultipleCategory_UI(commandData, UI_list1, UI_list3);
				Intersect.MultipleElementsToMultipleFamilyInstances_UI(commandData, UI_list1, UI_list4);
				Intersect.MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI(commandData, UI_list2, UI_list4);

			}
			else if (checkBox_3s && !checkBox_1s) // TRUE Todo documento
			{
				Intersect.MultipleElementsToMultipleCategory_UI_doc(commandData, UI_list1, UI_list3);
				Intersect.MultipleElementsToMultipleFamilyInstances_UI_doc(commandData, UI_list1, UI_list4);
				Intersect.MultipleFamilyInstanceToMultipleFamilyInstances_BBox_UI_doc(commandData, UI_list2, UI_list4);
			}
			else
			{

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
