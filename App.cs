#region Namespaces
using Autodesk.Revit.UI;
using System;
using System.Windows.Media.Imaging;

#endregion

namespace QuickClash
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    internal class App : IExternalApplication
    {
        public static string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "QuickClash";
            application.CreateRibbonTab(tabName);
            ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com");

            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "QuickClash");



            PushButton button = panel1.AddItem(new PushButtonData("button", "Start Clash", ExecutingAssemblyPath, "QuickClash.StartClash")) as PushButton;
            button.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/architech-working-(1).png"));
            button.ToolTip = "Create all Clash Parameters or restart all Analysis";
            button.LongDescription = "We start the Quick Clash Tool by creating all the Clash Parameters or returning them to their default values if they already exist.";
            button.SetContextualHelp(contextHelp);

            PushButton button2 = panel1.AddItem(new PushButtonData("button2", "Default Values", ExecutingAssemblyPath, "QuickClash.DefaultValues")) as PushButton;
            button2.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/architech-working-(1).png"));
            button2.ToolTip = "Set all the Clash parameter values to Default parameters";
            button2.LongDescription = "Set all the Clash parameter values to Default parameters. Id Element, Clash, Clash Grid Location, Clash Category.";
            button2.SetContextualHelp(contextHelp);

            //PushButton button3 = panel1.AddItem(new PushButtonData("button3", "Section Box", ExecutingAssemblyPath, "QuickClash.SectionBox")) as PushButton;
            //button3.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/3d-(1).png"));
            //button3.ToolTip = "Choose which Section Box to Apply";
            //button3.LongDescription = "We choose between which Categories of Elements we want to do the Analysis of Collisions or Interferences. It is chosen whether in Active View or if in the entire Document. \nDepending on the size of the document or file, if many Categories are chosen for the Analysis, this process may take several minutes.";
            //button3.SetContextualHelp(contextHelp);

            RibbonPanel panel2 = application.CreateRibbonPanel(tabName, "Clash Review");

            PushButton button4 = panel2.AddItem(new PushButtonData("button4", "Quick Clash", ExecutingAssemblyPath, "QuickClash.QuickClash")) as PushButton;
            button4.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/pipes-angles-(1).png"));
            button4.ToolTip = "Quick Clash Colision";
            button4.LongDescription = "Quick Collision Analysis of all Categories against all Categories in Active View.";
            button4.SetContextualHelp(contextHelp);


            PushButton button5 = panel2.AddItem(new PushButtonData("button5", "Quick Clash Links", ExecutingAssemblyPath, "QuickClash.QuickClashLinks")) as PushButton;
            button5.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/pipes-angles-(1).png"));
            button5.ToolTip = "Quick Clash Colision with Links Models";
            button5.LongDescription = "Quick Collision Analysis of all Categories against all Categories of Linked Models in Active View.";
            button5.SetContextualHelp(contextHelp);




            //PushButton button6 = panel2.AddItem(new PushButtonData("button6", "Clash Comments", ExecutingAssemblyPath, "QuickClash.ClashComments")) as PushButton;



            PushButton button7 = panel2.AddItem(new PushButtonData("button7", "Filter Clash", ExecutingAssemblyPath, "QuickClash.ClashFilter")) as PushButton;
            button7.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/broom-(1).png"));
            button7.ToolTip = "Apply Clash Filter";
            button7.LongDescription = "Apply a colour red Clash filter to indentified the elements interferences in Active View.";
            button7.SetContextualHelp(contextHelp);




            //PushButton button8 = panel2.AddItem(new PushButtonData("button8", "Remove Filter Clash", ExecutingAssemblyPath, "QuickClash.RemoveClashFilter")) as PushButton;
            //button8.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/broom-(1).png"));
            //button8.ToolTip = "Unapply Clash Filter";
            //button8.LongDescription = "Unapply the colour red Clash filter in Active View.";
            //button8.SetContextualHelp(contextHelp);




            //PushButton button9 = panel2.AddItem(new PushButtonData("button9", "Quick Clash Links", ExecutingAssemblyPath, "QuickClash.QuickClashLinks")) as PushButton;








            //button6.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/edit-(1).png"));
            //button6.ToolTip = "Comment on the Items pending revision";
            //button6.LongDescription = "The comment will be written in the Elements with CLASH of the Active View.";
            //button6.SetContextualHelp(contextHelp);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
