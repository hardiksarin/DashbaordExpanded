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
        private List<PaymentBill> transactionBill = new List<PaymentBill>();
        private List<TransactionModel> newTransactions = new List<TransactionModel>();
        List<string> credDeb = new List<string> { "Cr", "Dr" };
        int AccountId = 0;
        int ParticularId = 0;
        double amount = 0;
        int count = 0;
        ICreateRequestor callingForm;
        public ReceiptVoucher(ICreateRequestor caller)
        {
            InitializeComponent();
            callingForm = caller;
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

            balanceComboBox.ItemsSource = null;
            balanceComboBox.ItemsSource = credDeb;
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
            AccountCurrentBalanceDataValue.Text = model.current_bal.ToString();
            AccountId = model.lid;
        }

        private void ParticularLedgerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paymentBills.Clear();
            LedgerModel model = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
            ParticularCurrentBalanceDataValue.Text = model.current_bal.ToString();
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
                if (count < 1)
                {
                    count++;
                    amount = double.Parse(AmountTextbox.Text);
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
                    UpdateBalance();
                }
            }
        }  

        public void BillComplete(PaymentBill model)
        {
            double amount = double.Parse(AmountTextbox.Text);
            double billAmount = double.Parse(model.amount);
            PaymentBill currentBill = new PaymentBill();
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
                double temp = amount - billAmount;
                AmountTextbox.Text = $"{temp:0.00}";
            }
            else if(amount > billAmount)
            {
                currentBill = model;
                BillwiseDataGrid.Items.Add(currentBill);
                transactionBill.Add(currentBill);
                paymentBills.Remove(model);
                double temp = amount - billAmount;
                AmountTextbox.Text = $"{temp:0.00}";
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
            double a = double.Parse(AmountTextbox.Text);
            if (a != 0)
            {
                BillWisePopUp form = new BillWisePopUp(this, paymentBills);
                form.Show(); 
            }
        }

        /*private void DataGridRow_KeyUp(object sender, KeyEventArgs e)
        {
            
        }*/

        private void SaveTransactions()
        {
            newTransactions.Clear();
            foreach (BillModel bill in availableBills)
            {
                foreach (PaymentBill b in transactionBill)
                {
                    if (b.emi == bill.bill_name && b.due_date == bill.bill_due_date.Split(' ').First())
                    {
                        TransactionModel transactionModel = new TransactionModel();
                        transactionModel.transaction_date = b.due_date;
                        transactionModel.transaction_amount = double.Parse(b.amount);
                        transactionModel.bill_id = bill.bill_id;
                        newTransactions.Add(transactionModel);
                        if(Math.Floor(transactionModel.transaction_amount) == Math.Floor(bill.bill_amount))
                        {
                            bill.bill_done = true;
                        }
                        else
                        {
                            bill.bill_amount -= transactionModel.transaction_amount;
                        }
                    }
                }
            }

            foreach (TransactionModel model in newTransactions)
            {
                GlobalConfig.Connection.CreateTransaction(model);
                foreach (BillModel bill in availableBills)
                {
                    if (bill.bill_id == model.bill_id)
                    {
                        GlobalConfig.Connection.UpdateBill(bill);
                    }
                }
            }
            if(MessageBox.Show("Transaction Complete", "", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                callingForm.NewReceipt();
            }
        }

        private double GetBalance(LedgerModel model)
        {
            double current = 0;
            if (model.credit_bal > model.debit_bal)
            {
                current = model.credit_bal - model.debit_bal;
            }
            else if (model.credit_bal < model.debit_bal)
            {
                current = model.debit_bal - model.credit_bal;
            }
            else
            {
                current = 0;
            }
            return current;
        }

        private double GetBalance(double cred, double deb)
        {
            double current = 0;
            if (cred > deb)
            {
                current = cred - deb;
            }
            else if (cred < deb)
            {
                current = deb - cred;
            }
            else
            {
                current = 0;
            }
            return current;
        }

        /// <summary>
        /// Creates Receipt Voucher with Particular data, Updates Balance of both ledgers
        /// Add transaction bills and remove done bills from bills.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)  
        {
            VoucherModel voucherModel = new VoucherModel();
            ParticularModel particularModel = new ParticularModel();

            //Save Receipt Voucher

            //Balance Logic
            string balType = (string)balanceComboBox.SelectedItem;
            if (balType == credDeb[0])
            {
                double amnt = amount;
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

            }
            else if (balType == credDeb[1])
            {
                double amnt = amount;
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

            //VoucherModel
            string d = DatePicker.SelectedDate.ToString().Split(' ').First();
            string[] dateList = d.Split('-');
            voucherModel.v_date = $"{dateList[2]}-{dateList[1]}-{dateList[0]}";
            if (vouchers.Count != 0)
            {
                voucherModel.v_number = vouchers[vouchers.Count - 1].vid + 1;
            }
            else
            {
                voucherModel.v_number = 1;
            }
            voucherModel.vtype = "Receipt";
            voucherModel.account = AccountId;

            //Particular Model
            particularModel.particular_amount = double.Parse(AmountTextbox.Text);
            particularModel.particular_name = ParticularId;

            voucherModel.Particular = particularModel;
            //Create Voucher And Particular Model.
            voucherModel = GlobalConfig.Connection.CreateVoucher(voucherModel);

            //Save Transactions
            SaveTransactions();
        }

        private void UpdateBalance()
        {
            LedgerModel accountModel = (LedgerModel)AccountLedgerCombobox.SelectedItem;
            LedgerModel particular = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
            string balType = (string)balanceComboBox.SelectedItem;
            double amnt = amount;
            if (balType == credDeb[0])
            {
                //update Particular Ledger
                double credBal = particular.credit_bal + amnt;
                double debBal = particular.debit_bal;
                double curBal = GetBalance(credBal, debBal);
                ParticularCurrentBalanceDataValue.Text = curBal.ToString();

                //update Account Ledger
                double adebBal = accountModel.debit_bal + amnt;
                double acurBal = GetBalance(accountModel.credit_bal, adebBal);
                AccountCurrentBalanceDataValue.Text = acurBal.ToString();

            }
            else if (balType == credDeb[1])
            {
                //update Particular Ledger
                double pdebBal = particular.debit_bal + amnt;
                double curBal = GetBalance(particular.credit_bal, pdebBal);
                ParticularCurrentBalanceDataValue.Text = curBal.ToString();

                //update Account Ledger
                double acredBal = accountModel.credit_bal + amnt;
                double acurBal = GetBalance(acredBal, accountModel.debit_bal);
                AccountCurrentBalanceDataValue.Text = acurBal.ToString();
            }
        }

        private void balanceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AmountTextbox.Text.Length != 0)
            {
                UpdateBalance();
            }
        }
    }
}
