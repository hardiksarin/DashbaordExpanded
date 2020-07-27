using GravitonLibrary;
using GravitonLibrary.Models;
using System;
using System.Collections.Generic;
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
    public partial class ReceiptVoucher : UserControl
    {
        private List<LedgerModel> availableLedgers = new List<LedgerModel>();
        private List<BillModel> availableBills = new List<BillModel>();
        private List<PaymentBill> paymentBills = new List<PaymentBill>();
        private List<VoucherModel> vouchers = new List<VoucherModel>();
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
            vouchers = GlobalConfig.Connection.GetVoucher_All();
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
            int length = vouchers.Count;
            RecieptVoucherNumberLabel.Text = $"No. {vouchers[length - 1].vid + 1 }";
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
                BillWisePopUp form = new BillWisePopUp(paymentBills);
                form.Show();
            }
        }
    }
}
