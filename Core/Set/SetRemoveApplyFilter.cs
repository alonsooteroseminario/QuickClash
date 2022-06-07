using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash
{
    public static class SetRemoveApplyFilter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void Do(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            Autodesk.Revit.DB.View activeView = uidoc.ActiveView;

            FilteredElementCollector elements = new FilteredElementCollector(doc);
            FillPatternElement solidFillPattern = elements.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);
            List<ElementId> cats = new List<ElementId>
            {
                new ElementId(BuiltInCategory.OST_DuctCurves),
                new ElementId(BuiltInCategory.OST_PipeCurves),
                new ElementId(BuiltInCategory.OST_Conduit),
                new ElementId(BuiltInCategory.OST_CableTray),
                new ElementId(BuiltInCategory.OST_FlexDuctCurves),
                new ElementId(BuiltInCategory.OST_FlexPipeCurves)
            };
            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                cats.Add(new ElementId(bic));
            }
            List<ParameterFilterElement> lista_ParameterFilterElement1 = new List<ParameterFilterElement>();
            List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();
            List<ParameterFilterElement> lista_ParameterFilterElement_solved = new List<ParameterFilterElement>();

            using (Transaction ta = new Transaction(doc, "create clash filter view"))
            {
                _ = ta.Start();
                FilteredElementCollector collector = new FilteredElementCollector(doc);

                Parameter param = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash");
                Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");

                FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));
                List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();
                foreach (ParameterFilterElement e in collector_filterview)
                {
                    lista_filtros.Add(e);
                }
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
                    ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH YES FILTER", cats, new ElementParameterFilter(new FilterRule[] {
                            ParameterFilterRuleFactory.CreateEqualsRule(param.Id,"YES", true),
                            ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id,"", true)
                    }));
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
                    ParameterFilterElement parameterFilterElement_no = ParameterFilterElement.Create(doc, "CLASH NO FILTER", cats, new ElementParameterFilter(
                        ParameterFilterRuleFactory.CreateNotContainsRule(param.Id, "YES", true)));
                    lista_ParameterFilterElement_no.Add(parameterFilterElement_no);
                }

                for (int i = 0; i < lista_filtros.Count(); i++)
                {
                    if (lista_filtros[i].Name == "CLASH SOLVED FILTER")
                    {
                        lista_ParameterFilterElement_solved.Add(lista_filtros[i]);
                        i = lista_filtros.Count();
                        break;
                    }
                }
                if (lista_ParameterFilterElement_solved.Count() == 0)
                {
                    ParameterFilterElement parameterFilterElement_solved = ParameterFilterElement.Create(doc, "CLASH SOLVED FILTER", cats, new ElementParameterFilter(new FilterRule[]
                    {
                        ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id, "YES", true)
                    }));
                    lista_ParameterFilterElement_solved.Add(parameterFilterElement_solved);
                }

                ParameterFilterElement ParameterFilterElement1 = lista_ParameterFilterElement1.First();
                ParameterFilterElement ParameterFilterElement1_no = lista_ParameterFilterElement_no.First();
                ParameterFilterElement ParameterFilterElement1_solved = lista_ParameterFilterElement_solved.First();

                OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
                _ = ogs3.SetProjectionLineColor(new Color(250, 0, 0));
                _ = ogs3.SetSurfaceForegroundPatternColor(new Color(250, 0, 0));
                _ = ogs3.SetSurfaceForegroundPatternVisible(true);
                _ = ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);

                OverrideGraphicSettings ogs4 = new OverrideGraphicSettings();
                _ = ogs4.SetProjectionLineColor(new Color(192, 192, 192));
                _ = ogs4.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
                _ = ogs4.SetSurfaceForegroundPatternVisible(true);
                _ = ogs4.SetSurfaceForegroundPatternId(solidFillPattern.Id);
                _ = ogs4.SetHalftone(true);

                FilteredElementCollector collector_filterActiveview = new FilteredElementCollector(doc, activeView.Id).OfClass(typeof(ParameterFilterElement));
                List<ParameterFilterElement> lista_filtrosActiveView = new List<ParameterFilterElement>();
                foreach (ParameterFilterElement e in collector_filterActiveview)
                {
                    lista_filtrosActiveView.Add(e);
                }
                if (lista_filtrosActiveView.Count() != 0)
                {
                    for (int i = 0; i < lista_filtrosActiveView.Count(); i++)
                    {
                        if (lista_filtrosActiveView[i].Name == "CLASH YES FILTER")
                        {
                            i = lista_filtrosActiveView.Count();
                            break;
                        }
                    }
                }

                activeView.RemoveFilter(ParameterFilterElement1.Id);
                activeView.RemoveFilter(ParameterFilterElement1_no.Id);
                activeView.RemoveFilter(ParameterFilterElement1_solved.Id);

                _ = ta.Commit();
            }
        }
    }
}