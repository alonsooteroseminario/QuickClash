using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash
{
    public static class SetParameter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void Do(ExternalCommandData commandData, IList<Element> elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            foreach (Element e in elements)
            {
                Parameter param = e.LookupParameter("Clash Solved");
                Parameter param2 = e.LookupParameter("Done");

                using (Transaction t = new Transaction(doc, "set parameter"))
                {
                    _ = t.Start();
                    //param.Set("YES");
                    //param.Set("NO");

                    //param2.Set("YES");
                    //param2.Set("NO");
                    _ = t.Commit();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param></param>
        public static void Id(ExternalCommandData commandData, IList<Element> elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            foreach (Element e in elements)
            {
                Parameter param = e.LookupParameter("ID Element");

                ElementId selectedId = e.Id;
                string idString = selectedId.IntegerValue.ToString();

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    _ = t.Start();
                    _ = param.Set(idString);
                    _ = t.Commit();
                }
            }
        }

        public static void DefaultValues(ExternalCommandData commandData, IList<Element> elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            foreach (Element e in elements)
            {
                Parameter param = e.LookupParameter("ID Element");
                Parameter clashParam = e.LookupParameter("Clash");
                Parameter param4 = e.LookupParameter("Clash Grid Location");
                Parameter param5 = e.LookupParameter("Clash Category");

                ElementId selectedId = e.Id;
                string voidString = "";

                using (Transaction t = new Transaction(doc, "ID Element"))
                {
                    _ = t.Start();
                    _ = param.Set(voidString);
                    _ = clashParam.Set(voidString);
                    _ = param4.Set(voidString);
                    _ = param5.Set(voidString);
                    _ = t.Commit();
                }
            }
        }
    }
}