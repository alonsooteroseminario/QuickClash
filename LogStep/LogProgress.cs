using System;
using System.IO;

namespace QuickClash
{
    public class LogProgress
    {
        /// <summary>
        /// Write in the output file the time of the process
        /// that is carried out in the execution.
        /// </summary>
        /// <returns></returns>
        public static void UpDate(string messageProcess)
        {
            //DateTime now = DateTime.Now;

            //string version = SetVersion();

            //string path = "\\C:\\ProgramData\\Autodesk\\Revit\\Addins\\" + version + "\\LogProgress.txt";

            //using (StreamWriter sw = File.CreateText(path))
            //{
            //    sw.WriteLine(messageProcess + " " + now.ToLongTimeString());
            //}
        }

        public static string SetVersion()
        {
            #if REVIT2019
            string version = "2019";
            #endif
            #if REVIT2020
            string version = "2020";
            #endif
            #if REVIT2021
            string version = "2021";
            #endif
            #if REVIT2022
            string version = "2022";
            #endif
            return version;
        }
    }
}