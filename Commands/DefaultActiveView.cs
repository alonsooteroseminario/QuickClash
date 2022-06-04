using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace QuickClash
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class DefaultActiveView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                List<BuiltInCategory> UI_list1 = GetLists.BuiltCategories(false);

                List<Element> allElements = new List<Element>();

                foreach (BuiltInCategory bic in UI_list1)
                {
                    if (bic == BuiltInCategory.OST_CableTray)
                    {
                        IList<Element> cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "cabletrays");
                        foreach (Element i in cabletrays)
                        {
                            allElements.Add(i);
                        }
                    }
                    if (bic == BuiltInCategory.OST_Conduit)
                    {
                        IList<Element> conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "conduits");
                        foreach (Element i in conduits)
                        {
                            allElements.Add(i);
                        }
                    }
                    if (bic == BuiltInCategory.OST_DuctCurves)
                    {
                        IList<Element> ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "ducts");
                        foreach (Element i in ducts)
                        {
                            allElements.Add(i);
                        }
                    }
                    if (bic == BuiltInCategory.OST_PipeCurves)
                    {
                        IList<Element> pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "pipes");
                        foreach (Element i in pipes)
                        {
                            allElements.Add(i);
                        }
                    }
                    if (bic == BuiltInCategory.OST_FlexDuctCurves)
                    {
                        IList<Element> flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexducts");
                        foreach (Element i in flexducts)
                        {
                            allElements.Add(i);
                        }
                    }
                    if (bic == BuiltInCategory.OST_FlexPipeCurves)
                    {
                        IList<Element> flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "flexpipes");
                        foreach (Element i in flexpipes)
                        {
                            allElements.Add(i);
                        }
                    }
                }
                List<BuiltInCategory> bics_finst = GetLists.BuiltCategories(true);
                foreach (BuiltInCategory bic in bics_finst)
                {
                    IList<Element> family_instances_all = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");
                    foreach (Element elem in family_instances_all)
                    {
                        allElements.Add(elem);
                    }
                }
                SetParameter.DefaultValues(commandData, allElements);

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