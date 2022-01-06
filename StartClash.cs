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
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    internal class StartClash : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Application app = uiapp.Application;
                Document doc = uidoc.Document;


                #region try region

                List<DefinitionGroup> defGroups = new List<DefinitionGroup>();

                TaskDialog.Show("Dynoscript", "Recuerda:\n\n1.- Tener los Worksets del proyecto en modo Editables. \n\n2.- Si el Modelo es muy grande algunas tareas pueden demorar varios minutos. "
                                + Environment.NewLine + "\n3.- Si ya probaste Quick Clash por favor déjanos tus comentarios en nuestra página : \n" + Environment.NewLine + new Uri("https://www.dynoscript.com/quickclash/")
                                + " \n\nSe recibe cualquier tipo de FeedBack. Muchas Gracias! :) ", TaskDialogCommonButtons.Ok);
                DefinitionFile sharedParameterFile = app.OpenSharedParameterFile(); // Abrimos el archivo .txt de shared parameters
                foreach (DefinitionGroup dg in sharedParameterFile.Groups)
                {
                    defGroups.Add(dg);
                }

                IList<Element> ducts = Get.ElementsByBuiltCategory(commandData, BuiltInCategory.OST_DuctCurves, "ducts");
                Element e = ducts.First(); // primer ducto
                Parameter param = e.LookupParameter("Clash"); // buscamos el parametro "Clash" con el elemento e
                List<string> list_dg = new List<string>();

                for (int i = 0; i < defGroups.Count(); i++)
                {
                    DefinitionGroup dg = defGroups[i];
                    list_dg.Add(dg.Name.ToString());
                }
                //¿existe el group shared parameter?
                if (list_dg.Contains("ClashParameters")) // si existe group parameter
                {
                    // ¿existe parametro Clash?
                    if (param != null)// true
                    {
                        // no hacer nada
                    }
                    else//false
                    {
                        ClashParameters.CreateWhenSharedParameter(commandData, true);
                    }
                }
                else // no existe group parameter
                {
                    // ¿existe parametro Clash?
                    if (param != null) //true
                    {
                        // no hacer nada
                    }
                    else //false
                    {
                        ClashParameters.CreateWhenSharedParameter(commandData, false);
                    }
                }

                View.Create(commandData);
                SetIDValue.Do(commandData);
                SetEmptyYesNoParameters.Do(commandData);
                //ClashSchedules.Create(commandData);

                #endregion

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", e.Message.ToString());
                return Result.Cancelled;
            }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return Result.Succeeded;
        }
    }
}
