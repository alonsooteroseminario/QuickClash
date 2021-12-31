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
	public static class ClashSectionBoxView_LEVELS
	{

		public static void Do(ExternalCommandData commandData)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			// get list of all levels
			IList<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(l => l.Elevation).ToList();

			// get a ViewFamilyType for a 3D View
			ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
											 OfClass(typeof(ViewFamilyType)).
											 Cast<ViewFamilyType>()
											 where v.ViewFamily == ViewFamily.ThreeDimensional
											 select v).First();

			List<View3D> lista3dview = new List<View3D>();

			using (Transaction t = new Transaction(doc, "Create view"))
			{
				int ctr = 0;
				// loop through all levels
				foreach (Level level in levels)
				{
					t.Start();

					// Create the 3d view
					View3D view = View3D.CreateIsometric(doc, viewFamilyType.Id);

					// Set the name of the view
					view.Name = "COORD - Nivel " + level.Name;

					// Set the name of the transaction
					// A transaction can be renamed after it has been started
					t.SetName("Create view " + view.Name);

					// Create a new BoundingBoxXYZ to define a 3D rectangular space
					BoundingBoxXYZ boundingBoxXYZ = new BoundingBoxXYZ();

					// Set the lower left bottom corner of the box
					// Use the Z of the current level.
					// X & Y values have been hardcoded based on this RVT geometry
					boundingBoxXYZ.Min = new XYZ(-50, -100, level.Elevation);

					// Determine the height of the bounding box
					double zOffset = 0;
					// If there is another level above this one, use the elevation of that level
					if (levels.Count > ctr + 1)
						zOffset = levels.ElementAt(ctr + 1).Elevation;
					// If this is the top level, use an offset of 10 feet
					else
						zOffset = level.Elevation + 10;
					boundingBoxXYZ.Max = new XYZ(200, 125, zOffset);

					// Apply this bouding box to the view's section box
					(view as View3D).SetSectionBox(boundingBoxXYZ);
					lista3dview.Add(view);

					if (!view.IsTemplate)
					{
						view.DisplayStyle = DisplayStyle.Shading;
						view.DetailLevel = ViewDetailLevel.Fine;
					}

					t.Commit();

					// Open the just-created view
					// There cannot be an open transaction when the active view is set
					uidoc.ActiveView = view;

					ctr++;
				}
			}
			foreach (View3D view in lista3dview)
			{
				ClashFilterMultipleElementsInView_UI.Do(commandData, view);
				ClashSOLVEDFilterMultipleElementsInView_UI.Do(commandData, view);
			}


		}
	}
}
