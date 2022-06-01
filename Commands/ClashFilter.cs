using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class ClashFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            var activeView = uidoc.ActiveView;

            ICollection<ElementId> ListFilters = activeView.GetFilters();

            if (ListFilters.Count() == 0)
            {
                SetApplyFilter.Do(commandData);
            }
            else
            {
                SetRemoveApplyFilter.Do(commandData);
            }

            return Result.Succeeded;
        }
    }
}