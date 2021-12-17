﻿using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickClash.Create
{
    class ClashParameters
    {
		public static void WhenSharedParameter(ExternalCommandData commandData, bool exists)
        {
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Application app = uiapp.Application;
			Document doc = uidoc.Document;

            if (exists)
            {
				List<BuiltInCategory> bics = Lists.BuiltCategories();
				CategorySet categories = app.Create.NewCategorySet();
				foreach (BuiltInCategory bic in bics)
				{
					Category MECat = doc.Settings.Categories.get_Item(bic);
					categories.Insert(MECat);
				}
				// open shared parameter file
				DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();

				// get a group
				DefinitionGroup myGroup = myDefinitionFile.Groups.get_Item("ClashParameters");

				foreach (var param in Lists.Params())
				{
					Definition myDefinition_ProductDate = myGroup.Definitions.get_Item(param);
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
				List<string> listParam = Lists.Params();

				CategorySet categories = app.Create.NewCategorySet();

				List<BuiltInCategory> bics = Lists.BuiltCategories();

				foreach (BuiltInCategory bic in bics)
				{
					Category MECat = doc.Settings.Categories.get_Item(bic);
					categories.Insert(MECat);
				}


				DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();
				DefinitionGroup myGroup = myDefinitionFile.Groups.Create("ClashParameters");

				foreach (string paramName in listParam)
				{
					ExternalDefinitionCreationOptions option = new ExternalDefinitionCreationOptions(paramName, ParameterType.Text);
					option.UserModifiable = true;
					option.Description = "";
					Definition myDefinition_ProductDate = myGroup.Definitions.Create(option);
					InstanceBinding instanceBinding = app.Create.NewInstanceBinding(categories);
					using (Transaction t = new Transaction(doc, "CreateClashParameter"))
					{
						t.Start();
						doc.ParameterBindings.Insert(myDefinition_ProductDate, instanceBinding, BuiltInParameterGroup.PG_CONSTRAINTS);
						t.Commit();
					}
				}
				CreateView.Do(commandData);
			}
		}
	}
}