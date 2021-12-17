using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickClash
{
    public static class SetIDValue
	{
		/// <summary>
		/// Devuelve la lista de valores del diccionario ingresado como parámetro.
		/// </summary>
		/// <param>List of BuiltCategories.</param>
		public static void Do(ExternalCommandData commandData)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

		

			

		}
	}
}
