using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class QuickClash : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            List<Element> clash_yes = new List<Element>();

            SetNoValueClashParameter.Do(commandData); // Vista Activa , "Clash" y "Clash Grid Location" = " " vacio.

            Intersect.MultipleElementsToMultipleCategory(commandData);

            Intersect.MultipleElementsToMultipleFamilyInstances(commandData);// Vista Activa

            Intersect.MultipleFamilyInstanceToMultipleFamilyInstances_BBox(commandData);

            SetClashGridLocation.DoAllDocument(commandData);

            SetIDValue.Do(commandData, "ActiveView");

            List<Element> iclash_yes = GetAllNOClashElements_OnlyActiveView.Do(commandData);// Vista Activa

            foreach (Element e in iclash_yes)
            {
                clash_yes.Add(e);
            }

            CheckClashSolved.Do(commandData, clash_yes);// Vista Activa

            return Result.Succeeded;
        }

    }
}
