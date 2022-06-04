using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class RemoveClashFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Autodesk.Revit.DB.View activeView = uidoc.ActiveView;

                ICollection<ElementId> ListFilters = activeView.GetFilters();

                if (ListFilters.Count() != 0)
                {
                    SetRemoveApplyFilter.Do(commandData);
                }
                else
                {
                    SetApplyFilter.Do(commandData);
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                _ = TaskDialog.Show("Error", e.Message);
                LogProgress.UpDate(e.Message);
                return Result.Failed;
            }

        }
    }
}