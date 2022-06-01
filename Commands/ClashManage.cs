using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickClash.Views;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class ClashManage : IExternalCommand
    {
        public static Window RevitCommandWindow { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            RevitCommandWindow = new Window();

            var viewmodel = new ManageWindow(uidoc, RevitCommandWindow);
            RevitCommandWindow.Content = viewmodel;
            RevitCommandWindow.ShowDialog();

            return Result.Succeeded;
        }
    }
}
