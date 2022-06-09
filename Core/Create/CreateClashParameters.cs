using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace QuickClash.Create
{
    internal class ClashParameters
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
                    _ = categories.Insert(MECat);
                }
                List<BuiltInCategory> bics_familyInstance = GetLists.BuiltCategories(true);
                foreach (BuiltInCategory bic_familyInstanc in bics_familyInstance)
                {
                    Category MECat = doc.Settings.Categories.get_Item(bic_familyInstanc);
                    _ = categories.Insert(MECat);
                }
                DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();
                DefinitionGroup myGroup = myDefinitionFile.Groups.get_Item("Clash Parameters");
                foreach (string param in GetLists.Params())
                {
                    Definition myDefinition_ProductDate = myGroup.Definitions.get_Item(param);
                    InstanceBinding instanceBinding = app.Create.NewInstanceBinding(categories);
                    using (Transaction t = new Transaction(doc, "CreateClashParameter"))
                    {
                        _ = t.Start();
                        _ = doc.ParameterBindings.Insert(myDefinition_ProductDate, instanceBinding, BuiltInParameterGroup.PG_CONSTRAINTS);
                        _ = t.Commit();
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
                    _ = categories.Insert(MECat);
                }
                List<BuiltInCategory> bics_familyInstance = GetLists.BuiltCategories(true);
                foreach (BuiltInCategory bic_familyInstanc in bics_familyInstance)
                {
                    Category MECat = doc.Settings.Categories.get_Item(bic_familyInstanc);
                    _ = categories.Insert(MECat);
                }
                DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();
                DefinitionGroup myGroup = myDefinitionFile.Groups.Create("Clash Parameters");

                foreach (string paramName in listParam)
                {
                    #if REVIT2018
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text)                    
                    {
                        UserModifiable = true
                    };;
                    #endif
                    #if REVIT2019
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text)
                    {
                        UserModifiable = true
                    };;
                    #endif
                    #if REVIT2020
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text)                    
                    {
                        UserModifiable = true
                    };;
                    #endif
                    #if REVIT2021
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text)                    
                    {
                        UserModifiable = true
                    };;
                    #endif
                    #if REVIT2022
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, SpecTypeId.String.Text)
                    {
                        UserModifiable = true
                    };
                    #endif
                    #if REVIT2023
                    ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, SpecTypeId.String.Text)                    
                    {
                        UserModifiable = true
                    };
                    #endif

                    switch (paramName)
                    {
                        case "Clash":
                            option.Description = "Determines if the element has interference with another. ";
                            break;

                        case "Clash  Category":
                            option.Description = "The Category of the Element against which the interference exists. ";
                            break;

                        case "Clash Comments":
                            option.Description = "Comment on interference. ";
                            break;

                        case "Clash Grid Location":
                            option.Description = "Area closest to the interference. ";
                            break;

                        case "Clash Solved":
                            option.Description = "Interference resolved. If it is active on an Element, this Element will not be detected with interference in the analysis. ";
                            break;

                        case "Done":
                            option.Description = "Solved task. ";
                            break;

                        case "ID Element":
                            option.Description = "Element ID number.";
                            break;

                        case "Percent Done":
                            option.Description = "Task Solved Percentage.";
                            break;

                        case "Zone":
                            option.Description = "Zona General. ";
                            break;

                        default:
                            break;
                    }
                    Definition myDefinition_ProductDate = myGroup.Definitions.Create(option);
                    InstanceBinding instanceBinding = app.Create.NewInstanceBinding(categories);
                    using (Transaction t = new Transaction(doc, "CreateClashParameter"))
                    {
                        _ = t.Start();
                        _ = doc.ParameterBindings.Insert(myDefinition_ProductDate, instanceBinding, BuiltInParameterGroup.PG_CONSTRAINTS);
                        _ = t.Commit();
                    }
                }
            }
        }
    }
}