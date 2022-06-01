using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public ManageWindow(UIDocument uiDocument, Window win)
        {
            InitializeComponent();
            uidoc = uiDocument;
            doc = uidoc.Document;
            window = win;
        }
    }
}
