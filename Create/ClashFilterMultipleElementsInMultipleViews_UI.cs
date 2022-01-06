using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class ClashFilterMultipleElementsInMultipleViews_UI
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
            List<BuiltInCategory> bics_familyIns = Lists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                cats.Add(new ElementId(bic));
            }
            foreach (View3D view3d in lista_3dviews)
            {
                activeView = view3d;
                List<ParameterFilterElement> lista_ParameterFilterElement1 = new List<ParameterFilterElement>();
                List<ParameterFilterElement> lista_ParameterFilterElement2 = new List<ParameterFilterElement>();
                List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();
                using (Transaction ta = new Transaction(doc, "create clash filter view"))
                {
                    ta.Start();
                    FilteredElementCollector collector = new FilteredElementCollector(doc);
                    Parameter param = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash");
                    Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");
                    FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));
                    List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();
                    foreach (ParameterFilterElement e in collector_filterview)
                    {
                        lista_filtros.Add(e);
                    }
                    List<ElementFilter> elementFilterList1 = new List<ElementFilter>();
                    List<ElementFilter> elementFilterList_no = new List<ElementFilter>();
                    FilterRule[] filterRule_lista = new FilterRule[]
                    {
                        ParameterFilterRuleFactory.CreateEqualsRule(param.Id,"YES", true),
                        ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id,(int)0)
                    };
                    elementFilterList1.Add(new ElementParameterFilter(filterRule_lista));
                    elementFilterList_no.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateNotContainsRule(param.Id, "YES", true)));
                    for (int i = 0; i < lista_filtros.Count(); i++)
                    {
                        if (lista_filtros[i].Name == "CLASH YES FILTER")
                        {
                            lista_ParameterFilterElement1.Add(lista_filtros[i]);
                            i = lista_filtros.Count();
                            break;
                        }
                    }
                    if (lista_ParameterFilterElement1.Count() == 0)
                    {
                        ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH YES FILTER", cats, elementFilterList1.First()); // ingresar un ElementFilter	
                        lista_ParameterFilterElement1.Add(parameterFilterElement);
                    }
                    for (int i = 0; i < lista_filtros.Count(); i++)
                    {
                        if (lista_filtros[i].Name == "CLASH NO FILTER")
                        {
                            lista_ParameterFilterElement_no.Add(lista_filtros[i]);
                            i = lista_filtros.Count();
                            break;
                        }
                    }
                    if (lista_ParameterFilterElement_no.Count() == 0)
                    {
                        ParameterFilterElement parameterFilterElement_no = ParameterFilterElement.Create(doc, "CLASH NO FILTER", cats, elementFilterList_no.First());
                        lista_ParameterFilterElement_no.Add(parameterFilterElement_no);
                    }
                    ParameterFilterElement aa = lista_ParameterFilterElement1.First();
                    ParameterFilterElement aa_no = lista_ParameterFilterElement_no.First();
                    OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
                    ogs3.SetProjectionLineColor(new Color(250, 0, 0));
                    ogs3.SetSurfaceForegroundPatternColor(new Color(250, 0, 0));
                    ogs3.SetSurfaceForegroundPatternVisible(true);
                    ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);
                    OverrideGraphicSettings ogs4 = new OverrideGraphicSettings();
                    ogs4.SetProjectionLineColor(new Color(192, 192, 192));
                    ogs4.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
                    ogs4.SetSurfaceForegroundPatternVisible(true);
                    ogs4.SetSurfaceForegroundPatternId(solidFillPattern.Id);
                    ogs4.SetHalftone(true);
                    activeView.AddFilter(aa.Id);
                    activeView.AddFilter(aa_no.Id);
                    activeView.SetFilterOverrides(aa.Id, ogs3);
                    activeView.SetFilterOverrides(aa_no.Id, ogs4);
                    ta.Commit();
                }
            }
        }
    }
}
