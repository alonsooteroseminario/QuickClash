using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class GetAllClashElements
    {
        public static List<Element> Get(ExternalCommandData commandData)
        {
            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);

            List<Element> clash = new List<Element>();
            List<Element> clash_no = new List<Element>();

            foreach (BuiltInCategory bic in bics_familyIns)
            {

                IList<Element> familyinstance = GetElements.ElementsByBuiltCategoryActiveView(commandData, bic, "family_instances_all");

                foreach (Element elem in familyinstance)
                {
                    if (elem.LookupParameter("Clash").AsString() == "YES")
                    {
                        clash.Add(elem);
                    }
                    else
                    {
                        clash_no.Add(elem);
                    }
                }
            }


            IList<Element> ducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategoryActiveView(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");

            foreach (Element elem in ducts)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    clash.Add(elem);
                }
                else
                {
                    clash_no.Add(elem);
                }
            }
            foreach (Element elem in pipes)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    clash.Add(elem);
                }
                else
                {
                    clash_no.Add(elem);
                }
            }
            foreach (Element elem in conduits)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    clash.Add(elem);
                }
                else
                {
                    clash_no.Add(elem);
                }
            }
            foreach (Element elem in cabletrays)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    clash.Add(elem);
                }
                else
                {
                    clash_no.Add(elem);
                }
            }
            foreach (Element elem in flexducts)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    clash.Add(elem);
                }
                else
                {
                    clash_no.Add(elem);
                }
            }
            foreach (Element elem in flexpipes)
            {
                if (elem.LookupParameter("Clash").AsString() == "YES")
                {
                    clash.Add(elem);
                }
                else
                {
                    clash_no.Add(elem);
                }
            }
            return clash;
        }
    }
}