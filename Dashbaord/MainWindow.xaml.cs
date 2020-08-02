using GravitonLibrary;
using GravitonLibrary.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ICreateRequestor
    {
        public MainWindow()
        {
            GlobalConfig.InitializeConnections();
            InitializeComponent();
        }

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ButtonPopUpQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        public void CtrShortcut1(Object sender, ExecutedRoutedEventArgs e)
        {
            ButtonPopUpQuit.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Do you  want to close?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Environment.Exit(0);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 0));
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 1));
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 2));
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 3));
                    break;
                default:
                    break;
            }
        }


        private void PaymentVoucherCreate_Click(object sender, RoutedEventArgs e)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new PaymentVoucher(this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new ReceiptVoucher(this));
        }

        public void LedgerClicked(LedgerModel model)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new DisplayLedgers(this,model));
        }

        public void CategoryClicked(CostCategoryModel model)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new DisplayCategory(this,model));
        }

        public void GroupClicked(GroupModel model)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new DisplayGroup(this,model));
        }

        public void CostCenterClicked(CostCenterModel model)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new DisplayCostCenter(this,model));
        }

        public void Create(int index)
        {
            switch (index)
            {
                case 0:                                                 //Ledger
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new CreateLedger(this));
                    break;
                case 1:                                                 //Groups
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new CostGroupCreation(this));
                    break;
                case 2:                                                 //Cost Center
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new CostCenterCreation(this));
                    break;
                case 3:                                                 //Cost Category
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new CostCategoryCreation(this));
                    break;
                default:
                    break;
            }
        }

        public void Home(int index)
        {
            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 0));
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 1));
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 2));
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DisplayObject(this, 3));
                    break;
                default:
                    break;
            }
        }

        public void NewPayment()
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new PaymentVoucher(this));
        }

        public void NewReceipt()
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new ReceiptVoucher(this));
        }
    }
}
