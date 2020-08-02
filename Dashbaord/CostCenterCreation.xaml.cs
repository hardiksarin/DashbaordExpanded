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
    /// Interaction logic for CostCenterCreation.xaml
    /// </summary>
    public partial class CostCenterCreation : UserControl
    {
        private List<CostCategoryModel> availableCategory = new List<CostCategoryModel>();
        private List<CostCenterModel> availableCenter = new List<CostCenterModel>();
        ICreateRequestor callingForm;
        public CostCenterCreation(ICreateRequestor caller)
        {
            InitializeComponent();
            callingForm = caller;
            LoadListData();
            WireUpLists();
        }

        //Load Category And Cost Center Data
        private void LoadListData()
        {
            availableCategory = GlobalConfig.Connection.GetCategory_All();
            availableCenter = GlobalConfig.Connection.GetCostCenter_All();
        }

        //Initialise the combo box to list of groups and cost centers.
        private void WireUpLists()
        {
            CategoryInputComboBox.ItemsSource = null;
            CategoryInputComboBox.ItemsSource = availableCategory;
            CategoryInputComboBox.DisplayMemberPath = "category_name";

            UnderComboBox.ItemsSource = null;
            UnderComboBox.ItemsSource = availableCenter;
            UnderComboBox.DisplayMemberPath = "cc_name";
        }

        // Validate the login Form
        private bool ValidateForm()
        {
            bool output = true;
            if (NameInputTextBox.Text.Length == 0)
            {
                output = false;
            }
            if (AliasInputTetxBox.Text.Length == 0)
            {
                output = false;
            }
            if (CategoryInputComboBox.SelectedItem == null)
            {
                output = false;
            }
            if (UnderComboBox.SelectedItem == null)
            {
                output = false;
            }
            return output;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                CostCenterModel model = new CostCenterModel();
                CostCenterModel selectedModel = (CostCenterModel)UnderComboBox.SelectedItem;
                CostCategoryModel selectedCategory = (CostCategoryModel)CategoryInputComboBox.SelectedItem;
                model.cc_name = NameInputTextBox.Text;
                model.cc_alias = AliasInputTetxBox.Text;
                model.under_category = selectedCategory.category_id;
                model.under_cc = selectedModel.cost_center_id;

                GlobalConfig.Connection.CreateCostCenter(model);
                if (MessageBox.Show("Cost Center Created", "", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    callingForm.Home(2);
                }
            }
            else
            {
                MessageBox.Show("Please Fill in the Details Properly!");
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you  want to close?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                callingForm.Home(2);
        }
    }
}
