using GravitonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dashbaord
{
    /// <summary>
    /// Interaction logic for BillWisePopUp.xaml
    /// </summary>
    public partial class BillWisePopUp : Window
    {
        //private List<PaymentBill> paymentBills = new List<PaymentBill>();
        IBillRequestor callingForm;
        public BillWisePopUp(IBillRequestor caller,List<PaymentBill> bill)
        {
            //paymentBills = bill;
            InitializeComponent();
            callingForm = caller;
            BillwiseDataGrid.Items.Clear();
            foreach (PaymentBill b in bill)
            {
                BillwiseDataGrid.Items.Add(b);
            }

        }

        private void BillwiseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PaymentBill p = (PaymentBill)BillwiseDataGrid.SelectedItem;
            callingForm.BillComplete(p);
            this.Close();
        }
    }
}
