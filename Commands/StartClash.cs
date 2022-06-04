#region Namespaces

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuickClash.Create;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace QuickClash
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class StartClash : IExternalCommand
    {
        public bool Tests = false;

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            using (Transaction trans = new Transaction(doc, "Elements Tests"))
            {
                _ = trans.Start();
                if (Tests)
                {
                }
                _ = trans.Commit();
            }

            try
            {
                List<DefinitionGroup> defGroups = new List<DefinitionGroup>();

                DefinitionFile sharedParameterFile = app.OpenSharedParameterFile();

                foreach (DefinitionGroup dg in sharedParameterFile.Groups)
                {
                    defGroups.Add(dg);
                }
                IList<Element> ducts = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
                Parameter param = null;

                IList<Element> pipes = GetElements.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "pipes");
                Parameter param2 = null;

                foreach (Element item in ducts)
                {
                    if (ducts.Count() > 0)
                    {
                        param = item.LookupParameter("Clash");
                        break;
                    }
                }
                foreach (Element item in pipes)
                {
                    if (pipes.Count() > 0)
                    {
                        param2 = item.LookupParameter("Clash");
                        break;
                    }
                }
                if (ducts.Count() == 0)
                {
                    TaskDialog.Show("Error", "The model does not contain anyduct modeled. To start please draw a pipe. Thank you!");
                    LogProgress.UpDate("The model does not contain anyduct modeled. To start please draw a pipe. Thank you!");
                    return Result.Cancelled;
                }
                if (pipes.Count() == 0)
                {
                    TaskDialog.Show("Error", "The model does not contain any pipemodeled. To start please draw a pipe. Thank you!");
                    LogProgress.UpDate("The model does not contain any pipemodeled. To start please draw a pipe. Thank you!");
                    return Result.Cancelled;
                }
                if (ducts.Count() == 0 && pipes.Count() == 0)
                {
                    TaskDialog.Show("Error", "The model does not contain any pipe or duct modeled. To start please draw on pipe or duct. Thank you!");
                    LogProgress.UpDate("The model does not contain any pipe or duct modeled. To start please draw on pipe or duct. Thank you!");
                    return Result.Cancelled;
                }

                List<string> list_dg = new List<string>();
                for (int i = 0; i < defGroups.Count(); i++)
                {
                    DefinitionGroup dg = defGroups[i];
                    list_dg.Add(dg.Name.ToString());
                }
                if (list_dg.Contains("Clash Parameters"))
                {
                    if (param == null && param2 == null)
                    {
                        ClashParameters.CreateWhenSharedParameter(commandData, true);
                    }
                }
                else
                {
                    if (param == null && param2 == null)
                    {
                        ClashParameters.CreateWhenSharedParameter(commandData, false);
                    }
                }

                View.Create(commandData);

                FilteredElementCollector schedules = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule));
                bool val = true;
                foreach (var schedule in schedules)
                {
                    if (schedule.Name.ToString().Contains("OST_"))
                    {
                        val = false;
                        break;
                    }
                }
                if (val)
                {
                    ClashSchedules.Create(commandData);
                }
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                _ = TaskDialog.Show("Error", e.Message);
                LogProgress.UpDate(e.Message);
                return Result.Failed;
            }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            return application is null ? throw new ArgumentNullException(nameof(application)) : Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return application is null ? throw new ArgumentNullException(nameof(application)) : Result.Succeeded;
        }
    }
}