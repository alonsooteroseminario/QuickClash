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

            List<BuiltInCategory> bics = GetLists.BuiltCategories(true);

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
                    clashSchedule.Name = "CLASH " + bic.ToString() + " SCHEDULE";


                    List<SchedulableField> listashparam = new List<SchedulableField>();
                    List<ScheduleFieldId> fieldIds = new List<ScheduleFieldId>();

                    foreach (SchedulableField element in schedulableFields)
                    {
                        if (element.ParameterId.IntegerValue > 0)
                        {
                            listashparam.Add(element);
                        }
                    }

                    double nro_items_listahpram = listashparam.Count();

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
                                ScheduleField scheduleField = clashSchedule.Definition.AddField(item);
                                fieldIds.Add(scheduleField.FieldId);
                            }
                        }
                    }

                    // "Clash" parameter equal "YES"
                    ScheduleField foundField = clashSchedule.Definition.GetField(fieldIds.FirstOrDefault());

                    if (null != clashSchedule)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.RollBack();
                    }

                    using (Transaction ta = new Transaction(doc, "Add filter"))
                    {
                        ta.Start();
                        ScheduleFilter filter = new ScheduleFilter(foundField.FieldId, ScheduleFilterType.Contains, "YES");
                        clashSchedule.Definition.AddFilter(filter);
                        ta.Commit();
                    }

                }
            }
        }
    }
}
