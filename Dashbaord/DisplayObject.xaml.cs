using GravitonLibrary;
using GravitonLibrary.Models;
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
        private List<LedgerModel> availableLedgers = new List<LedgerModel>();
        private List<GroupModel> availableGroups = new List<GroupModel>();
        private List<CostCenterModel> availableCostCenters = new List<CostCenterModel>();
        private List<CostCategoryModel> availableCategory = new List<CostCategoryModel>();
        private int index;

        public DisplayObject(int id)
        {
            InitializeComponent();
            index = id;
            LoadListData();
            WireUpLists();
            //listcollection.Clear();
            //ListBoxItems.Items.Add("Hey");
            //ListBoxItems.Items.Add("hola");
            //ListBoxItems.Items.Add("Namaste");
            //ListBoxItems.Items.Add("123");
            //ListBoxItems.Items.Add("1");
            //ListBoxItems.Items.Add("234");
            //ListBoxItems.Items.Add("a1v2");
            //ListBoxItems.Items.Add("Luke Skywalker");
            //ListBoxItems.Items.Add("Thanos");
            //ListBoxItems.Items.Add("Tony Stark");
            //ListBoxItems.Items.Add("Peter North");
            //ListBoxItems.Items.Add("Bazinga");
            /*foreach (string n in ListBoxItems.Items)
            {
                
                listcollection.Add(n.ToString());
                SearchInputTextBox.CharacterCasing = CharacterCasing.Normal;
            }*/
            SearchInputTextBox.CharacterCasing = CharacterCasing.Normal;
        }

        private void LoadListData()
        {
            switch (index)
            {
                case 0:
                    availableLedgers = GlobalConfig.Connection.GetLedger_All();
                    break;
                case 1:
                    availableGroups = GlobalConfig.Connection.GetGroups_All();
                    break;
                case 2:
                    availableCostCenters = GlobalConfig.Connection.GetCostCenter_All();
                    break;
                case 3:
                    availableCategory = GlobalConfig.Connection.GetCategory_All();
                    break;
                default:
                    break;
            }
        }

        private void WireUpLists()
        {
            switch (index)
            {
                case 0:
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableLedgers;
                    ListBoxItems.DisplayMemberPath = "ledger_name";
                    break;
                case 1:
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableGroups;
                    ListBoxItems.DisplayMemberPath = "group_name";
                    break;
                case 2:
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableCostCenters;
                    ListBoxItems.DisplayMemberPath = "cc_name";
                    break;
                case 3:
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableCategory;
                    ListBoxItems.DisplayMemberPath = "category_name";
                    break;
                default:
                    break;
            }
        }
        //List<String> listcollection = new List<string>();
        private void SearchInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (index)
            {
                case 0:
                    if (string.IsNullOrEmpty(SearchInputTextBox.Text) == false)
                    {
                        //ListBoxItems.Items.Clear();
                        ListBoxItems.ItemsSource = null;
                        ListBoxItems.Items.Clear();
                        foreach (LedgerModel str in availableLedgers)
                        {
                            if (str.ledger_name.Contains(SearchInputTextBox.Text))
                            {
                                ListBoxItems.Items.Add(str);
                            }

                        }
                    }
                    else if (SearchInputTextBox.Text == "")
                    {
                        ListBoxItems.Items.Clear();
                        foreach (LedgerModel str in availableLedgers)
                        {
                            WireUpLists();
                            //ListBoxItems.Items.Add(str);
                        }
                    }
                    break;
                case 1:
                    if (string.IsNullOrEmpty(SearchInputTextBox.Text) == false)
                    {
                        ListBoxItems.ItemsSource = null;
                        ListBoxItems.Items.Clear();
                        foreach (GroupModel str in availableGroups)
                        {
                            if (str.group_name.Contains(SearchInputTextBox.Text))
                            {
                                ListBoxItems.Items.Add(str);
                            }

                        }
                    }
                    else if (SearchInputTextBox.Text == "")
                    {
                        ListBoxItems.Items.Clear();
                        foreach (GroupModel str in availableGroups)
                        {
                            WireUpLists();
                            //ListBoxItems.Items.Add(str);
                        }
                    }
                    break;
                case 2:
                    if (string.IsNullOrEmpty(SearchInputTextBox.Text) == false)
                    {
                        ListBoxItems.ItemsSource = null;
                        ListBoxItems.Items.Clear();
                        foreach (CostCenterModel str in availableCostCenters)
                        {
                            if (str.cc_name.Contains(SearchInputTextBox.Text))
                            {
                                ListBoxItems.Items.Add(str);
                            }

                        }
                    }
                    else if (SearchInputTextBox.Text == "")
                    {
                        ListBoxItems.Items.Clear();
                        foreach (CostCenterModel str in availableCostCenters)
                        {
                            WireUpLists();
                            //ListBoxItems.Items.Add(str);
                        }
                    }
                    break;
                case 3:
                    if (string.IsNullOrEmpty(SearchInputTextBox.Text) == false)
                    {
                        ListBoxItems.ItemsSource = null;
                        ListBoxItems.Items.Clear();
                        foreach (CostCategoryModel str in availableCategory)
                        {
                            if (str.category_name.Contains(SearchInputTextBox.Text))
                            {
                                ListBoxItems.Items.Add(str);
                            }

                        }
                    }
                    else if (SearchInputTextBox.Text == "")
                    {
                        ListBoxItems.Items.Clear();
                        foreach (CostCategoryModel str in availableCategory)
                        {
                            WireUpLists();
                            //ListBoxItems.Items.Add(str);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void ListBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
