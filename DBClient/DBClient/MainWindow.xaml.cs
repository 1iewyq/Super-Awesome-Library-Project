using DBInterface;
using DBLib;
using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;


namespace DBClient
{
    public delegate DataStruct SearchDelegate(string lastName);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();
            var tcp = new NetTcpBinding();
            var URL = "net.tcp://localhost:8200/BusinessService";
            var chanFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = chanFactory.CreateChannel();
            totalLabel.Content = "Total Items: " + foob.GetNumEntries();
            LoadData(0);
            indexBox.Text = "0";
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(indexBox.Text, out int index))
            {
                LoadData(index);
            }
            else
            {
                MessageBox.Show("Please enter a valid index number.");
            }
        }

        private void LoadData(int index)
        {
            try
            {
                foob.GetValuesForEntry(index, out var accNo, out var pin, out var bal, out var fName, out var lName, out var icon);
                fNameBox.Text = fName;
                lNameBox.Text = lName;
                balBox.Text = bal.ToString("C");
                accNoBox.Text = accNo.ToString();
                pinBox.Text = pin.ToString("D4");
                UserIcon.Source = Imaging.CreateBitmapSourceFromHBitmap(icon.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                icon.Dispose();
            }
            catch (FaultException<IndexOutOfRangeFault> exception)
            {
                MessageBox.Show(exception.Detail.Issue);
            }
        }

       /* private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchDelegate del = foob.SearchByLastName; //At BusinessServerInterface, uncomment the SearchByLastName method if you want to use this line.
            del.BeginInvoke(searchBox.Text, SearchCallback, del);
        }*/

        private void SearchCallback(IAsyncResult ar)
        {
            var del = (SearchDelegate)((AsyncResult)ar).AsyncDelegate;
            DataStruct result = del.EndInvoke(ar);

            Dispatcher.Invoke(() =>
            {
                fNameBox.Text = result.firstName;
                lNameBox.Text = result.lastName;
                accNoBox.Text = result.acctNo.ToString();
                pinBox.Text = result.pin.ToString("D4");
                balBox.Text = result.balance.ToString("C");


                searchBox.IsReadOnly = false;
                searchButton.IsEnabled = true;
                searchProgressBar.Visibility = Visibility.Collapsed;
                searchProgressBar.IsIndeterminate = false;
            });
        }
    }
}
