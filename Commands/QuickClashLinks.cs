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

            RevitCommandWindow = new Window()
            {
                Width = 825,
                Height = 500,
            };

            var viewmodel = new ManageWindow(commandData, uidoc, RevitCommandWindow);
            RevitCommandWindow.Content = viewmodel;
            RevitCommandWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}