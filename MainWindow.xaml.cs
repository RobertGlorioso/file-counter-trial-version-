using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage.Streams;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        CancellationTokenSource s_cts = new CancellationTokenSource();

        private void PrintManifest(object sender, RoutedEventArgs e)
        {
            ShoppingCart.ViewFilter = TU.Text;
            ShoppingCart.PrintManifest2();
        }

        private void SetItemCount(object sender, RoutedEventArgs e)
        {
            int itemCount = ShoppingCart.ItemCount;
            TF.Text = itemCount.ToString();
            string tg = ShoppingCart.TargetDir;
            TE.Text = tg;
            TT.ItemsSource = null;

            try
            {
                this.Dispatcher.Invoke(() => TT.ItemsSource = ShoppingCart.GetFileView()); 
            
            }
            catch (Exception ex) { Console.WriteLine(ex); throw (ex);  }

        }

        private async void DoSearch(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if ((string)b.Content != "Cancel")
            {
                b.Content = "Cancel";
                s_cts.Dispose();
                s_cts = new CancellationTokenSource();
                b.Foreground = new SolidColorBrush(Colors.YellowGreen);
                ShoppingCart.TargetDir = TC.Text;
                string tg = ShoppingCart.TargetDir;
                TE.Text = tg;
                try
                {
                    await Task.Run(() => Directory2Search(tg, '_', s_cts.Token));
                    //Directory2Search(tg, '_', s_cts.Token).Wait(); //blocks
                }
                catch (Exception ex)
                {
                    b.Foreground = new SolidColorBrush(Colors.Red);
                    TG.Text = ex.ToString();
                    Console.Write(ex);
                }
                finally
                {
                    int itemCount = ShoppingCart.ItemCount;
                    TF.Text = itemCount.ToString();
                    string tg2 = ShoppingCart.TargetDir;
                    TG.Text = tg2;
                    b.Content = "View Files";

                }

            }
            else
            {
                b.Content = "View Files";
                s_cts.Cancel();

            }
        }
        private async Task Directory2Search(string dir, char delim, CancellationToken cts)
        {
            ShoppingCart.TargetDir = dir ?? "D:/";
            if (delim == '\0') { delim = '_'; }
            this.Dispatcher.Invoke(() => { TE.Text = dir; });
            this.Dispatcher.Invoke(() => { TF.Text = ShoppingCart.ItemCount.ToString(); });
            this.Dispatcher.Invoke(() => { TT.ItemsSource = ShoppingCart.GetFileView(); });
            if (cts.IsCancellationRequested)
            {
                //cts.ThrowIfCancellationRequested();
                return;
            }
            try
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                FileInfo[] files = directory.GetFiles();
                DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (FileInfo f in files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)))
                {
                    var parsedFile = System.IO.Path.GetFileName(f.ToString()).Split(delim);
                    if (parsedFile.Length > 0)
                    {
                        {
                            ShoppingCart.ItemCount += 1;
                            var loannum = parsedFile.First();
                            var lf = ShoppingCart.LoanFiles ?? new Dictionary<string, HashSet<string>>();
                            if (lf.ContainsKey(loannum))
                            {
                                lf[loannum].Add(dir.ToString() + "\\" + f.ToString());
                            }
                            else
                            {
                                lf.Add(loannum, new HashSet<string> { dir.ToString() + "\\" + f.ToString() });
                            }
                            ShoppingCart.LoanFiles = lf;
                        }
                    }
                }

                foreach (DirectoryInfo d in dirs.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)))
                {
                    if (Directory.Exists(d.FullName))
                    {
                        //await Task.Run(() => DirectorySearch(d, delim, cts));
                        await Directory2Search(d.FullName, delim, cts);
                    }
                }
            }
            catch (Exception ex) {  Console.WriteLine(ex.ToString()); }
        }
        
    }

    public class ShoppingCart : DependencyObject,INotifyCollectionChanged,INotifyPropertyChanged
    {
        public static IEnumerable<object> GetFileView()
        {
            var lf = ShoppingCart.LoanFiles ?? new Dictionary<string, HashSet<string>>();
            var df = new HashSet<string>();
            var dl = new List<string>();
            var lfk = lf.Keys;
            var listFilesView = from loan in lfk
                                let c = lf.TryGetValue(loan, out df) ? lf[loan].ToList() : dl
                                select new { loan_id = loan, numFiles = c.Count };
            return listFilesView.Take(100);
        }
        public static void PrintManifest()
        {
            var lf = ShoppingCart.LoanFiles ?? new Dictionary<string, HashSet<string>>();
            var df = new HashSet<string>();
            var dl = new List<string>();
            var lfk = lf.Keys;
            var listFilesView = from loan in lfk
                                let c = lf.TryGetValue(loan, out df) ? lf[loan].ToList() : dl
                                select loan.ToString() + "," + c.Count.ToString();
            string path = @"C:\temp\manifest.csv"; // path to file
            using (FileStream fs = File.Create(path))
            {
                var buffer = new StringBuilder();
                listFilesView.ToList().ForEach(item => buffer.AppendLine(item));
                byte[] info = new UTF8Encoding(true).GetBytes(buffer.ToString());
                fs.Write(info, 0, info.Length);
                byte[] data = new byte[] { 0x0 };
                fs.Write(data, 0, data.Length);
            }

        }
        public static void PrintManifest2()
        {
            
            var lf = ShoppingCart.LoanFiles ?? new Dictionary<string, HashSet<string>>();
            var ff = ShoppingCart.ViewFilter ?? "";
            string[] sf = ff.Split(new char[] { '\n', '\r' });
            var df = new HashSet<string>();
            var dl = new List<string>();
            var lfk = lf.Keys;
            var buffer = new StringBuilder();
            foreach (String s in sf)
            {
                if (lfk.Contains(s))
                {
                    var lfffff = lf.TryGetValue(s, out df) ? lf[s].ToList() : dl;

                    lfffff.ForEach(item => buffer.AppendLine(item));

                }
            }
            string path = @"C:\temp\manifest.csv"; // path to file
            Console.WriteLine(buffer.Length);
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(buffer.ToString());
                fs.Write(info, 0, info.Length);

                byte[] data = new byte[] { 0x0 };
                fs.Write(data, 0, data.Length);
            }
            
        }
        public static Dictionary<String, HashSet<String>> LoanFiles { get; set; }

        public static DependencyProperty ItemCountProperty =
             DependencyProperty.Register("ItemCount", typeof(int), typeof(ShoppingCart), new PropertyMetadata(760));

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty DirectoryProperty =
            DependencyProperty.Register("TargetDir", typeof(string), typeof(ShoppingCart), new PropertyMetadata("C:/"));

        public static string ViewFilter
        { get; set; }
        public static string TargetDir
        { get; set; }
        public static int ItemCount
        { get; set; }
    }
}

