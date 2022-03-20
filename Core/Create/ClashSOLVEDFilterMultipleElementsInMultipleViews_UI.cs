using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class ClashSOLVEDFilterMultipleElementsInMultipleViews_UI
    {

        public static void Do(ExternalCommandData commandData, List<View3D> lista_3dviews)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
            Application app = uiApp.Application;
            var activeView = uidoc.ActiveView;
            FilteredElementCollector elementss = new FilteredElementCollector(doc);
            FillPatternElement solidFillPattern = elementss.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);
            List<ElementId> cats = new List<ElementId>();
            cats.Add(new ElementId(BuiltInCategory.OST_DuctCurves));
            cats.Add(new ElementId(BuiltInCategory.OST_PipeCurves));
            cats.Add(new ElementId(BuiltInCategory.OST_Conduit));
            cats.Add(new ElementId(BuiltInCategory.OST_CableTray));
            cats.Add(new ElementId(BuiltInCategory.OST_FlexDuctCurves));
            cats.Add(new ElementId(BuiltInCategory.OST_FlexPipeCurves));
            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                cats.Add(new ElementId(bic));
            }
            foreach (View3D view3d in lista_3dviews)
            {
                activeView = view3d;
                using (Transaction ta = new Transaction(doc, "create clash solved filter view"))
                {
                    ta.Start();
                    List<ParameterFilterElement> lista_ParameterFilterElement = new List<ParameterFilterElement>();
                    List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();
                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");
                    FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));
                    List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();
                    foreach (ParameterFilterElement e in collector_filterview)
                    {
                        lista_filtros.Add(e);
                    }
                    List<ElementFilter> elementFilterList = new List<ElementFilter>();
                    elementFilterList.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id, (int)1))); // Clash Solved , EQUAL,  False(int=0),
                    for (int i = 0; i < lista_filtros.Count(); i++)
                    {
                        if (lista_filtros[i].Name == "CLASH SOLVED FILTER")
                        {
                            lista_ParameterFilterElement.Add(lista_filtros[i]);
                            i = lista_filtros.Count();
                            break;
                        }
                    }
                    if (lista_ParameterFilterElement.Count() == 0)
                    {
                        ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH SOLVED FILTER", cats, elementFilterList.First());
                        lista_ParameterFilterElement.Add(parameterFilterElement);
                    }
                    ParameterFilterElement aa = lista_ParameterFilterElement.First();
                    OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
                    ogs3.SetProjectionLineColor(new Color(192, 192, 192));
                    ogs3.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
                    ogs3.SetSurfaceForegroundPatternVisible(true);
                    ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);
                    activeView.AddFilter(aa.Id);
                    activeView.SetFilterOverrides(aa.Id, ogs3);
                    ta.Commit();
                }
            }
        }
    }
}
