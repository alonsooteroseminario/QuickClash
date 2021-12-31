using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using forms = System.Windows.Forms;

namespace QuickClash
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class SectionBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uiApp.ActiveUIDocument.Document;
            Application app = uiApp.Application;

            // Get Active View
            var activeView = uidoc.ActiveView;

			var view = doc.ActiveView;
			string view_name = "COORD"; // nombre de la vista activa 
			List<bool> listabool_3 = new List<bool>();
			List<bool> listabool_2 = new List<bool>();
			List<bool> listabool_1 = new List<bool>();
			List<bool> listabool_4 = new List<bool>();

			string ee = "";
			using (var form = new Form3())
			{
				form.ShowDialog();

				if (form.DialogResult == forms.DialogResult.Cancel)
				{
					return Result.Cancelled;
				}

				if (form.DialogResult == forms.DialogResult.OK)
				{
					bool ee4 = form.checkBox_4; // TRUE aplicar
					listabool_4.Add(ee4);
					bool ee3 = form.checkBox_3; // TRUE aplicar
					listabool_3.Add(ee3);
					bool ee2 = form.checkBox_2; // TRUE aplicar
					listabool_2.Add(ee2);
					bool ee1 = form.checkBox_1; // TRUE aplicar
					listabool_1.Add(ee1);
				}

				if (listabool_3.First() && !(listabool_2.First()) && !(listabool_1.First()) && !(listabool_4.First()))
				{
					#region Create clash 3d view
					using (Transaction t = new Transaction(doc, "Create clash 3d view"))
					{
						t.Start();
						double Min_Z = double.MaxValue;

						// encontrar Min_Y , Min_X , Max_X , Max_Y
						double Min_X = double.MaxValue;
						double Min_Y = double.MaxValue;

						double Max_X = double.MinValue;
						double Max_Y = double.MinValue;
						double Max_Z = double.MinValue;

						List<ElementId> ids = new List<ElementId>();

						IList<Reference> references = uidoc.Selection.PickObjects(ObjectType.Element);

						foreach (Reference reference in references)
						{
							Element e = doc.GetElement(reference);
							ElementId eId = e.Id;
							ids.Add(eId);
						}


						foreach (ElementId id in ids)
						{
							Element elm = doc.GetElement(id);
							BoundingBoxXYZ box = elm.get_BoundingBox(view);
							if (box.Max.X > Max_X)
							{
								Max_X = box.Max.X;
							}
							if (box.Max.Y > Max_Y)
							{
								Max_Y = box.Max.Y;
							}
							if (box.Max.Z > Max_Z)
							{
								Max_Z = box.Max.Z;
							}

							if (box.Min.X < Min_X)
							{
								Min_X = box.Min.X;
							}
							if (box.Min.Y < Min_Y)
							{
								Min_Y = box.Min.Y;
							}
							if (box.Min.Z < Min_Z)
							{
								Min_Z = box.Min.Z;
							}
						}

						XYZ Max = new XYZ(Max_X, Max_Y, Max_Z);
						XYZ Min = new XYZ(Min_X, Min_Y, Min_Z);

						BoundingBoxXYZ myBox = new BoundingBoxXYZ();

						myBox.Min = Min;
						myBox.Max = Max;

						ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
													 OfClass(typeof(ViewFamilyType)).
													 Cast<ViewFamilyType>()
														 where v.ViewFamily == ViewFamily.ThreeDimensional
														 select v).First();

						View3D dupleView = View3D.CreateIsometric(doc, viewFamilyType.Id);

						FilteredElementCollector DUFcoll = new FilteredElementCollector(doc).OfClass(typeof(View3D));
						List<View3D> views = new List<View3D>(); // lista vacia
						List<View3D> views_COORD = new List<View3D>(); // lista vacia
						int numero = 1;
						foreach (View3D ve in DUFcoll)
						{
							views.Add(ve); // lista con todos los ViewSchedule del proyecto
						}
						for (int i = 0; i < views.Count(); i++)
						{
							View3D ve = views[i];
							if (ve.Name.Contains(view_name))
							{
								views_COORD.Add(ve); // todas la vistas con nombre igual COORD

							}

						}
						for (int i = 0; i < views_COORD.Count(); i++)
						{
							View3D ve = views_COORD[i];
							if (ve.Name.Contains(view_name + "  Copy "))
							{
								numero = numero + 1;
							}
							else
							{
								numero = 1; // solo COORD
							}
						}

						dupleView.Name = view_name + "  Copy " + (numero).ToString();



						(dupleView as View3D).SetSectionBox(myBox);

						dupleView.DisplayStyle = DisplayStyle.Shading;
						dupleView.DetailLevel = ViewDetailLevel.Fine;


						List<Element> riv = new List<Element>();
						FilteredElementCollector links = new FilteredElementCollector(doc, dupleView.Id);
						ElementCategoryFilter linkFilter = new ElementCategoryFilter(BuiltInCategory.OST_RvtLinks);
						links.WhereElementIsNotElementType();
						links.WherePasses(linkFilter);
						riv.AddRange(links.ToElements());


						t.Commit();

						uidoc.ActiveView = dupleView;
						ClashFilterMultipleElementsInView_UI.Do(commandData, dupleView);
						ClashSOLVEDFilterMultipleElementsInView_UI.Do(commandData, dupleView);

					}

					#endregion
				}
				else if (listabool_1.First() && !(listabool_2.First()) && !(listabool_3.First()) && !(listabool_4.First()))
				{
					ClashSectionBoxView_ELEMENT.Do(commandData);
				}
				else if (listabool_2.First() && !(listabool_1.First()) && !(listabool_3.First()) && !(listabool_4.First()))
				{
					ClashSectionBoxView_LEVELS.Do(commandData);
				}
				else if (listabool_4.First() && !(listabool_1.First()) && !(listabool_3.First()) && !(listabool_2.First()))
				{
					ClashSectionBoxView_ZONE.Do(commandData);
				}
				else
				{
					TaskDialog.Show("FINAL", "Selecciona SOLAMENTE 1 CHECK a la vez para que funcione correctamente por favor! :)");
				}

			}

			return Result.Succeeded;
		}
    }
}
