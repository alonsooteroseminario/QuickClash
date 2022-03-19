using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class QuickClashLinks : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Intersect.MultipleElementsToLinksElements(commandData);

            Intersect.MultipleFamilyInstanceToLinksElements(commandData);

            Intersect.MultipleElementsToLinksFamilyInstance(commandData);

            Intersect.MultipleFamilyInstanceToLinksFamilyInstance(commandData);

            SetClashGridLocation.DoActiveView(commandData);

            SetIDValue.Do(commandData, "ActiveView");

            return Result.Succeeded;
        }

    }
}
