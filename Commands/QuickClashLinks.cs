using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuickClash.Views;
using System.Windows;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class QuickClashLinks : IExternalCommand
    {
        public static Window RevitCommandWindow { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Iniciar Window primero
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            RevitCommandWindow = new Window();

            var viewmodel = new ManageWindow(uidoc, RevitCommandWindow);
            RevitCommandWindow.Content = viewmodel;
            RevitCommandWindow.ShowDialog();



            Intersect.MultipleElementsToLinksElements(commandData);

            Intersect.MultipleElementsToLinksFamilyInstance(commandData);

            Intersect.MultipleFamilyInstanceToLinksElements(commandData);

            Intersect.MultipleFamilyInstanceToLinksFamilyInstance(commandData);

            SetClashGridLocation.DoActiveView(commandData);

            return Result.Succeeded;
        }
    }
}