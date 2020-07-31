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

// TODO: Cr/Dr Toggle Adjecnt to Amount
// TODO: Placemnet to Submit/Create the voucher
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
        int AccountId = 0;
        int ParticularId = 0;
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
            AccountCurrentBalanceDataValue.Text = model.ledger_opening_balance.ToString();
            AccountId = model.lid;

        }

        private void ParticularLedgerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LedgerModel model = (LedgerModel)ParticularLedgerCombobox.SelectedItem;
            ParticularCurrentBalanceDataValue.Text = model.ledger_opening_balance.ToString();
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

        private void StartDatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (MessageBox.Show("Do you  want to save?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    WireUpDatabase();
                }
            }
        }

        private void WireUpDatabase()
        {
            VoucherModel voucherModel = new VoucherModel();
            ParticularModel particularModel = new ParticularModel();

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
                billModel.bill_due_date = bill.due_date;
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
    }
}
