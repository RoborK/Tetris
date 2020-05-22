using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tetris
{
   
    public class Record: IComparer<Record> 
    {
        /// <param name="Name">Имя игрока</param>
        /// <param name="Lines">количество собраных линий</param>
        /// <param name="Score">количество набранных очков</param>
        public Record(string Name, int Lines, int Score)
        {
            this.Name = Name;
            this.Lines = Lines;
            this.Score = Score;
        }
        public string Name;
        public int Lines;
        public int Score;


        public int Compare(Record x, Record y)
        {
            return x.Score.CompareTo(y.Score);
        }
        public override string ToString()
        {
            return String.Format("{0} {1} {2}", Name, Lines, Score);
        }
    }
    public partial class RecordsWindow : Window
    {
        static string file = "result.txt";
        public static List<Record> result = new List<Record>();
        public RecordsWindow()
        {
            InitializeComponent();
        }

        public static void SaveRecord()
        {
            result.Sort((x, y) => y.Score.CompareTo(x.Score));
            if (result.Count > 10) result.RemoveRange(10, result.Count -10);
            using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
            {
                foreach (var item in result)
                {
                    sw.WriteLine(item);
                }
            }
        }
        public static void GetRecord()
        {
            result.Clear();
            string[] inp = File.ReadAllLines(file, Encoding.GetEncoding(1251));
            foreach (var item in inp)
            {
                string[] rec = item.Split();
                if (rec.Length == 3)
                {
                    result.Add(new Record(rec[0], int.Parse(rec[1]), int.Parse(rec[2])));
                } 
               
            }


        }
        public static void AddRecord(string Name, int Lines, int Score)
        {
            GetRecord();
            result.Add(new Record(Name, Lines, Score));
            SaveRecord();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int rows = RecordGrid.RowDefinitions.Count;
            int columns = RecordGrid.ColumnDefinitions.Count;
            Label[,] Table = new Label[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {

                    Table[x, y] = new System.Windows.Controls.Label();
                    Grid.SetColumn(Table[x, y], x);
                    Grid.SetRow(Table[x, y], y);
                    RecordGrid.Children.Add(Table[x, y]);
                    Table[x, y].Background = Brushes.Azure;
                    Table[x, y].BorderThickness = new Thickness(4);
                }
            }
            Table[0,0].Content = "№";
            Table[1, 0].Content = "Name";
            Table[2, 0].Content = "Lines";
            Table[3, 0].Content = "Score";
            int index = 1;
            GetRecord();
            foreach (var item in result)
            {
                Table[0,index].Content = index;
                Table[1, index].Content = item.Name;
                Table[2, index].Content = item.Lines;
                Table[3, index].Content = item.Score;
                index++;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
