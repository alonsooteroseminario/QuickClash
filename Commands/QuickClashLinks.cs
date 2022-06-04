using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuickClash.Views;
using System;
using System.Windows;

namespace QuickClash
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class QuickClashLinks : IExternalCommand
    {
        public static Window RevitCommandWindow { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                _ = uidoc.Document;

                RevitCommandWindow = new Window()
                {
                    Width = 825,
                    Height = 500,
                };

                ManageWindow viewmodel = new ManageWindow(commandData, uidoc, RevitCommandWindow);
                RevitCommandWindow.Content = viewmodel;
                _ = RevitCommandWindow.ShowDialog();

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