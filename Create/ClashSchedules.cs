using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash.Create
{
    class ClashSchedules
    {
        public static void Create(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            List<BuiltInCategory> bics = Lists.BuiltCategories(true);

            string msg = "";
            string msg3 = "";

            foreach (BuiltInCategory bic in bics)
            {
                ViewSchedule clashSchedule = null;
                using (Transaction transaction = new Transaction(doc, "Creating CLASH Schedule"))
                {
                    transaction.Start();
                    clashSchedule = Autodesk.Revit.DB.ViewSchedule.CreateSchedule(doc, new ElementId(bic));
                    doc.Regenerate();
                    ScheduleDefinition definition = clashSchedule.Definition;
                    IList<SchedulableField> schedulableFields = definition.GetSchedulableFields(); // [a,b,c,s,d,f,....]
                    List<SchedulableField> listashparam = new List<SchedulableField>();
                    List<ScheduleFieldId> clashId = new List<ScheduleFieldId>();

                    foreach (SchedulableField element in schedulableFields)
                    {
                        if (element.ParameterId.IntegerValue > 0)
                        {
                            listashparam.Add(element);
                        }
                    }

                    double nro_items_listahpram = listashparam.Count();

                    //for (int i = 0; i < 1; i++)
                    //{
                    //	if (listashparam[i].GetName(doc).ToString() == "Clash")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i]);
                    //		ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId);
                    //	}
                    //	if (listashparam[i + 1].GetName(doc).ToString() == "Clash Category")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 1]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId2 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId2);
                    //	}
                    //	if (listashparam[i + 2].GetName(doc).ToString() == "Clash Comments")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 2]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId3 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId3);
                    //	}
                    //	if (listashparam[i + 3].GetName(doc).ToString() == "Clash Grid Location")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 3]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId3 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId3);
                    //	}
                    //	if (listashparam[i + 4].GetName(doc).ToString() == "Clash Solved")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 4]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId4 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId4);
                    //	}
                    //	if (listashparam[i + 5].GetName(doc).ToString() == "Done")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 5]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId5 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId5);
                    //	}
                    //	if (listashparam[i + 6].GetName(doc).ToString() == "ID Element")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 6]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId6 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId6);
                    //	}
                    //	if (listashparam[i + 7].GetName(doc).ToString() == "Percent Done")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 7]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId7 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId7);
                    //	}
                    //	if (listashparam[i + 8].GetName(doc).ToString() == "Zone")
                    //	{
                    //		clashSchedule.Definition.AddField(listashparam[i + 8]);
                    //		//ScheduleField field = clashSchedule.Definition.GetField(0);
                    //		ScheduleFieldId fielId8 = clashSchedule.Definition.GetFieldId(0);
                    //		clashId.Add(fielId8);
                    //	}
                    //	msg3 = listashparam[i].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 1].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 2].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 3].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 4].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 5].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 6].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 7].GetName(doc).ToString() + Environment.NewLine
                    //		+ listashparam[i + 8].GetName(doc).ToString() + Environment.NewLine;
                    //}


                    //ScheduleField foundField = clashSchedule.Definition.GetField(clashId.FirstOrDefault());

                    if (null != clashSchedule)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.RollBack();
                    }

                    //using (Transaction t = new Transaction(doc, "Add filter"))
                    //{
                    //	t.Start();
                    //	ScheduleFilter filter = new ScheduleFilter(foundField.FieldId, ScheduleFilterType.Equal, "YES");
                    //	clashSchedule.Definition.AddFilter(filter);
                    //	t.Commit();
                    //}
                    //using (Transaction tran = new Transaction(doc, "Cambiar nombre"))
                    //{
                    //	tran.Start();
                    //	TableData td = clashSchedule.GetTableData(); // get viewschedule table data
                    //	TableSectionData tsd = td.GetSectionData(SectionType.Header); // get header section data
                    //	string text = tsd.GetCellText(0, 0);
                    //	tsd.SetCellText(0, 0, "CLASH " + bic.ToString() + " SCHEDULE");
                    //	clashSchedule.Name = "CLASH " + bic.ToString() + " SCHEDULE";
                    //	tsd.InsertColumn(0);
                    //	tran.Commit();
                    //}
                }
            }
            TaskDialog.Show("Creation CLASH Parameters", msg + "Se crearon los siguientes Clash Parameters: \n\n" + msg3 + Environment.NewLine);

        }
    }
}
