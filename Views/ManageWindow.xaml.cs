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
        private readonly UIDocument uidoc;
        private readonly Document doc;
        private readonly Window window;
        private readonly ExternalCommandData _commandData;
        private readonly List<Element> _lista_links;

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
            _lista_links = lista_links;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Element> lista_links_filtered = new List<Element>();

            for (int i = 0; i < datagrid.Items.Count; i++)
            {
                CheckBox mycheckbox = datagrid.Columns[0].GetCellContent(datagrid.Items[i]) as CheckBox;
                if (!(bool)mycheckbox.IsChecked)
                {
                    lista_links_filtered.Add(_lista_links[i]);
                }
            }

            window.Close();

            Intersect.MultipleElementsToLinksElements(_commandData, lista_links_filtered);

            Intersect.MultipleElementsToLinksFamilyInstance(_commandData, lista_links_filtered);

            Intersect.MultipleFamilyInstanceToLinksElements(_commandData, lista_links_filtered);

            Intersect.MultipleFamilyInstanceToLinksFamilyInstance(_commandData, lista_links_filtered);

            SetClashGridLocation.DoActiveView(_commandData);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            window.Close();
        }
    }
}