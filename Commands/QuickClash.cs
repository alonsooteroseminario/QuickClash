using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace QuickClash
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class QuickClash : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //try
            //{
                Intersect2.MultipleElementsToMultipleCategory(commandData);

                Intersect2.MultipleElementsToMultipleFamilyInstances(commandData);

                Intersect2.MultipleFamilyInstanceToMultipleFamilyInstances_BBox(commandData);

                SetClashGridLocation.DoActiveView(commandData);

                SetIDValue.Do(commandData, "ActiveView");

                return Result.Succeeded;
            //}
            //catch (Exception e)
            //{
            //    _ = TaskDialog.Show("Error", e.Message);
            //    LogProgress.UpDate(e.Message);
            //    return Result.Failed;
            //}

        }
    }
}