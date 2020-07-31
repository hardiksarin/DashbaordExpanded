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

// TODO: Create a New Item Button 
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
        ICreateRequestor callingForm;
        public DisplayObject(ICreateRequestor caller,int id)
        {
            InitializeComponent();
            callingForm = caller;
            index = id;
            LoadListData();
            WireUpLists();
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
                case 0:                                                 //Ledger
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableLedgers;
                    ListBoxItems.DisplayMemberPath = "ledger_name";
                    break;
                case 1:                                                 //Groups
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableGroups;
                    ListBoxItems.DisplayMemberPath = "group_name";
                    break;
                case 2:                                                 //Cost Center
                    ListBoxItems.ItemsSource = null;

                    ListBoxItems.ItemsSource = availableCostCenters;
                    ListBoxItems.DisplayMemberPath = "cc_name";
                    break;
                case 3:                                                 //Cost Category
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
            switch (index)
            {
                case 0:
                    LedgerModel model = (LedgerModel)ListBoxItems.SelectedItem;
                    /*MainWindow window = new MainWindow();
                    window.GridPrincipal.Children.Clear();
                    window.GridPrincipal.Children.Add(new DisplayLedgers(model));*/
                    callingForm.LedgerClicked(model);
                    break;
                case 1:
                    GroupModel groupModel = (GroupModel)ListBoxItems.SelectedItem;
                    callingForm.GroupClicked(groupModel);
                    break;
                case 2:
                    CostCenterModel costCenterModel = (CostCenterModel)ListBoxItems.SelectedItem;
                    callingForm.CostCenterClicked(costCenterModel);
                    break;
                case 3:
                    CostCategoryModel categoryModel = (CostCategoryModel)ListBoxItems.SelectedItem;
                    callingForm.CategoryClicked(categoryModel);
                    break;
                default:
                    break;
            }
        }
    }
}
