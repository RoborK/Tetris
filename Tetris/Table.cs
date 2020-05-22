using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{


    class GridField //поле клеток для фигур тетрамино
    {
        static public Brush fieldBrush = Brushes.White;
        static public Brush granBrush = Brushes.White;
        /// <param name="tetramino"></param>
        public void TetraminoDraw(Tetramino tetramino)
        {
            Point pos = tetramino.Position;
            Point[] figure = tetramino.Cells;
            Brush color = tetramino.Color;
            foreach (Point P in figure)
            {
                Field[(int)(P.X + pos.X) + ((columns / 2) - 1),
                    (int)(P.Y + pos.Y)].Fill = color;
            }
        }
        public void TetraminoErase(Tetramino tetramino)
        {
            Point pos = tetramino.Position;
            Point[] figure = tetramino.Cells;
            foreach (Point S in figure)
            {
                Field[(int)(S.X + pos.X) + ((columns / 2) - 1),
                    (int)(S.Y + pos.Y)].Fill = GridField.fieldBrush;
            }
        }
        public int rows;
        public int columns;
        public Rectangle[,] Field;
        public GridField(Grid grid)
        {
            grid.Background = GridField.fieldBrush;
            rows = grid.RowDefinitions.Count;
            columns = grid.ColumnDefinitions.Count;
            Field = new Rectangle[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {

                    Field[x, y] = new System.Windows.Shapes.Rectangle();
                    Grid.SetColumn(Field[x, y], x);
                    Grid.SetRow(Field[x, y], y);
                    grid.Children.Add(Field[x, y]);
                    Field[x, y].Fill = fieldBrush;
                    Field[x, y].Stroke = granBrush;
                    Field[x, y].StrokeThickness = 0.5;
                }
            }
        }
        public void DrowField()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Field[x, y].Fill = fieldBrush;
                    Field[x, y].Stroke = granBrush;
                }
            }
        }

    }
    public class Table // Игровое поле
    {
     

        private int score;
        private int linesAssembled;
        private int lvl;
        public int StepLvl = 3;
        private int thisLevelLinesAssembled;
        private bool gameOver = false;
        public bool LvlUp;
        private Tetramino currentTetramino;
        private GridField TetrisField;
        private GridField NextFigGrid;
        private Tetramino nextTetramino;
        public int GetLvlProc()
        {
            if (linesAssembled == 0 || thisLevelLinesAssembled == StepLvl) return 0;
            return 100 * thisLevelLinesAssembled / StepLvl;
        }
        /// <param name="TetrisGrid">Grid основного игрового поля</param>
        /// <param name="NextFigGrid">Grid для показа следующей фигуры</param>
        public Table(Grid TetrisGrid, Grid NextFigGrid)
        {
            TetrisField = new GridField(TetrisGrid);
            this.NextFigGrid = new GridField(NextFigGrid);
            LvlUp = false;
            score = 0;
            linesAssembled = 0;
            lvl = 1;
            thisLevelLinesAssembled = 0;

            currentTetramino = new Tetramino();
            TetrisField.TetraminoDraw(currentTetramino);
            nextTetramino = new Tetramino();
            this.NextFigGrid.TetraminoDraw(nextTetramino);
        }
        public int Score => score;
        public int Lines => linesAssembled;
        public int LVL => lvl;
        public bool GameOver => gameOver;
        private void CheckGameOver()
        {
            for (int x = 0; x < TetrisField.columns; x++)
                if (TetrisField.Field[x, 0].Fill != GridField.fieldBrush)
                    gameOver = true;
        }
        private void CheckRows()
        {
            bool full;
            int kol = 0;
            for (int y = 0; y < TetrisField.rows; y++)
            {
                full = true;
                for (int x = 0; x < TetrisField.columns; x++)
                {
                    if (TetrisField.Field[x, y].Fill == GridField.fieldBrush)
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    kol++;
                    RemoveRow(y);
                    score += 100 * lvl;
                    linesAssembled += 1;
                    thisLevelLinesAssembled += 1;

                }
            }

            if (kol > 1)
            {
                score += kol * kol * 100 * lvl;//бонус удалённых линий
                CheckLvlUp();
            }
        }
        private void CheckLvlUp()
        {
            if (thisLevelLinesAssembled >= StepLvl)
            {
                lvl++;
                thisLevelLinesAssembled -= StepLvl;
                StepLvl++;
                LvlUp = true;
            }
        }
        private void RemoveRow(int row)
        {
            for (int y = row; y > 0; y--)
            {
                for (int x = 0; x < TetrisField.columns; x++)
                {
                    TetrisField.Field[x, y].Fill = TetrisField.Field[x, y - 1].Fill;
                }
            }
        }
        public void CurentTetraminoMoveLeft()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Cells;
            bool canMove = true;
            TetrisField.TetraminoErase(currentTetramino);
            foreach (Point P in figure)
            {
                // не можем сдвинуть влево - у левой границы поля
                if (((int)(P.X + pos.X) + ((TetrisField.columns / 2) - 1) - 1) < 0)
                {
                    canMove = false;
                }
                // не можем влево - слева стоит фигура
                else if (TetrisField.Field[((int)(P.X + pos.X) + ((TetrisField.columns / 2) - 1) - 1),
                    (int)(P.Y + pos.Y)].Fill != GridField.fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.MoveLeft();
            TetrisField.TetraminoDraw(currentTetramino);
        }
        public void CurTetraminoMoveRight()
        {
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Cells;
            TetrisField.TetraminoErase(currentTetramino);
            bool canMove = true;
            foreach (Point S in figure)
            {
                if (((int)(S.X + pos.X) + ((TetrisField.columns / 2) - 1) + 1) >= TetrisField.columns)
                {
                    canMove = false;
                }
                else if (TetrisField.Field[((int)(S.X + pos.X) + ((TetrisField.columns / 2) - 1) + 1),
                    (int)(S.Y + pos.Y)].Fill != GridField.fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
                currentTetramino.MoveRight();
            TetrisField.TetraminoDraw(currentTetramino);
        }
        public void CurTetraminoMovDown(bool CoolGame = false)
        {
            // бонус за быструю игру 
            if (CoolGame) score += 1 * lvl;
            Point pos = currentTetramino.Position;
            Point[] figure = currentTetramino.Cells;
            bool canMove = true;
            TetrisField.TetraminoErase(currentTetramino);
            foreach (Point S in figure)
            {
                if (((int)(S.Y + pos.Y) + 1) >= TetrisField.rows)
                {
                    canMove = false;
                }
                else if (TetrisField.Field[((int)(S.X + pos.X) + ((TetrisField.columns / 2) - 1)),
                    (int)(S.Y + pos.Y) + 1].Fill != GridField.fieldBrush)
                {
                    canMove = false;
                }
            }
            if (canMove)
            {
                currentTetramino.MoveDown();
                TetrisField.TetraminoDraw(currentTetramino);
            }
            else
            {
                TetrisField.TetraminoDraw(currentTetramino);
                CheckRows();
                CheckGameOver();
                currentTetramino = nextTetramino;
                NextFigGrid.TetraminoErase(nextTetramino);
                nextTetramino = new Tetramino();
                NextFigGrid.TetraminoDraw(nextTetramino);
                score += lvl * 10; 
            }
        }
        public void CurTetraminoMoveRotate()
        {
            if (currentTetramino.NameFig != 'O')
            {
                Point pos = currentTetramino.Position;
                Point[] S = new Point[4];
                Point[] figure = currentTetramino.Cells;
                bool canMove = true;
                figure.CopyTo(S, 0);
                TetrisField.TetraminoErase(currentTetramino);
                for (int i = 0; i < S.Length; i++)
                {
                    double x = S[i].X;
                    S[i].X = -S[i].Y;
                    S[i].Y = x;

                    if (((int)((S[i].Y + pos.Y) + 1)) <= 0) // сверху - стена
                    {
                        canMove = false;
                    }
                    else if (((int)((S[i].Y + pos.Y) + 1)) >= TetrisField.rows) // снизу - стена
                    {
                        canMove = false;
                    }
                    else if (((int)(S[i].X + pos.X) + ((TetrisField.columns / 2) - 1) + 1) <= 0) //слева - стена
                    {
                        canMove = false;
                    }
                    else if (((int)(S[i].X + pos.X) + ((TetrisField.columns / 2) - 1) + 1) > TetrisField.columns) // справа - стена
                    {
                        canMove = false;
                    }
                    else if (TetrisField.Field[(int)(S[i].X + pos.X) + ((TetrisField.columns / 2) - 1),
                        (int)(S[i].Y + pos.Y)].Fill != GridField.fieldBrush)
                    {
                        canMove = false;
                    }
                }
                if (canMove)
                    currentTetramino.Turn();
                TetrisField.TetraminoDraw(currentTetramino);

            }
        }
    }

}
