using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace QuickClash.Create
{
    internal class ClashSchedules
    {
        public static void Create(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            List<BuiltInCategory> bics_familyInstance = GetLists.BuiltCategories(true);
            List<BuiltInCategory> bics = GetLists.BuiltCategories(false);
            List<BuiltInCategory> bics_All = new List<BuiltInCategory>();
            foreach (BuiltInCategory item in bics_familyInstance)
            {
                bics_All.Add(item);
            }
            foreach (BuiltInCategory item in bics)
            {
                bics_All.Add(item);
            }
            foreach (BuiltInCategory bic in bics_All)
            {
                ViewSchedule clashSchedule = null;
                using (Transaction transaction = new Transaction(doc, "Creating CLASH Schedule"))
                {
                    _ = transaction.Start();
                    clashSchedule = ViewSchedule.CreateSchedule(doc, new ElementId(bic));
                    doc.Regenerate();
                    ScheduleDefinition definition = clashSchedule.Definition;
                    IList<SchedulableField> schedulableFields = definition.GetSchedulableFields();
                    clashSchedule.Name = "CLASH " + bic.ToString() + " SCHEDULE";
                    List<SchedulableField> listashparam = new List<SchedulableField>();
                    List<ScheduleFieldId> fieldIds = new List<ScheduleFieldId>();
                    List<string> verified_Param = new List<string>();

                    foreach (SchedulableField element in schedulableFields)
                    {
                        if (element.ParameterId.IntegerValue > 0)
                        {
                            listashparam.Add(element);
                        }
                    }

                    List<string> list_params = new List<string>()
                    {
                        "Clash", "Clash Category", "Clash Comments", "Clash Grid Location", "Clash Solved", "Done", "ID Element", "Done", "Percent Done", "Zone"
                    };

                    foreach (SchedulableField item in listashparam)
                    {
                        foreach (string param in list_params)
                        {
                            if (item.GetName(doc).ToString() == param)
                            {
                                if (!verified_Param.Contains(param))
                                {
                                    ScheduleField scheduleField = clashSchedule.Definition.AddField(item);
                                    fieldIds.Add(scheduleField.FieldId);
                                    verified_Param.Add(param);
                                }
                            }
                        }
                    }
                    ScheduleField foundField = clashSchedule.Definition.GetField(fieldIds.FirstOrDefault());
                    if (null != clashSchedule)
                    {
                        _ = transaction.Commit();
                    }
                    else
                    {
                        _ = transaction.RollBack();
                    }
                    using (Transaction ta = new Transaction(doc, "Add filter"))
                    {
                        _ = ta.Start();
                        ScheduleFilter filter = new ScheduleFilter(foundField.FieldId, ScheduleFilterType.Contains, "YES");
                        clashSchedule.Definition.AddFilter(filter);
                        _ = ta.Commit();
                    }
                }
            }
        }
    }
}