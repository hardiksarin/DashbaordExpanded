﻿using GravitonLibrary;
using GravitonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for PaymentVoucher.xaml
    /// </summary>
    public partial class PaymentVoucher : UserControl
    {
        private List<LedgerModel> availableLedgers = new List<LedgerModel>();
        private List<BillModel> availableBills = new List<BillModel>();
        private List<PaymentBill> paymentBills = new List<PaymentBill>();
        private List<VoucherModel> vouchers = new List<VoucherModel>();
        List<string> credDeb = new List<string> { "Cr", "Dr" };
        int count = 0;
        int AccountId = 0;
        int ParticularId = 0;
        double AccountBalance = 0;
        double ParticularBalance = 0;
        public PaymentVoucher()
        {
            InitializeComponent();
            LoadListData();
            WireUpVoucherForm();
            WireUpLists();
        }


        private void LoadListData()
        {
            availableLedgers = GlobalConfig.Connection.GetLedger_All();
            vouchers = GlobalConfig.Connection.GetVoucher_Payment();
        }

        private void WireUpLists()
        {
            AccountLedgerCombobox.ItemsSource = null;

            AccountLedgerCombobox.ItemsSource = availableLedgers;
            AccountLedgerCombobox.DisplayMemberPath = "ledger_name";

            ParticularLedgerCombobox.ItemsSource = null;

            ParticularLedgerCombobox.ItemsSource = availableLedgers;
            ParticularLedgerCombobox.DisplayMemberPath = "ledger_name";

            balanceComboBox.ItemsSource = null;
            balanceComboBox.ItemsSource = credDeb;
        }

        private void WireUpVoucherForm()
        {
            if (vouchers != null)
            {
                int length = vouchers.Count;
                PaymentVoucherNumberLabel.Text = $"No. {length + 1}";
            }
            else
            {
                PaymentVoucherNumberLabel.Text = "No. 1";
            }
        }

        private void AccountLedgerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LedgerModel model = (LedgerModel)AccountLedgerCombobox.SelectedItem;
            AccountCurrentBalanceDataValue.Text = model.current_bal.ToString();
            AccountBalance = model.current_bal;
            AccountId = model.lid;

        }

        private void ParticularLedgerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LedgerModel model = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
            ParticularCurrentBalanceDataValue.Text = model.current_bal.ToString();
            ParticularBalance = model.current_bal;
            ParticularId = model.lid;
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ValidateForm())
            {
                BillwiseDataGrid.Items.Clear();
                paymentBills.Clear();

                int amount = int.Parse(AmountTextbox.Text);
                //DateTime? n = StartDatePicker.SelectedDate;
                int num = int.Parse(NumberOfEmiTextbox.Text);
                string date = StartDatePicker.SelectedDate.ToString().Split(' ').First();
                for (int i = 0; i < num; i++)
                {
                    PaymentBill p = new PaymentBill();
                    p.reference = "New";
                    p.emi = $"E0{i + 1}";
                    p.due_date = i == 0 ? date : NextMonth(date);
                    p.amount = $"{(double)amount / num}";
                    date = p.due_date;
                    paymentBills.Add(p);

                    BillwiseDataGrid.Items.Add(p);
                }
            }
            else
            {
                MessageBox.Show("Please Fill in the Details Properly!");
            }
        }

        private string NextMonth(string date)
        {
            string nextDate = date;
            string[] dateList = nextDate.Split('-');
            int month = int.Parse(dateList[1]);
            if(month < 12)
            {
                if(month < 9)
                {
                    nextDate = $"{dateList[0]}-0{month + 1}-{dateList[2]}";
                }
                else
                {
                    nextDate = $"{dateList[0]}-{month + 1}-{dateList[2]}";
                }
            }else if(month == 12)
            {
                int year = int.Parse(dateList[2]);
                nextDate = $"{dateList[0]}-0{1}-{year + 1}";
            }
            return nextDate;
        }

        private bool ValidateForm()
        {
            bool output = true;
            if(AccountLedgerCombobox.SelectedItem == null)
            {
                output = false;
            }
            if (ParticularLedgerCombobox.SelectedItem == null)
            {
                output = false;
            }
            if(AmountTextbox.Text.Length == 0)
            {
                output = false;
            }
            if (NumberOfEmiTextbox.Text.Length == 0)
            {
                output = false;
            }
            if(DatePicker.SelectedDate == null)
            {
                output = false;
            }
            return output;
        }

        /*private void StartDatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                
            }
        }*/

        private double GetBalance(LedgerModel model)
        {
            double current = 0;
            if(model.credit_bal > model.debit_bal)
            {
                current = model.credit_bal - model.debit_bal;
            }
            else if(model.credit_bal < model.debit_bal)
            {
                current = model.debit_bal - model.credit_bal;
            }
            else
            {
                current = 0;
            }
            return current;
        }

        private void WireUpDatabase()
        {
            VoucherModel voucherModel = new VoucherModel();
            ParticularModel particularModel = new ParticularModel();
            //Balance Logic

            string balType = (string)balanceComboBox.SelectedItem;
            if(balType == credDeb[0])
            {
                double amnt = double.Parse(AmountTextbox.Text);
                LedgerModel accountModel = (LedgerModel)AccountLedgerCombobox.SelectedItem;
                LedgerModel particular = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
                //TODO Ledger Balance

                //update Particular Ledger
                particular.credit_bal = particular.credit_bal + amnt;
                particular.current_bal = GetBalance(particular);
                GlobalConfig.Connection.UpdateLedger(particular);

                //update Account Ledger
                accountModel.debit_bal = accountModel.debit_bal + amnt;
                accountModel.current_bal = GetBalance(accountModel);
                GlobalConfig.Connection.UpdateLedger(accountModel);
                
            }else if(balType == credDeb[1])
            {
                double amnt = double.Parse(AmountTextbox.Text);
                LedgerModel accountModel = (LedgerModel)AccountLedgerCombobox.SelectedItem;
                LedgerModel particular = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
                //TODO Ledger Balance

                //update Particular Ledger
                particular.debit_bal = particular.debit_bal + amnt;
                particular.current_bal = GetBalance(particular);
                GlobalConfig.Connection.UpdateLedger(particular);

                //update Account Ledger
                accountModel.credit_bal = accountModel.credit_bal + amnt;
                accountModel.current_bal = GetBalance(accountModel);
                GlobalConfig.Connection.UpdateLedger(accountModel);
            }

            //Voucher Model
            string d = DatePicker.SelectedDate.ToString().Split(' ').First();
            string[] dateList = d.Split('-');
            voucherModel.v_date = $"{dateList[2]}-{dateList[1]}-{dateList[0]}";
            voucherModel.v_number = vouchers[vouchers.Count - 1].vid + 1; 
            voucherModel.vtype = "Payment";
            voucherModel.account = AccountId;

            //Particular Model
            particularModel.particular_amount = double.Parse(AmountTextbox.Text);
            particularModel.particular_name = ParticularId;

            voucherModel.Particular = particularModel;
            //Create Voucher And Particular Model.
            voucherModel = GlobalConfig.Connection.CreateVoucher(voucherModel);

            //Bills
            foreach (PaymentBill bill in paymentBills)
            {
                BillModel billModel = new BillModel();
                billModel.bill_name = bill.emi;
                string[] list = bill.due_date.Split('-');
                billModel.bill_due_date = $"{list[2]}-{list[1]}-{list[0]}";
                billModel.bill_amount = double.Parse(bill.amount);
                billModel.lid = ParticularId;
                billModel.pid = voucherModel.Particular.pid;
                billModel.bill_done = false;
                billModel.vid = voucherModel.vid;

                availableBills.Add(billModel);
            }

            foreach (BillModel bill in availableBills)
            {
                GlobalConfig.Connection.CreateBill(bill);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (count < 2)
            {
                if (MessageBox.Show("Do you  want to save ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    WireUpDatabase();
                    MessageBox.Show("Payment Voucher Created"); 
                }
                count++;
            }
        }

        private void UpdateBalance()
        {
            LedgerModel accountModel = (LedgerModel)AccountLedgerCombobox.SelectedItem;
            LedgerModel particular = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
            string balType = (string)balanceComboBox.SelectedItem;
            double amnt = double.Parse(AmountTextbox.Text);
            if (balType == credDeb[0])
            {
                //update Particular Ledger
                particular.credit_bal = particular.credit_bal + amnt;
                particular.current_bal = GetBalance(particular);
                ParticularCurrentBalanceDataValue.Text = particular.current_bal.ToString();

                //update Account Ledger
                accountModel.debit_bal = accountModel.debit_bal + amnt;
                accountModel.current_bal = GetBalance(accountModel);
                AccountCurrentBalanceDataValue.Text = accountModel.current_bal.ToString();

            }
            else if (balType == credDeb[1])
            {
                //update Particular Ledger
                particular.debit_bal = particular.debit_bal + amnt;
                particular.current_bal = GetBalance(particular);
                ParticularCurrentBalanceDataValue.Text = particular.current_bal.ToString();

                //update Account Ledger
                accountModel.credit_bal = accountModel.credit_bal + amnt;
                accountModel.current_bal = GetBalance(accountModel);
                AccountCurrentBalanceDataValue.Text = accountModel.current_bal.ToString();
            }
        }

        private void balanceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AmountTextbox.Text.Length != 0)
            {
                UpdateBalance();
            }
        }

        private void AmountTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (AmountTextbox.Text.Length != 0)
                {
                    UpdateBalance();
                } 
            }
        }
    }
}
