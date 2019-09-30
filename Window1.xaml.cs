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
using System.Windows.Shapes;
using System.Xml;


namespace Lazy_Romeo
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ProfileDetailsWindow : Window
    {
        public ProfileDetailsWindow()
        {
            InitializeComponent();
            WrongProfileTextBox.Text = "";
            ProfilePasswordBox.Password = null;
            LoadProfiles();
            ConfirmButton.Visibility = Visibility.Hidden;
        }

        private void LoadProfiles()
        {

            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile1.xml");

            XmlNode root_node = doc.DocumentElement;
            XmlNodeList nodes = root_node.ChildNodes;
            foreach (XmlNode n in nodes)
                if (n.Name == "PERSON")
                    foreach (XmlNode tmp in n)
                        if (tmp.Name == "NICKNAME")
                            ProfileComboBox.Items.Add(tmp.InnerText);
        }

        private void ProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WrongProfileTextBox.Text = "";
            LnameTextBlock.Text = "Last name: ";
            FnameTextBlock.Text = "First name: ";
            ProfilePasswordBox.Clear();
            ProfilePasswordBox.IsEnabled = true;
            ConfirmButton.Visibility = Visibility.Hidden;
            OpenProfileOKButton.Visibility = Visibility.Visible;

        }

        private void OpenProfileOKButton_Click(object sender, RoutedEventArgs e)
        {
            
            XmlDocument doc = new XmlDocument();
            doc.Load("XMLFile1.xml");

            XmlNode root_node = doc.DocumentElement;
            XmlNodeList nodes = root_node.ChildNodes;

            foreach (XmlNode n in nodes)
            {
                if (n.Name == "PERSON")
                    if ((string)ProfileComboBox.SelectedItem == n.Attributes[0].Value & ProfilePasswordBox.Password == n.Attributes[1].Value)
                    {
                        WrongProfileTextBox.Text = "";
                        foreach (XmlNode tmp in n)
                            switch (tmp.Name)
                            {
                                case "LNAME": { LnameTextBlock.Text = "Last name: " + tmp.InnerText; break; }
                                case "FNAME": { FnameTextBlock.Text = "First name: " + tmp.InnerText; break; }
                            }
                        ProfilePasswordBox.IsEnabled = false;
                        OpenProfileOKButton.Visibility = Visibility.Hidden;
                        ConfirmButton.Visibility = Visibility.Visible;
                        break;
                    }
                    else
                    {
                        WrongProfileTextBox.Text = "<   Wrong password or profile   >";
                        LnameTextBlock.Text = "Last name: ";
                        FnameTextBlock.Text = "First name: ";
                    }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            this.Close();
        }
    }
}
