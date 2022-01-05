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
	public static class Get
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param></param>
		public static IList<Element> ElementsByBuiltCategory(ExternalCommandData commandData, BuiltInCategory bic, string builtCategory)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

			ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
			ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
			LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
			FilteredElementCollector MEcoll = new FilteredElementCollector(doc);

			ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
			ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
			ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
			ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
			ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
			ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));

			ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
			ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
			ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
			ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
			ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
			ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);

			LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);
			LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);
			LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);
			LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);
			LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);
			LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

			IList<Element> salida = null;

			IList <Element> family_instances_all = MEcoll.WherePasses(MEInstancesFilter).ToElements();
			FilteredElementCollector DUcoll = new FilteredElementCollector(doc);

			IList<Element> ducts = DUcoll.WherePasses(DUInstancesFilter).ToElements();
			FilteredElementCollector DUcoll2 = new FilteredElementCollector(doc);
			IList<Element> pipes = DUcoll2.WherePasses(DUInstancesFilter2).ToElements();
			FilteredElementCollector DUcoll3 = new FilteredElementCollector(doc);
			IList<Element> conduits = DUcoll3.WherePasses(DUInstancesFilter3).ToElements();
			FilteredElementCollector DUcoll4 = new FilteredElementCollector(doc);
			IList<Element> cabletrays = DUcoll4.WherePasses(DUInstancesFilter4).ToElements();
			FilteredElementCollector DUcoll5 = new FilteredElementCollector(doc);
			IList<Element> flexducts = DUcoll5.WherePasses(DUInstancesFilter5).ToElements();
			FilteredElementCollector DUcoll6 = new FilteredElementCollector(doc);
			IList<Element> flexpipes = DUcoll6.WherePasses(DUInstancesFilter6).ToElements();



			if (builtCategory == "family_instances_all")
            {
				salida = family_instances_all;
            }
			if (builtCategory == "ducts")
			{
				salida = ducts;
			}
			if (builtCategory == "pipes")
			{
				salida = pipes;
			}
			if (builtCategory == "conduits")
			{
				salida = conduits;
			}
			if (builtCategory == "cabletrays")
			{
				salida = cabletrays;
			}
			if (builtCategory == "flexducts")
			{
				salida = flexducts;
			}
			if (builtCategory == "flexpipes")
			{
				salida = flexpipes;
			}
			return salida;

		}

		public static IList<Element> ElementsByBuiltCategoryActiveView(ExternalCommandData commandData, BuiltInCategory bic, string builtCategory)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;
			var activeView = uidoc.ActiveView;


			ElementClassFilter familyFilter = new ElementClassFilter(typeof(FamilyInstance));
			ElementCategoryFilter MECategoryfilter = new ElementCategoryFilter(bic);
			LogicalAndFilter MEInstancesFilter = new LogicalAndFilter(familyFilter, MECategoryfilter);
			FilteredElementCollector MEcoll = new FilteredElementCollector(doc);

			ElementClassFilter elemFilter = new ElementClassFilter(typeof(Duct));
			ElementClassFilter elemFilter2 = new ElementClassFilter(typeof(Pipe));
			ElementClassFilter elemFilter3 = new ElementClassFilter(typeof(Conduit));
			ElementClassFilter elemFilter4 = new ElementClassFilter(typeof(CableTray));
			ElementClassFilter elemFilter5 = new ElementClassFilter(typeof(FlexDuct));
			ElementClassFilter elemFilter6 = new ElementClassFilter(typeof(FlexPipe));

			ElementCategoryFilter DUCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_DuctCurves);
			ElementCategoryFilter DUCategoryfilter2 = new ElementCategoryFilter(BuiltInCategory.OST_PipeCurves);
			ElementCategoryFilter DUCategoryfilter3 = new ElementCategoryFilter(BuiltInCategory.OST_Conduit);
			ElementCategoryFilter DUCategoryfilter4 = new ElementCategoryFilter(BuiltInCategory.OST_CableTray);
			ElementCategoryFilter DUCategoryfilter5 = new ElementCategoryFilter(BuiltInCategory.OST_FlexDuctCurves);
			ElementCategoryFilter DUCategoryfilter6 = new ElementCategoryFilter(BuiltInCategory.OST_FlexPipeCurves);

			LogicalAndFilter DUInstancesFilter = new LogicalAndFilter(elemFilter, DUCategoryfilter);
			LogicalAndFilter DUInstancesFilter2 = new LogicalAndFilter(elemFilter2, DUCategoryfilter2);
			LogicalAndFilter DUInstancesFilter3 = new LogicalAndFilter(elemFilter3, DUCategoryfilter3);
			LogicalAndFilter DUInstancesFilter4 = new LogicalAndFilter(elemFilter4, DUCategoryfilter4);
			LogicalAndFilter DUInstancesFilter5 = new LogicalAndFilter(elemFilter5, DUCategoryfilter5);
			LogicalAndFilter DUInstancesFilter6 = new LogicalAndFilter(elemFilter6, DUCategoryfilter6);

			IList<Element> salida = null;

			IList<Element> family_instances_all = MEcoll.WherePasses(MEInstancesFilter).ToElements();
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



			if (builtCategory == "family_instances_all")
			{
				salida = family_instances_all;
			}
			if (builtCategory == "ducts")
			{
				salida = ducts;
			}
			if (builtCategory == "pipes")
			{
				salida = pipes;
			}
			if (builtCategory == "conduits")
			{
				salida = conduits;
			}
			if (builtCategory == "cabletrays")
			{
				salida = cabletrays;
			}
			if (builtCategory == "flexducts")
			{
				salida = flexducts;
			}
			if (builtCategory == "flexpipes")
			{
				salida = flexpipes;
			}
			return salida;

		}

	}
}
