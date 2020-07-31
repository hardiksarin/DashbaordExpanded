using GravitonLibrary;
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
    /// Interaction logic for ReceiptVoucher.xaml
    /// </summary>
    public partial class ReceiptVoucher : UserControl,IBillRequestor
    {
        private List<LedgerModel> availableLedgers = new List<LedgerModel>();
        private List<BillModel> availableBills = new List<BillModel>();
        private List<PaymentBill> paymentBills = new List<PaymentBill>();
        private List<VoucherModel> vouchers = new List<VoucherModel>();
        private List<PaymentBill> recieptBills = new List<PaymentBill>();
        PaymentBill currentBill = new PaymentBill();
        private List<PaymentBill> transactionBill = new List<PaymentBill>();
        private List<TransactionModel> newTransactions = new List<TransactionModel>();
        int AccountId = 0;
        int ParticularId = 0;
        public ReceiptVoucher()
        {
            InitializeComponent();
            LoadListData();
            WireUpVoucherForm();
            WireUpLists();
        }

        private void LoadListData()
        {
            availableLedgers = GlobalConfig.Connection.GetLedger_All();
            vouchers = GlobalConfig.Connection.GetVoucher_Reciept();
        }

        private void WireUpLists()
        {
            AccountLedgerCombobox.ItemsSource = null;

            AccountLedgerCombobox.ItemsSource = availableLedgers;
            AccountLedgerCombobox.DisplayMemberPath = "ledger_name";

            ParticularLedgerCombobox.ItemsSource = null;

            ParticularLedgerCombobox.ItemsSource = availableLedgers;
            ParticularLedgerCombobox.DisplayMemberPath = "ledger_name";
        }

        private void WireUpVoucherForm()
        {
            if (vouchers != null)
            {
                int length = vouchers.Count;
                RecieptVoucherNumberLabel.Text = $"No. {length + 1 }";
            }
            else
            {
                RecieptVoucherNumberLabel.Text = "No. 1";
            }
        }

        private void AccountLedgerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LedgerModel model = (LedgerModel)AccountLedgerCombobox.SelectedItem;
            AccountCurrentBalanceDataValue.Text = model.ledger_opening_balance.ToString();
            AccountId = model.lid;
        }

        private void ParticularLedgerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paymentBills.Clear();
            LedgerModel model = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
            ParticularCurrentBalanceDataValue.Text = model.ledger_opening_balance.ToString();
            ParticularId = model.lid;

            availableBills = GlobalConfig.Connection.GetBill_ById(model);
            foreach (BillModel bill in availableBills)
            {
                PaymentBill pb = new PaymentBill();
                pb.reference = "Against";
                pb.emi = bill.bill_name;
                pb.due_date = bill.bill_due_date.Split(' ').First();
                pb.amount = bill.bill_amount.ToString();

                paymentBills.Add(pb);
            }
        }

        private void AmountTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                double amount = double.Parse(AmountTextbox.Text);
                if (ValidateForm() && amount != 0)
                {
                    recieptBills = paymentBills;
                    BillWisePopUp form = new BillWisePopUp(this, recieptBills);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Please Fill in the Details Properly!");
                }
            }
        }

        public void BillComplete(PaymentBill model)
        {
            double amount = double.Parse(AmountTextbox.Text);
            double billAmount = double.Parse(model.amount);
            if (amount < billAmount)
            {
                //model.amount = amount.ToString();
                //currentBill = model;
                currentBill.emi = model.emi;
                currentBill.due_date = model.due_date;
                currentBill.reference = model.reference;
                currentBill.amount = amount.ToString();
                BillwiseDataGrid.Items.Add(currentBill);
                transactionBill.Add(currentBill);
                model.amount = (billAmount - amount).ToString();
                AmountTextbox.Text = "0";
            }
            else if(amount == billAmount)
            {
                currentBill = model;
                BillwiseDataGrid.Items.Add(currentBill);
                transactionBill.Add(currentBill);
                paymentBills.Remove(model);
                AmountTextbox.Text = (amount - billAmount).ToString();
            }
            else if(amount > billAmount)
            {
                currentBill = model;
                BillwiseDataGrid.Items.Add(currentBill);
                transactionBill.Add(currentBill);
                paymentBills.Remove(model);
                AmountTextbox.Text = (amount - billAmount).ToString();
            }
            
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

            if(DatePicker.SelectedDate == null)
            {
                output = false;
            }
            return output;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            BillWisePopUp form = new BillWisePopUp(this, paymentBills);
            form.Show();
        }

        private void DataGridRow_KeyUp(object sender, KeyEventArgs e)
        {
            newTransactions.Clear();
            foreach (BillModel bill in availableBills)
            {
                foreach (PaymentBill b in transactionBill)
                {
                    if (b.emi == bill.bill_name)
                    {
                        TransactionModel transactionModel = new TransactionModel();
                        transactionModel.transaction_date = b.due_date;
                        transactionModel.transaction_amount = double.Parse(b.amount);
                        transactionModel.bill_id = bill.bill_id;
                        newTransactions.Add(transactionModel);
                        bill.bill_done = true;
                    }
                } 
            }

            foreach (TransactionModel model in newTransactions)
            {
                GlobalConfig.Connection.CreateTransaction(model);
                foreach (BillModel bill in availableBills)
                {
                    if(bill.bill_id == model.bill_id)
                    {
                        GlobalConfig.Connection.UpdateBill(bill); 
                    }
                }
            }
            MessageBox.Show("Done");
        }
    }
}
