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
	public static class ClashSectionBoxView_ZONE
	{

		public static void Do(ExternalCommandData commandData)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;


			Dictionary<string, XYZ> intersectionPoints = getIntersections(commandData, doc);

			List<View3D> tresDclashview = new List<View3D>();

			List<Element> clash_elements = new List<Element>();

			foreach (Element e in DYNO_GetAllClashElements_OnlyActiveView(commandData))
			{
				clash_elements.Add(e);
			}

			string msg = "";
			try
			{
				foreach (KeyValuePair<string, XYZ> kp in intersectionPoints)
				{
					List<Element> clash_elements_zone = new List<Element>();
					List<Element> clash_elements_zone_no = new List<Element>();
					List<ElementId> clash_elements_zone_ids = new List<ElementId>();

					string intersc = kp.Value.ToString(); // 

					string intersec_key = kp.Key.ToString(); // B / 1 - 0

					foreach (Element elem in clash_elements)
					{
						if (elem.LookupParameter("Clash Grid Location").AsString() == intersec_key)
						{
							clash_elements_zone.Add(elem);
						}
						else
						{
							clash_elements_zone_no.Add(elem);
						}
					}

					if (clash_elements_zone.Count() > 1)
					{

						// get a ViewFamilyType for a 3D View
						ViewFamilyType viewFamilyType = (from v in new FilteredElementCollector(doc).
														 OfClass(typeof(ViewFamilyType)).
														 Cast<ViewFamilyType>()
														 where v.ViewFamily == ViewFamily.ThreeDimensional
														 select v).First();


						using (Transaction t = new Transaction(doc, "Create clash 3d view"))
						{

							t.Start();
							// Create the 3d view
							View3D clashview = View3D.CreateIsometric(doc, viewFamilyType.Id);

							foreach (Element elem in clash_elements_zone)
							{
								clash_elements_zone_ids.Add(elem.Id);
							}

							double Min_Z1 = double.MaxValue;

							// encontrar Min_Y , Min_X , Max_X , Max_Y
							double Min_X1 = double.MaxValue;
							double Min_Y1 = double.MaxValue;

							double Max_X1 = double.MinValue;
							double Max_Y1 = double.MinValue;
							double Max_Z1 = double.MinValue;

							List<double> lista_Min_Z1 = new List<double>();
							List<double> lista_Min_X1 = new List<double>();
							List<double> lista_Min_Y1 = new List<double>();

							List<double> lista_Max_X1 = new List<double>();
							List<double> lista_Max_Y1 = new List<double>();
							List<double> lista_Max_Z1 = new List<double>();


							foreach (ElementId id in clash_elements_zone_ids)
							{
								Element elm = doc.GetElement(id);

								BoundingBoxXYZ box = elm.get_BoundingBox(null);

								if (box.Max.X > Max_X1)
								{
									Max_X1 = box.Max.X;
									lista_Max_X1.Add(Max_X1);
								}
								if (box.Max.Y > Max_Y1)
								{
									Max_Y1 = box.Max.Y;
									lista_Max_Y1.Add(Max_Y1);
								}
								if (box.Max.Z > Max_Z1)
								{
									Max_Z1 = box.Max.Z;
									lista_Max_Z1.Add(Max_Z1);
								}



								if (box.Min.X < Min_X1)
								{
									Min_X1 = box.Min.X;
									lista_Min_X1.Add(Min_X1);
								}
								if (box.Min.Y < Min_Y1)
								{
									Min_Y1 = box.Min.Y;
									lista_Min_Y1.Add(Min_Y1);
								}
								if (box.Min.Z < Min_Z1)
								{
									Min_Z1 = box.Min.Z;
									lista_Min_Z1.Add(Min_Z1);
								}
							}

							// menor a mayor
							lista_Max_X1.Sort();
							lista_Max_Y1.Sort();
							lista_Max_Z1.Sort();
							lista_Min_X1.Sort();
							lista_Min_Y1.Sort();
							lista_Min_Z1.Sort();

							//mayor a menor
							lista_Max_X1.Reverse();
							lista_Max_Y1.Reverse();
							lista_Max_Z1.Reverse();

							XYZ Max1 = new XYZ(lista_Max_X1.First(), lista_Max_Y1.First(), lista_Max_Z1.First());
							XYZ Min1 = new XYZ(lista_Min_X1.First(), lista_Min_Y1.First(), lista_Min_Z1.First());


							BoundingBoxXYZ bb_zone = new BoundingBoxXYZ();

							bb_zone.Min = Min1;
							bb_zone.Max = Max1;


							// Create a new BoundingBoxXYZ to define a 3D rectangular space
							BoundingBoxXYZ elem_bb = bb_zone;


							double offset = 2;

							var SectionBox = clashview.GetSectionBox();
							var vMax = SectionBox.Max + SectionBox.Transform.Origin;
							var vMin = SectionBox.Min + SectionBox.Transform.Origin;
							var bbMax = elem_bb.Max; //Point
							var bbMin = elem_bb.Min; //Point


							double Max_X = elem_bb.Max.X;


							double Max_Y = elem_bb.Max.Y;


							double Max_Z = elem_bb.Max.Z;



							double Min_X = elem_bb.Min.X;


							double Min_Y = elem_bb.Min.Y;


							double Min_Z = elem_bb.Min.Z;



							XYZ Max = new XYZ(Max_X + offset, Max_Y + offset, Max_Z + offset);
							XYZ Min = new XYZ(Min_X - offset, Min_Y - offset, Min_Z - offset);

							BoundingBoxXYZ myBox = new BoundingBoxXYZ();

							myBox.Min = Min;
							myBox.Max = Max;

							clashview.SetSectionBox(myBox);

							//					if (!clashview.IsTemplate) 
							//					{
							clashview.DisplayStyle = DisplayStyle.Shading;
							clashview.DetailLevel = ViewDetailLevel.Fine;



							msg = msg + Max.ToString() + Environment.NewLine + Min.ToString() + Environment.NewLine + "-----------------------------------------------------------" + Environment.NewLine;

							//BoundingBoxXYZ zone_bb = null;

							string number = (tresDclashview.Count() + 1).ToString();
							// Set the name of the view

							clashview.Name = number + ".- Zone    " + intersec_key + "     " + " Section Box    ";

							// Set the name of the transaction
							// A transaction can be renamed after it has been started
							t.SetName("Create view " + clashview.Name);

							clashview.SetSectionBox(myBox);
							tresDclashview.Add(clashview);
							t.Commit();
							// There cannot be an open transaction when the active view is set


						}


					}

				}
				uidoc.ActiveView = tresDclashview.First();
				DYNO_CreateClashFilterMultipleElementsInMultipleViews_UI(commandData, tresDclashview);
				DYNO_CreateClashSOLVEDFilterMultipleElementsInMultipleViews_UI(commandData, tresDclashview);

			}
			catch (Exception ex)
			{
				//TaskDialog.Show("Error ex", ex.ToString());
				throw;
			}


		}

		public static void DYNO_CreateClashFilterMultipleElementsInMultipleViews_UI(ExternalCommandData commandData, List<View3D> lista_3dviews) // Crea filtros en la vista activa
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			FilteredElementCollector elementss = new FilteredElementCollector(doc);
			FillPatternElement solidFillPattern = elementss.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);

			List<ElementId> cats = new List<ElementId>();

			cats.Add(new ElementId(BuiltInCategory.OST_DuctCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_PipeCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_Conduit));
			cats.Add(new ElementId(BuiltInCategory.OST_CableTray));
			cats.Add(new ElementId(BuiltInCategory.OST_FlexDuctCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_FlexPipeCurves));

			BuiltInCategory[] bics_familyIns = new BuiltInCategory[]
				{
					    //BuiltInCategory.OST_CableTray,
					    BuiltInCategory.OST_CableTrayFitting,
					    //BuiltInCategory.OST_Conduit,
					    BuiltInCategory.OST_ConduitFitting,
					    //BuiltInCategory.OST_DuctCurves,
					    BuiltInCategory.OST_DuctFitting,
						BuiltInCategory.OST_DuctTerminal,
						BuiltInCategory.OST_ElectricalEquipment,
						BuiltInCategory.OST_ElectricalFixtures,
						BuiltInCategory.OST_LightingDevices,
						BuiltInCategory.OST_LightingFixtures,
						BuiltInCategory.OST_MechanicalEquipment,
					    //BuiltInCategory.OST_PipeCurves,
					    //BuiltInCategory.OST_FlexDuctCurves,
					    //BuiltInCategory.OST_FlexPipeCurves,
					    BuiltInCategory.OST_PipeFitting,
						BuiltInCategory.OST_PlumbingFixtures,
						BuiltInCategory.OST_SpecialityEquipment,
						BuiltInCategory.OST_Sprinklers,
					//BuiltInCategory.OST_Wire,
				};

			foreach (BuiltInCategory bic in bics_familyIns)
			{
				cats.Add(new ElementId(bic));
			}



			foreach (View3D view3d in lista_3dviews)
			{

				//View activeView = this.ActiveUIDocument.ActiveView;
				activeView = view3d;

				List<ParameterFilterElement> lista_ParameterFilterElement1 = new List<ParameterFilterElement>();
				List<ParameterFilterElement> lista_ParameterFilterElement2 = new List<ParameterFilterElement>();
				List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();

				using (Transaction ta = new Transaction(doc, "create clash filter view"))
				{
					ta.Start();

					//activeView.Name = "COORD";

					FilteredElementCollector collector = new FilteredElementCollector(doc);

					Parameter param = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash");
					Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");

					FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));

					List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();


					foreach (ParameterFilterElement e in collector_filterview)
					{
						lista_filtros.Add(e);
					}

					List<ElementFilter> elementFilterList1 = new List<ElementFilter>();
					List<ElementFilter> elementFilterList_no = new List<ElementFilter>();

					FilterRule[] filterRule_lista = new FilterRule[]
					{
						ParameterFilterRuleFactory.CreateEqualsRule(param.Id,"YES", true),
						ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id,(int)0)
					};

					elementFilterList1.Add(new ElementParameterFilter(filterRule_lista));
					elementFilterList_no.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateNotContainsRule(param.Id, "YES", true)));


					for (int i = 0; i < lista_filtros.Count(); i++)
					{
						if (lista_filtros[i].Name == "CLASH YES FILTER")
						{
							lista_ParameterFilterElement1.Add(lista_filtros[i]);
							i = lista_filtros.Count();
							break;
						}

					}

					if (lista_ParameterFilterElement1.Count() == 0)
					{

						ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH YES FILTER", cats, elementFilterList1.First()); // ingresar un ElementFilter	
						lista_ParameterFilterElement1.Add(parameterFilterElement);


					}

					for (int i = 0; i < lista_filtros.Count(); i++)
					{
						if (lista_filtros[i].Name == "CLASH NO FILTER")
						{
							lista_ParameterFilterElement_no.Add(lista_filtros[i]);
							i = lista_filtros.Count();
							break;
						}

					}
					if (lista_ParameterFilterElement_no.Count() == 0)
					{
						ParameterFilterElement parameterFilterElement_no = ParameterFilterElement.Create(doc, "CLASH NO FILTER", cats, elementFilterList_no.First());
						lista_ParameterFilterElement_no.Add(parameterFilterElement_no);
					}

					ParameterFilterElement aa = lista_ParameterFilterElement1.First();
					ParameterFilterElement aa_no = lista_ParameterFilterElement_no.First();


					OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
					ogs3.SetProjectionLineColor(new Color(250, 0, 0));
					ogs3.SetSurfaceForegroundPatternColor(new Color(250, 0, 0));
					ogs3.SetSurfaceForegroundPatternVisible(true);
					ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);

					OverrideGraphicSettings ogs4 = new OverrideGraphicSettings();
					ogs4.SetProjectionLineColor(new Color(192, 192, 192));
					ogs4.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
					ogs4.SetSurfaceForegroundPatternVisible(true);
					ogs4.SetSurfaceForegroundPatternId(solidFillPattern.Id);
					ogs4.SetHalftone(true);

					activeView.AddFilter(aa.Id);
					activeView.AddFilter(aa_no.Id);

					activeView.SetFilterOverrides(aa.Id, ogs3);
					activeView.SetFilterOverrides(aa_no.Id, ogs4);

					ta.Commit();
				}
			}


		}// Crea filtros en la lista de vistas ingresadas

		public static void DYNO_CreateClashSOLVEDFilterMultipleElementsInMultipleViews_UI(ExternalCommandData commandData, List<View3D> lista_3dviews) // Crea filtros en la vista activa
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			FilteredElementCollector elementss = new FilteredElementCollector(doc);
			FillPatternElement solidFillPattern = elementss.OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().First(a => a.GetFillPattern().IsSolidFill);

			List<ElementId> cats = new List<ElementId>();

			cats.Add(new ElementId(BuiltInCategory.OST_DuctCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_PipeCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_Conduit));
			cats.Add(new ElementId(BuiltInCategory.OST_CableTray));
			cats.Add(new ElementId(BuiltInCategory.OST_FlexDuctCurves));
			cats.Add(new ElementId(BuiltInCategory.OST_FlexPipeCurves));

			BuiltInCategory[] bics_familyIns = new BuiltInCategory[]
				{
					    //BuiltInCategory.OST_CableTray,
					    BuiltInCategory.OST_CableTrayFitting,
					    //BuiltInCategory.OST_Conduit,
					    BuiltInCategory.OST_ConduitFitting,
					    //BuiltInCategory.OST_DuctCurves,
					    BuiltInCategory.OST_DuctFitting,
						BuiltInCategory.OST_DuctTerminal,
						BuiltInCategory.OST_ElectricalEquipment,
						BuiltInCategory.OST_ElectricalFixtures,
						BuiltInCategory.OST_LightingDevices,
						BuiltInCategory.OST_LightingFixtures,
						BuiltInCategory.OST_MechanicalEquipment,
					    //BuiltInCategory.OST_PipeCurves,
					    //BuiltInCategory.OST_FlexDuctCurves,
					    //BuiltInCategory.OST_FlexPipeCurves,
					    BuiltInCategory.OST_PipeFitting,
						BuiltInCategory.OST_PlumbingFixtures,
						BuiltInCategory.OST_SpecialityEquipment,
						BuiltInCategory.OST_Sprinklers,
					//BuiltInCategory.OST_Wire,
				};

			foreach (BuiltInCategory bic in bics_familyIns)
			{
				cats.Add(new ElementId(bic));
			}



			foreach (View3D view3d in lista_3dviews)
			{

				//View activeView = this.ActiveUIDocument.ActiveView;
				activeView = view3d;

				using (Transaction ta = new Transaction(doc, "create clash solved filter view"))
				{
					ta.Start();

					//activeView.Name = "COORD";
					List<ParameterFilterElement> lista_ParameterFilterElement = new List<ParameterFilterElement>();
					List<ParameterFilterElement> lista_ParameterFilterElement_no = new List<ParameterFilterElement>();

					FilteredElementCollector collector = new FilteredElementCollector(doc);

					Parameter param_solved = collector.OfClass(typeof(Duct)).FirstElement().LookupParameter("Clash Solved");

					FilteredElementCollector collector_filterview = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement));

					List<ParameterFilterElement> lista_filtros = new List<ParameterFilterElement>();
					foreach (ParameterFilterElement e in collector_filterview)
					{
						lista_filtros.Add(e);
					}

					List<ElementFilter> elementFilterList = new List<ElementFilter>();

					elementFilterList.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateEqualsRule(param_solved.Id, (int)1))); // Clash Solved , EQUAL,  False(int=0),


					for (int i = 0; i < lista_filtros.Count(); i++)
					{
						if (lista_filtros[i].Name == "CLASH SOLVED FILTER")
						{
							lista_ParameterFilterElement.Add(lista_filtros[i]);
							i = lista_filtros.Count();
							break;
						}

					}

					if (lista_ParameterFilterElement.Count() == 0)
					{
						ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, "CLASH SOLVED FILTER", cats, elementFilterList.First());
						lista_ParameterFilterElement.Add(parameterFilterElement);
					}

					ParameterFilterElement aa = lista_ParameterFilterElement.First();

					OverrideGraphicSettings ogs3 = new OverrideGraphicSettings();
					ogs3.SetProjectionLineColor(new Color(192, 192, 192));
					ogs3.SetSurfaceForegroundPatternColor(new Color(192, 192, 192));
					ogs3.SetSurfaceForegroundPatternVisible(true);
					ogs3.SetSurfaceForegroundPatternId(solidFillPattern.Id);

					activeView.AddFilter(aa.Id);
					activeView.SetFilterOverrides(aa.Id, ogs3);

					ta.Commit();
				}
			}


		}// Crea filtros en la lista de vistas ingresadas

		public static List<Element> DYNO_GetAllClashElements_OnlyActiveView(ExternalCommandData commandData)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			// FAMILY INSTANCES

			BuiltInCategory[] bics_familyIns = new BuiltInCategory[]
				{
					    //BuiltInCategory.OST_CableTray,
					    BuiltInCategory.OST_CableTrayFitting,
					    //BuiltInCategory.OST_Conduit,
					    BuiltInCategory.OST_ConduitFitting,
					    //BuiltInCategory.OST_DuctCurves,
					    BuiltInCategory.OST_DuctFitting,
						BuiltInCategory.OST_DuctTerminal,
						BuiltInCategory.OST_ElectricalEquipment,
						BuiltInCategory.OST_ElectricalFixtures,
						BuiltInCategory.OST_LightingDevices,
						BuiltInCategory.OST_LightingFixtures,
						BuiltInCategory.OST_MechanicalEquipment,
					    //BuiltInCategory.OST_PipeCurves,
					    BuiltInCategory.OST_FlexDuctCurves,
						BuiltInCategory.OST_FlexPipeCurves,
						BuiltInCategory.OST_PipeFitting,
						BuiltInCategory.OST_PlumbingFixtures,
						BuiltInCategory.OST_SpecialityEquipment,
						BuiltInCategory.OST_Sprinklers,
						BuiltInCategory.OST_Wire,
				};

			// get elements with "clash" parameter value == "YES"
			List<Element> clash = new List<Element>();
			// get elements with "clash" parameter value == "NO"
			List<Element> clash_no = new List<Element>();

			foreach (BuiltInCategory bic in bics_familyIns)
			{
				ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
				// Create a category filter for MechanicalEquipment
				ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
				// Create a logic And filter for all MechanicalEquipment Family
				LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
				// Apply the filter to the elements in the active document
				FilteredElementCollector MEcoll = new FilteredElementCollector(doc, activeView.Id);
				IList<Element> mechanicalequipment = MEcoll.WherePasses(MEInstancesFilter).ToElements();



				foreach (Element elem in mechanicalequipment)
				{
					if (elem.LookupParameter("Clash").AsString() == "YES")
					{
						clash.Add(elem);
					}
					else
					{
						clash_no.Add(elem);
					}
				}

			}

			// ELements
			ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
			ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
			ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
			ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
			ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
			ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));

			// Create a category filter for Ducts
			ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
			ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
			ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
			ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
			ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
			ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);


			// Create a logic And filter for all MechanicalEquipment Family
			LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);
			LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);
			LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);
			LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);
			LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);
			LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

			// Apply the filter to the elements in the active document
			FilteredElementCollector DUcoll = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements();

			FilteredElementCollector DUcoll2 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> pipes = DUcoll2.WherePasses(DUInstancesFilter2).ToElements();

			FilteredElementCollector DUcoll3 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> conduits = DUcoll3.WherePasses(DUInstancesFilter3).ToElements();

			FilteredElementCollector DUcoll4 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> cabletrays = DUcoll4.WherePasses(DUInstancesFilter4).ToElements();

			FilteredElementCollector DUcoll5 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> flexducts = DUcoll5.WherePasses(DUInstancesFilter5).ToElements();

			FilteredElementCollector DUcoll6 = new FilteredElementCollector(doc, activeView.Id);
			IList<Element> flexpipes = DUcoll6.WherePasses(DUInstancesFilter6).ToElements();


			foreach (Element elem in ducts)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in pipes)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in conduits)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in cabletrays)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in flexducts)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			foreach (Element elem in flexpipes)
			{
				if (elem.LookupParameter("Clash").AsString() == "YES")
				{
					clash.Add(elem);
				}
				else
				{
					clash_no.Add(elem);
				}
			}
			return clash;

		}

		public static Dictionary<string, XYZ> getIntersections(ExternalCommandData commandData, Document _doc)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;

			string coordinate = null;
			string coordA = null;
			string coordB = null;
			string coordC = null;
			Dictionary<string, XYZ> intersectionPoints = new Dictionary<string, XYZ>();


			ICollection<ElementId> grids = new FilteredElementCollector(doc).WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Grids)).WhereElementIsNotElementType().ToElementIds();

			ICollection<ElementId> refgrid = new FilteredElementCollector(doc).WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Grids)).WhereElementIsNotElementType().ToElementIds();
			foreach (ElementId eid in grids)

			{
				Grid g = _doc.GetElement(eid) as Grid;

				coordA = g.Name;

				Curve c = g.Curve;

				refgrid.Remove(eid);

				foreach (ElementId eid2 in refgrid)
				{
					Grid g2 = _doc.GetElement(eid2) as Grid;

					coordB = g2.Name;
					Curve c2 = g2.Curve;

					IntersectionResultArray results;

					SetComparisonResult result = c.Intersect(c2, out results);

					if (result != SetComparisonResult.Overlap)
					{ }

					else if (results == null || results.Size != 1)

					{ }

					else if (results.Size > 0)
					{
						for (int i = 0; i < results.Size; i++)
						{
							IntersectionResult iresult = results.get_Item(i);
							coordC = i.ToString();

							coordinate = coordA + " / " + coordB + " - " + coordC;

							XYZ point = iresult.XYZPoint;

							intersectionPoints.Add(coordinate, point);

						}
					}
					continue;
				}

			}
			return intersectionPoints;
		}
	}
}
