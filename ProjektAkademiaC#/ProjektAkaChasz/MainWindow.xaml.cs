using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace ProjektAkaChasz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public enum MultimediaType { Movie, Music, Game }

    public partial class MainWindow : Window
    {
        public ObservableCollection<Item> ItemList { get; set; }
        public List<Item> Cart { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            ItemList = new ObservableCollection<Item>();
            Cart = new List<Item>();
        }

        private void MakeOrder_Click(object sender, RoutedEventArgs e)
        {
            if(Cart.Count > 0)
            {
                MessageBox.Show("Thank You for Making the Order. Total Price is " + GetTotalPrice().ToString() + "$", "Shopping Summary");
            }
            else
            {
                MessageBox.Show("Add One Item to Your Cart at Least", "Add an Item");
            }
        } 

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cart.Add(this.ItemList[this.OverallListView.SelectedIndex]);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Select an Item", "No Item Selected");
                Console.Write(exc.Message);
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cart.Remove(this.ItemList[this.OverallListView.SelectedIndex]);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Select an Item", "No Item Selected");
                Console.Write(exc.Message);
            }
        }

        private void ShowCartItems_Click(object sender, RoutedEventArgs e)
        {
            string cartInfo = "";

            for(int i = 0; i < Cart.Count; i++)
            {         
                cartInfo += Cart[i].Name + ", " + Cart[i].Author + ", " + Cart[i].Price + "$" + "\r\n";
            }

            this.CartTextBox.Text = cartInfo;
        }

        private void SortCart_Click(object sender, RoutedEventArgs e)
        {
            Cart.Sort();
            ShowCartItems_Click(sender, e);
        }

        private void ExtractToFile_CLick(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("OrderedItems.txt"))
            {
                for (int i = 0; i < Cart.Count; i++)
                {
                    writer.WriteLine((i+1) + ". " + Cart[i].Name + ", " + Cart[i].Author + ", " + Cart[i].Price + "$" + "\r\n");
                }

                writer.WriteLine("-------------------");
                writer.WriteLine("Total Price: " + GetTotalPrice().ToString() + "$");
            }
        }

        private void CalculateTotalPrice_Click(object sender, RoutedEventArgs e)
        {
            double totalPrice = GetTotalPrice();

            this.TotalPriceTextBox.Text = totalPrice.ToString() + "$";
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("PredefinedItems.txt"))
                {
                    string line;
                    List<string> lines = new List<string>();
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);  
                    }

                    ShowLoadedItems(lines);
                }
            }
            catch (Exception exc)
            {
                Console.Write(exc.Message);
            }

            this.RaportTextBox.AppendText(@"Items form 'PredefinedItems.txt' loaded." + "\r\n");
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("PredefinedItems2.txt"))
            {
                for (int i = 0; i < ItemList.Count; i++ )
                {
                    string type = "";

                    if (ItemList[i] is MovieDVD) type = "Movie";
                    if (ItemList[i] is MusicCD) type = "Music";
                    if (ItemList[i] is GameCD) type = "Game";

                    writer.WriteLine(type);
                    writer.WriteLine(ItemList[i].Name);
                    writer.WriteLine(ItemList[i].Author);
                    writer.WriteLine(ItemList[i].Year);
                    writer.WriteLine(ItemList[i].Type);
                    writer.WriteLine(ItemList[i].Price);
                    writer.WriteLine(ItemList[i].Add);
                }
            }

            this.RaportTextBox.AppendText(@"Items saved to 'PredefinedItems.txt'." + "\r\n");
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RaportTextBox.AppendText("Item no " + (this.OverallListView.SelectedIndex + 1) + " deleted." + "\r\n");
                this.ItemList.RemoveAt(this.OverallListView.SelectedIndex);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Select Item to Delete", "No Chosen Item Warning");
                Console.Write(exc.Message);
            }            
        }

        private void IncreasePrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double oldPrice = this.ItemList[this.OverallListView.SelectedIndex].Price;
                double currentPrice = 0;
                
                this.ItemList[this.OverallListView.SelectedIndex].IncreasePrice(double.Parse(this.PriceMultiplyTextBox.Text));
                currentPrice = this.ItemList[this.OverallListView.SelectedIndex].Price;

                this.RaportTextBox.AppendText("Price increased from " + oldPrice + " to " + currentPrice + "." + "\r\n");
            }
            catch(Exception exc)
            {
                MessageBox.Show("Select Item to Increase its Price", "No Chosen Item Warning");
                Console.Write(exc.Message);
            }     
        }

        private void AddMovieToList_Click(object sender, RoutedEventArgs e)
        {
            AddToList(this.MovieTitleTextBox, this.MovieDirectorTextBox, this.MovieReleaseYearTextBox, this.MovieTypeTextBox, this.MoviePriceTextBox,
                      this.PolishSubtitlesRadio, this.PolishLectorRadio, this.NoLectorNoSubRadio,
                      Adds.Subtitles, Adds.Lector, Adds.None, MultimediaType.Movie);
        }

        private void AddMusicToList_Click(object sender, RoutedEventArgs e)
        {
            AddToList(this.MusicTitleTextBox, this.MusicArtistTextBox, this.MusicReleaseYearTextBox, this.MusicTypeTextBox, this.MusicPriceTextBox,
                      this.GrammyRadio, this.BillboardRadio, this.MTVRadio,
                      Adds.Grammy_Award, Adds.Billboard_Award, Adds.MTV_Award, MultimediaType.Music);
        }

        private void AddGameToList_Click(object sender, RoutedEventArgs e)
        {
            AddToList(this.GameTitleTextBox, this.GameStudioTextBox, this.GameReleaseYearTextBox, this.GameTypeTextBox, this.GamePriceTextBox,
                      this.AgeOf3Radio, this.AgeOf12Radio, this.AgeOf18Radio,
                      Adds.Age_Of_3, Adds.Age_Of_12, Adds.Age_Of_18, MultimediaType.Game);
        }

        private void AddToList(TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4, TextBox tb5, 
                               RadioButton rb1, RadioButton rb2, RadioButton rb3, 
                               Adds a1, Adds a2, Adds a3, MultimediaType multiType)
        {
            if (AreTextFieldsFilled(tb1.Text, tb2.Text, tb3.Text, tb4.Text, tb5.Text))
            {
                Adds add = CheckAdditionalFeatures(rb1, rb2, rb3, a1, a2, a3);

                try
                {
                    switch (multiType)
                    {
                        case MultimediaType.Movie: ItemList.Add(new MovieDVD(tb1.Text, tb2.Text, int.Parse(tb3.Text), tb4.Text, double.Parse(tb5.Text), add)); break;
                        case MultimediaType.Music: ItemList.Add(new MusicCD(tb1.Text, tb2.Text, int.Parse(tb3.Text), tb4.Text, double.Parse(tb5.Text), add)); break;
                        case MultimediaType.Game: ItemList.Add(new GameCD(tb1.Text, tb2.Text, int.Parse(tb3.Text), tb4.Text, double.Parse(tb5.Text), add)); break;
                    }
                }
                catch (FormatException fe)
                {
                    MessageBox.Show(@"Type 'Release Year' and 'Price' in the format of Integer or Double", "Number Format Warning");
                    Console.Write(fe.Message);
                }
            }
        }

        private bool AreTextFieldsFilled(string tf1, string tf2, string tf3, string tf4, string tf5)
        {
            if(tf1 != "" &&
               tf2 != "" &&
               tf3 != "" &&
               tf4 != "" &&
               tf5 != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Adds CheckAdditionalFeatures(RadioButton rb1, RadioButton rb2, RadioButton rb3, Adds feature1, Adds feature2, Adds feature3)
        {
            Adds add = Adds.None;

            if (rb1.IsChecked.Value)
            {
                add = feature1;
            }
            if (rb2.IsChecked.Value)
            {
                add = feature2;
            }
            if (rb3.IsChecked.Value)
            {
                add = feature3; ;
            }

            return add;
        }

        private void ShowLoadedItems(List<string> lines)
        {
            int oneRecordRows = 7;

            for (int i = 0; i < lines.Count; i++)
            {
                if (i % oneRecordRows == 0)
                {
                    switch (lines[i])
                    {
                        case "Movie": ItemList.Add(new MovieDVD(lines[i + 1], lines[i + 2], int.Parse(lines[i + 3]), lines[i + 4], double.Parse(lines[i + 5]), Adds.Lector)); break;
                        case "Music": ItemList.Add(new MusicCD(lines[i + 1], lines[i + 2], int.Parse(lines[i + 3]), lines[i + 4], double.Parse(lines[i + 5]), Adds.Grammy_Award)); break;
                        case "Game": ItemList.Add(new GameCD(lines[i + 1], lines[i + 2], int.Parse(lines[i + 3]), lines[i + 4], double.Parse(lines[i + 5]), Adds.Age_Of_18)); break;
                    }
                }
            }
        }

        private double GetTotalPrice()
        {
            double totalPrice = 0;

            for (int i = 0; i < Cart.Count; i++)
            {
                totalPrice += Cart[i].Price;
            }
            return totalPrice;
        }               
    }
}
