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
        public PrepareModelInterface(Document _doc, IList<Element> elements)
        {
            InitializeComponent();

            _elementList = elements;

            //Check for initial values
            
            _modelDate = GetValueOrDefault("BIM42_Date",DateTime.Now.ToShortDateString());
            modelDate.Text = _modelDate;

            _modelName = GetValueOrDefault("BIM42_File",Path.GetFileName(_doc.PathName));
            modelName.Text = _modelName;

            _modelIndice = GetValueOrDefault("BIM42_Version","0");
            modelIndice.Text = _modelIndice;

            _modelLot = GetValueOrDefault("BIM42_Discipline", "Architecture");
            modelLot.Text = _modelLot;
        }
        private IList<Element> _elementList;
        public IList<Element> ElementList
        {
            get { return _elementList; }
            set { _elementList = value; }
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

        private bool _overrideValues;
        public bool OverrideValues
        {
            get { return _overrideValues; }
        }

        private bool _inView;
        public bool InView
        {
            get { return _inView; }
        }

        private string GetValueOrDefault(string parameterName, string defaultValue)
        {
            //Check if the param exist
            Element firstOne = _elementList.FirstOrDefault();
            if (firstOne.GetParameters(parameterName).Count != 0)
            {
                foreach (Element e in _elementList)
                {
                    Parameter param = e.GetParameters(parameterName).FirstOrDefault();
                    if (param.AsString() != null || param.AsString() != "")
                    {
                        return param.AsString();
                    }
                }

                return defaultValue;
            }
            else
            {
                return defaultValue;
            }



            
        }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            _modelName = modelName.Text;
            _modelIndice = modelIndice.Text;
            _modelLot = modelLot.Text;
            _modelDate = modelDate.Text;
            _inView = (bool)elementInview.IsChecked;
            _overrideValues = (bool)overrideValues.IsChecked;

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

