using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QuickClash.Views
{
    /// <summary>
    /// Interaction logic for ManageWindow.xaml
    /// </summary>
    public partial class ManageWindow : UserControl
    {
        private UIDocument uidoc;
        private Document doc;
        private Window window;
        ExternalCommandData _commandData;

        public ManageWindow(ExternalCommandData commandData, UIDocument uiDocument, Window win)
        {
            InitializeComponent();
            uidoc = uiDocument;
            doc = uidoc.Document;
            window = win;
            _commandData = commandData;

            IList<Element> linkInstances = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)).ToElements();

            List<Element> lista_links = new List<Element>();

            foreach (RevitLinkInstance link in linkInstances)
            {
                lista_links.Add(link);
            }
            datagrid.ItemsSource = lista_links;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Intersect.MultipleElementsToLinksElements(_commandData);

            Intersect.MultipleElementsToLinksFamilyInstance(_commandData);

            Intersect.MultipleFamilyInstanceToLinksElements(_commandData);

            Intersect.MultipleFamilyInstanceToLinksFamilyInstance(_commandData);

            SetClashGridLocation.DoActiveView(_commandData);
        }

    }
}