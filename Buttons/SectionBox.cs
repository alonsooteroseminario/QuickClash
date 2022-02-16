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
    class SectionBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string view_name = "COORD";
            List<bool> listabool_3 = new List<bool>();
            List<bool> listabool_2 = new List<bool>();
            List<bool> listabool_1 = new List<bool>();
            List<bool> listabool_4 = new List<bool>();
            using (var form = new Form3())
            {
                form.ShowDialog();
                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }
                if (form.DialogResult == forms.DialogResult.OK)
                {
                    bool ee4 = form.checkBox_4;
                    listabool_4.Add(ee4);
                    bool ee3 = form.checkBox_3;
                    listabool_3.Add(ee3);
                    bool ee2 = form.checkBox_2;
                    listabool_2.Add(ee2);
                    bool ee1 = form.checkBox_1;
                    listabool_1.Add(ee1);
                }
                if (listabool_3.First() && !(listabool_2.First()) && !(listabool_1.First()) && !(listabool_4.First()))
                {
                    ClashSectionBoxView.Do(commandData, view_name);
                }
                else if (listabool_1.First() && !(listabool_2.First()) && !(listabool_3.First()) && !(listabool_4.First()))
                {
                    ClashSectionBoxView_ELEMENT.Do(commandData);
                }
                else if (listabool_2.First() && !(listabool_1.First()) && !(listabool_3.First()) && !(listabool_4.First()))
                {
                    ClashSectionBoxView_LEVELS.Do(commandData);
                }
                else if (listabool_4.First() && !(listabool_1.First()) && !(listabool_3.First()) && !(listabool_2.First()))
                {
                    ClashSectionBoxView_ZONE.Do(commandData);
                }
                else
                {
                    TaskDialog.Show("FINAL", "Selecciona SOLAMENTE 1 CHECK a la vez para que funcione correctamente por favor! :)");
                }
            }
            return Result.Succeeded;
        }
    }
}
