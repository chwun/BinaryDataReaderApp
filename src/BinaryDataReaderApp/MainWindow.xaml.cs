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
using BinaryDataReader.Lib;

namespace BinaryDataReaderApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadTemplate();
        }

        private void ReadTemplate()
        {
            BinaryDataTemplate template = new BinaryDataTemplate("Test-Template", new BinaryDataTemplateXMLProvider(@"C:\tmp\Templates BinaryDataReaderApp\OEEEventsFileTemplate_mitTSF.xml"));
        }
    }
}
