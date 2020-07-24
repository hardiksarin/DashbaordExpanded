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
    /// Interaction logic for CostCategoryCreation.xaml
    /// </summary>
    public partial class CostCategoryCreation : UserControl
    {
        public CostCategoryCreation()
        {
            InitializeComponent();
        }

        // Validate the login Form
        private bool ValidateLoginForm()
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
            if (RevenueItemCheckBox.IsChecked == false && NonRevenueItemCheckBox.IsChecked == false)
            {
                output = false;
            }
            //if (NonRevenueItemCheckBox.IsChecked == null)
            //{
            //    output = false;
            //}
            return output;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateLoginForm())
            {
                CostCategoryModel model = new CostCategoryModel();
                model.category_name = NameInputTextBox.Text;
                model.category_alias = AliasInputTetxBox.Text;
                if (RevenueItemCheckBox.IsChecked == true)
                {
                    model.revenue = true;
                }
                else
                {
                    model.revenue = false;
                }
                GlobalConfig.Connection.CreateCategory(model);
            }
            else
            {
                MessageBox.Show("Please Fill in the Details Properly!");
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you  want to close?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Environment.Exit(0);
        }
    }
}
