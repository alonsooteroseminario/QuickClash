using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash.Create
{
    class ClashParameters
    {
        public static void CreateWhenSharedParameter(ExternalCommandData commandData, bool exists)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            if (exists)
            {
                List<BuiltInCategory> bics = GetLists.BuiltCategories(false);
                CategorySet categories = app.Create.NewCategorySet();
                foreach (BuiltInCategory bic in bics)
                {
                    Category MECat = doc.Settings.Categories.get_Item(bic);
                    categories.Insert(MECat);
                }
                List<BuiltInCategory> bics_familyInstance = GetLists.BuiltCategories(true);
                foreach (var bic_familyInstanc in bics_familyInstance)
                {
                    Category MECat = doc.Settings.Categories.get_Item(bic_familyInstanc);
                    categories.Insert(MECat);
                }
                DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();
                DefinitionGroup myGroup = myDefinitionFile.Groups.get_Item("ClashParameters");
                foreach (var param in GetLists.Params())
                {
                    Definition myDefinition_ProductDate = myGroup.Definitions.get_Item(param);
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(param, ParameterType.Text);
                    if (param == "Clash" || param == "Clash Solved" || param == "Done")
                    {
                        option = new ExternalDefinitionCreationOptions(param, ParameterType.YesNo);
                    }
                    option.UserModifiable = true;
                    switch (param)
                    {
                        case "Clash":
                            option.Description = "Determina si el elemento tiene interferencia con otro. ";
                            break;
                        case "Clash  Category":
                            option.Description = "La Categoría del Elemento contra el que existe la interferencia. ";
                            break;
                        case "Clash Comments":
                            option.Description = "Comentario sobre la interferencia. ";
                            break;
                        case "Clash Grid Location":
                            option.Description = "Zona más cerca de la interferencia. ";
                            break;
                        case "Clash Solved":
                            option.Description = "Interferencia resuelta. Sí está activo en un Elemento, ese Elemento no será detectado con interferencias en el análisis. ";
                            break;
                        case "Done":
                            option.Description = "Tarea resuelta. ";
                            break;
                        case "ID Element":
                            option.Description = "Número de ID del Elemento";
                            break;
                        case "Percent Done":
                            option.Description = "Porcentaje de Tarea resuelta. ";
                            break;
                        case "Zone":
                            option.Description = "Zona General. ";
                            break;
                    }
                    InstanceBinding instanceBinding = app.Create.NewInstanceBinding(categories);
                    using (Transaction t = new Transaction(doc, "CreateClashParameter"))
                    {
                        t.Start();
                        doc.ParameterBindings.Insert(myDefinition_ProductDate, instanceBinding, BuiltInParameterGroup.PG_CONSTRAINTS);
                        t.Commit();
                    }
                }
            }
            else
            {
                List<string> listParam = GetLists.Params();
                CategorySet categories = app.Create.NewCategorySet();
                List<BuiltInCategory> bics = GetLists.BuiltCategories(false);
                foreach (BuiltInCategory bic in bics)
                {
                    Category MECat = doc.Settings.Categories.get_Item(bic);
                    categories.Insert(MECat);
                }
                List<BuiltInCategory> bics_familyInstance = GetLists.BuiltCategories(true);
                foreach (var bic_familyInstanc in bics_familyInstance)
                {
                    Category MECat = doc.Settings.Categories.get_Item(bic_familyInstanc);
                    categories.Insert(MECat);
                }
                DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();
                DefinitionGroup myGroup = myDefinitionFile.Groups.Create("ClashParameters");
                foreach (string paramName in listParam)
                {
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text);
                    if (paramName == "Clash" || paramName == "Clash Solved" || paramName == "Done")
                    {
                        option = new ExternalDefinitionCreationOptions(paramName, ParameterType.YesNo);
                    }
                    option.UserModifiable = true;
                    switch (paramName)
                    {
                        case "Clash":
                            option.Description = "Determina si el elemento tiene interferencia con otro. ";
                            break;
                        case "Clash  Category":
                            option.Description = "La Categoría del Elemento contra el que existe la interferencia. ";
                            break;
                        case "Clash Comments":
                            option.Description = "Comentario sobre la interferencia. ";
                            break;
                        case "Clash Grid Location":
                            option.Description = "Zona más cerca de la interferencia. ";
                            break;
                        case "Clash Solved":
                            option.Description = "Interferencia resuelta. Sí está activo en un Elemento, ese Elemento no será detectado con interferencias en el análisis. ";
                            break;
                        case "Done":
                            option.Description = "Tarea resuelta. ";
                            break;
                        case "ID Element":
                            option.Description = "Número de ID del Elemento";
                            break;
                        case "Percent Done":
                            option.Description = "Porcentaje de Tarea resuelta. ";
                            break;
                        case "Zone":
                            option.Description = "Zona General. ";
                            break;
                    }
                    Definition myDefinition_ProductDate = myGroup.Definitions.Create(option);
                    InstanceBinding instanceBinding = app.Create.NewInstanceBinding(categories);
                    using (Transaction t = new Transaction(doc, "CreateClashParameter"))
                    {
                        t.Start();
                        doc.ParameterBindings.Insert(myDefinition_ProductDate, instanceBinding, BuiltInParameterGroup.PG_CONSTRAINTS);
                        t.Commit();
                    }
                }
            }
        }
    }
}
