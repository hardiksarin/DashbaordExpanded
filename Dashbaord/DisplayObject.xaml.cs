using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashbaord
{
    /// <summary>
    /// Interaction logic for DisplayObject.xaml
    /// </summary>
    public partial class DisplayObject : UserControl
    {
        public DisplayObject()
        {
            InitializeComponent();
            listcollection.Clear();
            ListBoxItems.Items.Add("Hey");
            ListBoxItems.Items.Add("hola");
            ListBoxItems.Items.Add("Namaste");
            ListBoxItems.Items.Add("123");
            ListBoxItems.Items.Add("1");
            ListBoxItems.Items.Add("234");
            ListBoxItems.Items.Add("a1v2");
            ListBoxItems.Items.Add("Luke Skywalker");
            ListBoxItems.Items.Add("Thanos");
            ListBoxItems.Items.Add("Tony Stark");
            ListBoxItems.Items.Add("Peter North");
            ListBoxItems.Items.Add("Bazinga");
            foreach (string n in ListBoxItems.Items)
            {
                
                listcollection.Add(n.ToString());
                SearchInputTextBox.CharacterCasing = CharacterCasing.Normal;
            }
        }
        List<String> listcollection = new List<string>();
        private void SearchInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(SearchInputTextBox.Text) == false)
            {
                ListBoxItems.Items.Clear();
                foreach(string str in listcollection)
                {
                    if (str.Contains(SearchInputTextBox.Text))
                    {
                        ListBoxItems.Items.Add(str);
                    }
                    
                }
            }
            else if (SearchInputTextBox.Text == "")
            {
                ListBoxItems.Items.Clear();
                foreach (string str in listcollection)
                {
                    ListBoxItems.Items.Add(str);
                }
            }
        }
    }
}
