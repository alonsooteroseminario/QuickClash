using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class QuickClash : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            Intersect.MultipleElementsToMultipleCategory(commandData);

            Intersect.MultipleElementsToMultipleFamilyInstances(commandData);

            Intersect.MultipleFamilyInstanceToMultipleFamilyInstances_BBox(commandData);

            SetClashGridLocation.DoActiveView(commandData);

            SetIDValue.Do(commandData, "ActiveView");

            return Result.Succeeded;
        }

    }
}
