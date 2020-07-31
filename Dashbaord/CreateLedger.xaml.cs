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
    /// Interaction logic for CreateLedger.xaml
    /// </summary>
    public partial class CreateLedger : UserControl
    {
        List<GroupModel> availableGroups = new List<GroupModel>();
        List<string> stateList = new List<string>() { "Andhra Pradesh", "Arunachal Pradesh", "Assam", "Bihar", "Chhattisgarh", "Goa", "Gujarat", "Haryana", "Himachal Pradesh", "Jammu and Kashmir", "Jharkhand", "Karnataka", "Kerala", "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland", "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Telangana", "Tripura", "Uttarakhand", "Uttar Pradesh", "West Bengal", "Andaman and Nicobar Islands", "Chandigarh", "Dadra and Nagar Haveli", "Daman and Diu", "Delhi", "Lakshadweep", "Puducherry" };
        public CreateLedger()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            string date = now.Day.ToString();
            string month = now.Month.ToString();
            string year = now.Year.ToString();
            CurrentDate.Text = $"Date : {date}-{month}-{year}";
            LoadListData();
            WireUpLists();
        }

        private void LoadListData()
        {
            availableGroups = GlobalConfig.Connection.GetGroups_All();
        }

        private void WireUpLists()
        {
            UnderGroupDropDown.ItemsSource = null;

            UnderGroupDropDown.ItemsSource = availableGroups;
            UnderGroupDropDown.DisplayMemberPath = "group_name";

            StateValue.ItemsSource = null;
            StateValue.ItemsSource = stateList;
        }

        //Validates the Ledger Form
        private bool ValidateForm()
        {
            bool output = true;
            if (LedgerNameValue.Text.Length == 0)
            {
                output = false;
            }
            if (LedgerAliasValue.Text.Length == 0)
            {
                output = false;
            }
            if (MDNameValue.Text.Length == 0)
            {
                output = false;
            }
            if (MDAddressValue.Text.Length == 0)
            {
                output = false;
            }
            if (MDPincodeValue.Text.Length == 0)
            {
                output = false;
            }
            if (UnderGroupDropDown.SelectedItem == null)
            {
                output = false;
            }
            if (StateValue.SelectedItem == null)
            {
                output = false;
            }
            return output;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                LedgerModel model = new LedgerModel();
                MailingDetailsModel mailingModel = new MailingDetailsModel();
                GroupModel selectedGroup = (GroupModel)UnderGroupDropDown.SelectedItem;
                model.ledger_name = LedgerNameValue.Text;
                model.ledger_alias = LedgerAliasValue.Text;
                model.ledger_opening_balance = 1562.0;
                model.under_group = selectedGroup.group_id;
                if (BillBasedAccouting.IsChecked == true)
                {
                    model.bill_based_accounting = true;
                }
                else
                {
                    model.bill_based_accounting = false;
                }
                if (CostCentersApplicable.IsChecked == true)
                {
                    model.cost_centers_applicable = true;
                }
                else
                {
                    model.cost_centers_applicable = false;
                }
                if (EnableIntrestCalculations.IsChecked == true)
                {
                    model.enable_interest_calculations = true;
                }
                else
                {
                    model.enable_interest_calculations = false;
                }
                mailingModel.md_name = MDNameValue.Text;
                mailingModel.md_address = MDAddressValue.Text;
                mailingModel.md_city = MDCityValue.Text;
                mailingModel.md_state = (string)StateValue.SelectedItem;
                mailingModel.md_country = "India";
                mailingModel.md_pincode = MDPincodeValue.Text;

                model.mailingModel = mailingModel;
                GlobalConfig.Connection.CreateLedger(model);
            }
            else
            {
                MessageBox.Show("Please Fill in the Details Properly!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you  want to close?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Environment.Exit(0);
        }
    }
}
