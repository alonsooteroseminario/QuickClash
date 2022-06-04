#region Namespaces

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

#endregion

namespace QuickClash
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SectionBoxSelection : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                View3D viewer_copy = View.CopySelection(commandData);
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                _ = TaskDialog.Show("Error", e.Message.ToString());
                LogProgress.UpDate(e.Message);
                return Result.Cancelled;
            }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            return application is null ? throw new ArgumentNullException(nameof(application)) : Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return application is null ? throw new ArgumentNullException(nameof(application)) : Result.Succeeded;
        }
    }
}