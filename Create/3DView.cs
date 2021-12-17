using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickClash
{
    public static class View
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

			ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
												 OfClass(typeof(ViewFamilyType)).
												 Cast<ViewFamilyType>()
											 where v.ViewFamily == ViewFamily.ThreeDimensional
											 select v).First();

			using (Transaction t = new Transaction(doc, "Create COORD view"))
			{
				t.Start();

				View3D COORD = View3D.CreateIsometric(doc, viewFamilyType.Id);

				COORD.DisplayStyle = DisplayStyle.Shading;
				COORD.DetailLevel = ViewDetailLevel.Fine;

				FilteredElementCollector DUFcoll = new FilteredElementCollector(doc).OfClass(typeof(View3D));
				List<View3D> views = new List<View3D>();
				List<View3D> views_COORD = new List<View3D>();
				int numero = 1;
				foreach (View3D ve in DUFcoll)
				{
					views.Add(ve);
				}
				for (int i = 0; i < views.Count(); i++)
				{
					View3D ve = views[i];
					if (ve.Name.Contains("COORD"))
					{
						views_COORD.Add(ve);
					}

				}

				if (views_COORD.Count() == 0)
				{
					COORD.Name = "COORD";
				}
				else
				{
					for (int i = 0; i < views_COORD.Count(); i++)
					{
						View3D ve = views_COORD[i];
						if (ve.Name.Contains("COORD" + "  Copy "))
						{
							numero = numero + 1;
						}
						else
						{
							numero = 1;
						}
					}
					COORD.Name = "COORD" + "  Copy " + (numero).ToString();
				}

				List<Element> riv = new List<Element>();
				FilteredElementCollector links = new FilteredElementCollector(doc, COORD.Id);
				ElementCategoryFilter linkFilter = new ElementCategoryFilter(BuiltInCategory.OST_RvtLinks);
				links.WhereElementIsNotElementType();
				links.WherePasses(linkFilter);
				riv.AddRange(links.ToElements());

				t.Commit();
				uidoc.ActiveView = COORD;
			}
		}
	}
}
