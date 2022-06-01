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
            string tabName = "Quick Clash";
            application.CreateRibbonTab(tabName);
            ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url, "http://www.weclash.xyz");

            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "Clash Analysis");

            PushButton button = panel1.AddItem(new PushButtonData("button", "Start Clash", ExecutingAssemblyPath, "QuickClash.StartClash")) as PushButton;
            button.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/architech-working-(1).png"));
            button.ToolTip = "Create all Clash Parameters";
            button.LongDescription = "Start the execution of the Addin. What happens when you run it is that all the Clash Parameters are created: Clash, Clash Solved, Element ID, etc; along with a view called COORD to start using the Clash Detection tool on it.";
            button.SetContextualHelp(contextHelp);

            PushButton button2 = panel1.AddItem(new PushButtonData("button2", "Default Values", ExecutingAssemblyPath, "QuickClash.DefaultActiveView")) as PushButton;
            button2.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/architech-working-(1).png"));
            button2.ToolTip = "Set all the Clash parameter values to Default parameters";
            button2.LongDescription = "Set all the Clash parameter values to Default parameters. Id Element, Clash, Clash Grid Location, Clash Category.";
            button2.SetContextualHelp(contextHelp);

            RibbonPanel panel2 = application.CreateRibbonPanel(tabName, "Clash Review");

            PushButton button4 = panel2.AddItem(new PushButtonData("button4", "Quick Clash", ExecutingAssemblyPath, "QuickClash.QuickClash")) as PushButton;
            button4.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/pipes-angles-(1).png"));
            button4.ToolTip = "Quick Clash Colision";
            button4.LongDescription = "Quick Collision Analysis of all Categories against all Categories in Active View.";
            button4.SetContextualHelp(contextHelp);


            PushButton button5 = panel2.AddItem(new PushButtonData("button5", "Quick Clash Links", ExecutingAssemblyPath, "QuickClash.QuickClashLinks")) as PushButton;
            button5.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/pipes-angles-(1).png"));
            button5.ToolTip = "Quick Clash Colision with Links Models";
            button5.LongDescription = "Quick Collision Analysis of all Categories against all Categories of Links Models in Active View.";
            button5.SetContextualHelp(contextHelp);








            PushButton button7 = panel2.AddItem(new PushButtonData("button7", "Filter Clash", ExecutingAssemblyPath, "QuickClash.ClashFilter")) as PushButton;
            button7.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/broom-(1).png"));
            button7.ToolTip = "Apply Clash Filter";
            button7.LongDescription = "Apply a colour red Clash filter to indentified the elements interferences in Active View.";
            button7.SetContextualHelp(contextHelp);


            RibbonPanel panel3 = application.CreateRibbonPanel(tabName, "Create");

            PushButton button3 = panel3.AddItem(new PushButtonData("button3", "Selection Box", ExecutingAssemblyPath, "QuickClash.SectionBoxSelection")) as PushButton;
            button3.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/3d-(1).png"));
            button3.ToolTip = "Copy and Section Box apply";
            button3.LongDescription = "Create a copy of the active 3D view and apply Section Box on Elements selected.";
            button3.SetContextualHelp(contextHelp);


            PushButton button8 = panel3.AddItem(new PushButtonData("button8", "Zones Views", ExecutingAssemblyPath, "QuickClash.ZonesViews")) as PushButton;
            button8.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/3d-(1).png"));
            button8.ToolTip = "";
            button8.LongDescription = "";
            button8.SetContextualHelp(contextHelp);

            PushButton button9 = panel3.AddItem(new PushButtonData("button9", "Elements Views", ExecutingAssemblyPath, "QuickClash.ElementsViews")) as PushButton;
            button9.LargeImage = new BitmapImage(new Uri("pack://application:,,,/QuickClash;component/Resources/3d-(1).png"));
            button9.ToolTip = "";
            button9.LongDescription = "";
            button9.SetContextualHelp(contextHelp);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}