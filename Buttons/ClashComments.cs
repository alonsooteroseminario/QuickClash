using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using forms = System.Windows.Forms;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class ClashComments : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
            var activeView = uidoc.ActiveView;
            List<BuiltInCategory> bics_familyIns = GetLists.BuiltCategories(true);
            IList<Element> clash = new List<Element>();
            IList<Element> clash_no = new List<Element>();
            foreach (BuiltInCategory bic in bics_familyIns)
            {
                ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
                ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
                LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
                FilteredElementCollector MEcoll = new FilteredElementCollector(doc);
                IList<Element> mechanicalequipment = MEcoll.WherePasses(MEInstancesFilter).ToElements();

                foreach (Element elem in mechanicalequipment)
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
            IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
            IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_PipeCurves, "pipes");
            IList<Element> conduits = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_Conduit, "conduits");
            IList<Element> cabletrays = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_CableTray, "cabletrays");
            IList<Element> flexducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexDuctCurves, "flexducts");
            IList<Element> flexpipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_FlexPipeCurves, "flexpipes");
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
            using (var form = new Form1())
            {
                form.ShowDialog();
                if (form.DialogResult == forms.DialogResult.Cancel)
                {
                    return Result.Cancelled;
                }
                string param_value = form.textString.ToString();
                foreach (Element e in clash)
                {
                    Parameter param = e.LookupParameter("Clash Comments");
                    using (Transaction t = new Transaction(doc, "Set Comment value to Clash comments paramtere in Active View"))
                    {
                        t.Start();
                        param.Set(param_value);
                        //param2.Set(0);
                        t.Commit();
                    }
                }
            }
            using (Transaction t = new Transaction(doc, "Cambiar nombre Comentado"))
            {
                t.Start();
                activeView.Name = activeView.Name.ToString() + " - Comentado";
                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}
