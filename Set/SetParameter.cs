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
	public static class SetParameter
	{
		/// <summary>
		/// Devuelve la lista de valores del diccionario ingresado como parámetro.
		/// </summary>
		/// <param>List of BuiltCategories.</param>
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
					t.Start();
					param.Set(1);
					param.Set(0);

					param2.Set(1);
					param2.Set(0);
					t.Commit();
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

				Autodesk.Revit.DB.ElementId selectedId = e.Id;
				string idString = selectedId.IntegerValue.ToString();

				using (Transaction t = new Transaction(doc, "ID Element"))
				{
					t.Start();
					param.Set(idString);
					t.Commit();
				}
			}
		}
	}
}
