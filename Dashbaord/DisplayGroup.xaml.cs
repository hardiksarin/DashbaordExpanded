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
    /// Interaction logic for DisplayGroup.xaml
    /// </summary>
    public partial class DisplayGroup : UserControl
    {
        private List<GroupModel> availableGroups = new List<GroupModel>();
        GroupModel model;
        ICreateRequestor callingForm;
        public DisplayGroup(ICreateRequestor caller ,GroupModel groupModel)
        {
            InitializeComponent();
            callingForm = caller;
            model = groupModel;
            LoadListData();
            WireUpLists();
            WireUpForm();
        }

        //Load Group Data
        private void LoadListData()
        {
            availableGroups = GlobalConfig.Connection.GetGroups_All();
        }

        //Initialise the combo box to list of groups.
        private void WireUpLists()
        {
            UnderComboBox.ItemsSource = null;

            UnderComboBox.ItemsSource = availableGroups;
            UnderComboBox.DisplayMemberPath = "group_name";
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
            if (UnderComboBox.SelectedItem == null)
            {
                output = false;
            }
            return output;
        }

        private void WireUpForm()
        {
            foreach (GroupModel grp in availableGroups)
            {
                if(grp.group_id == model.under_group)
                {
                    UnderComboBox.Text = grp.group_name;
                }
            }
            NameInputTextBox.Text = model.group_name;
            AliasInputTetxBox.Text = model.group_alias;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                GroupModel groupModel = new GroupModel();
                GroupModel selectedGroup = (GroupModel)UnderComboBox.SelectedItem;
                groupModel.group_id = model.group_id;
                groupModel.group_name = NameInputTextBox.Text;
                groupModel.group_alias = AliasInputTetxBox.Text;
                groupModel.under_group = selectedGroup.group_id;

                GlobalConfig.Connection.UpdateGroups(groupModel);
                if (MessageBox.Show("Group Updated", "", MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    callingForm.Home(1);
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
                callingForm.Home(1);
        }
    }
}
