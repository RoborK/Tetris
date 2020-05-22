using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Tetris
{
    public partial class CustomWindow : Window
    {
        Tetramino MainT;
        int indexFig, indexField, indexGran;        
        List<Brush> AllFigBrushes, AllFieldBrushes;
        GridField I, J, L, T, S, Z, O, Main;
        Brush FieldBrush = GridField.fieldBrush, GranBrush = GridField.granBrush;
        Brush FigColor = FigureParams.ColorsDic['I'];

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FigureParams.SetColor(MainT.NameFig, FigColor);
            GridField.fieldBrush = FieldBrush;
            GridField.granBrush = GranBrush;
            GranColor.Fill = GranBrush;
            FieldColor.Fill = FieldBrush;
            RePaintFig();
        }

        private void PrevFieldButton_Click(object sender, RoutedEventArgs e)
        {
            indexField = (indexFig - 1 + AllFieldBrushes.Count) % AllFieldBrushes.Count;
            ChangeColorField(AllFieldBrushes[indexField], AllFieldBrushes[indexGran]);
        }

        private void NextGranButton_Click(object sender, RoutedEventArgs e)
        {
            indexGran = (indexGran + 1) % AllFieldBrushes.Count;
            ChangeColorField(AllFieldBrushes[indexField], AllFieldBrushes[indexGran]);
        }

        private void PrevGranButton_Click(object sender, RoutedEventArgs e)
        {

            indexGran = (indexGran - 1 + AllFieldBrushes.Count) % AllFieldBrushes.Count;
            ChangeColorField(AllFieldBrushes[indexField], AllFieldBrushes[indexGran]);
        }

        private void NextFieldButton_Click(object sender, RoutedEventArgs e)
        {
            indexField = (indexField +1) % AllFieldBrushes.Count;
            ChangeColorField(AllFieldBrushes[indexField], AllFieldBrushes[indexGran]);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FigColor = FigureParams.ColorsDic[MainT.NameFig];
            FieldBrush = GridField.fieldBrush;
            GranBrush = GridField.granBrush;
            RePaintFig();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            indexFig = (indexFig - 1 + AllFigBrushes.Count) % AllFigBrushes.Count;
            ChangeColorFig(indexFig);
        }


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            indexFig = (indexFig + 1) % AllFigBrushes.Count;
            ChangeColorFig(indexFig);
        }

        
        private void FigGrid_J_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CancelButton_Click(sender, e);
            Main.TetraminoErase(MainT);
            MainT = new Tetramino((sender as Grid).Name.Last());
            MainT.MoveDown();
            Main.TetraminoDraw(MainT);
            indexFig = AllFigBrushes.FindIndex(x => x == MainT.Color);
            FigColor = MainT.Color;
        }

        private void ChangeColorFig(int index)
        {
            FigureParams.SetColor(MainT.NameFig, AllFigBrushes[index]);
            Main.TetraminoErase(MainT);
            MainT = new Tetramino(MainT.NameFig);
            MainT.MoveDown();
            Main.TetraminoDraw(MainT);
        }
        private void ChangeColorField(Brush FieldBrush, Brush GranBrush)
        {
            GridField.fieldBrush = FieldBrush;
            GridField.granBrush = GranBrush;
            GranColor.Fill = GranBrush;
            FieldColor.Fill = FieldBrush;
            Main.DrowField();
            Main.TetraminoDraw(MainT);
        }


        public CustomWindow()
        {
            InitializeComponent();
            AllFigBrushes = new List<Brush>(10)
            {
                Brushes.Cyan,
                Brushes.Orange,
                Brushes.Red,
                Brushes.Yellow,
                Brushes.Purple,
                Brushes.Blue,
                Brushes.Green,
            };
            int count = AllFigBrushes.Count;
            for(int i = 0; i < count; i++)
            {
                AllFigBrushes.Add(new LinearGradientBrush(((SolidColorBrush)AllFigBrushes[i]).Color, Colors.White, 90));
                AllFigBrushes.Add(new LinearGradientBrush(((SolidColorBrush)AllFigBrushes[i]).Color, Colors.White, 45));
                AllFigBrushes.Add(new LinearGradientBrush(((SolidColorBrush)AllFigBrushes[i]).Color, Colors.White, 0));
                AllFigBrushes.Add(new LinearGradientBrush(((SolidColorBrush)AllFigBrushes[i]).Color, Colors.Black, 90));
                AllFigBrushes.Add(new LinearGradientBrush(((SolidColorBrush)AllFigBrushes[i]).Color, Colors.Black, 45));
                AllFigBrushes.Add(new LinearGradientBrush(((SolidColorBrush)AllFigBrushes[i]).Color, Colors.Black, 0));
            }
            AllFieldBrushes = new List<Brush>(10)
            {
                Brushes.White,
                Brushes.Black,
                Brushes.DarkBlue,
                Brushes.Silver
                
            };

            I = new GridField(FigGrid_I);
            J = new GridField(FigGrid_J);
            L = new GridField(FigGrid_L);
            T = new GridField(FigGrid_T);
            S = new GridField(FigGrid_S);
            Z = new GridField(FigGrid_Z);
            O = new GridField(FigGrid_O);
            Main = new GridField(MainGrid);
            MainT = new Tetramino('I');
            indexFig = AllFigBrushes.FindIndex(x => x == MainT.Color);
            indexField = AllFieldBrushes.FindIndex(x => x == GridField.fieldBrush);
            indexGran = AllFieldBrushes.FindIndex(x => x == GridField.granBrush);
            FigColor = MainT.Color;
            MainT.MoveDown();
            GranColor.Fill = GranBrush;
            FieldColor.Fill = FieldBrush;
            RePaintFig();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            CancelButton_Click(sender, e);
            Close();
        }
        private void RePaintFig(char f='0')
        {
            Tetramino t = new Tetramino();
            t.MoveDown();
            if (FigureParams.CellsDic.Keys.Contains(f))
            {
                GetGridField(f).DrowField();
                t = new Tetramino(f);
                t.MoveDown();
                GetGridField(f).TetraminoDraw(t);
               
            }
            else
            {
                foreach (var item in FigureParams.CellsDic.Keys)
                {
                    GetGridField(item).DrowField();
                    t = new Tetramino(item);
                    t.MoveDown();
                    GetGridField(item).TetraminoDraw(t);
                }
                Main.DrowField();
                Main.TetraminoDraw(MainT);
            }
        }
        private  GridField GetGridField(char f = '0')
        {
            switch (f)
            {
                case 'I': 
                    return  I; 
                case 'J': 
                    return  J; 
                case 'L': 
                    return  L; 
                case 'O': 
                    return  O; 
                case 'S':
                    return  S; 
                case 'Z':
                    return  Z; 
                case 'T': 
                    return  T; 
                default: return  Main; 
            }
        }
    }
}
