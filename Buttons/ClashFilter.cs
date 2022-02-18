using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class ClashFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            var activeView = uidoc.ActiveView;

            FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));
            List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();
            foreach (ParameterFilterElement e in collector_filterview)
            {
                lista_filtros.Add(e);
            }
            for (int i = 0; i < lista_filtros.Count(); i++)
            {
                if (!lista_filtros[i].Name.Contains("CLASH YES FILTER"))
                {
                    ApplyFilter.Do(commandData);
                    break;
                }
            }

            return Result.Succeeded;
        }

    }
}
