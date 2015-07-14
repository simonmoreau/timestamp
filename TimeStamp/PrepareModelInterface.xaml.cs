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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;

namespace TimeStamp
{
    /// <summary>
    /// Interaction logic for PrepareModelInterface.xaml
    /// </summary>
    public partial class PrepareModelInterface : Window
    {
        public PrepareModelInterface(Document _doc)
        {
            InitializeComponent();

            _modelName = Path.GetFileName(_doc.PathName);
            modelName.Text = _modelName;
            _modelIndice = "0";
            modelIndice.Text = _modelIndice;
            _modelLot = "Lot";
            modelLot.Text = _modelLot;
            _modelDate = DateTime.Now.ToShortDateString();
            modelDate.Text = _modelDate;
        }

        private string _modelName;
        public string ModelName
        {
            get { return _modelName; }
        }

        private string _modelIndice;
        public string ModelIndice
        {
            get { return _modelIndice; }
        }

        private string _modelLot;
        public string ModelLot
        {
            get { return _modelLot; }
        }

        private string _modelDate;
        public string Modeldate
        {
            get { return _modelDate; }
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            _modelName = modelName.Text;
            _modelIndice = modelIndice.Text;
            _modelLot = modelLot.Text;
            _modelDate = modelDate.Text;

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;
            this.Close();
        }
    }
}

