using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SudokuSolverDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateSudokuGrid();

            UILocked = false;
        }

        private bool UILocked {get; set;}

        /// <summary>
        /// Create the grid cells here.
        /// I didn't use proper binding and MVVM since the boilerplate needed would be excessive
        /// </summary>
        private void CreateSudokuGrid()
        {
            for(int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    TextBox box = new TextBox();
                    box.PreviewTextInput += NumberValidation;
                    box.TextChanged += NumberChanged;
                    box.Height = 50;
                    box.Width = 50;
                    box.FontSize = 23;
                    box.TextAlignment = TextAlignment.Center;
                    box.VerticalContentAlignment = VerticalAlignment.Center;
                    box.MaxLength = 1;

                    // Add some thicker borders
                    double left = 0.5;
                    double top = 0.5;
                    double right = 0.5;
                    double bottom = 0.5;

                    if (x % 3 == 0)
                    {
                        left = 2;
                    }
                    if (y % 3 == 0)
                    {
                        top = 2;
                    }
                    if (x == 8)
                    {
                        right = 2;
                    }
                    if (y == 8)
                    {
                        bottom = 2;
                    }

                    box.BorderThickness = new Thickness(left, top, right, bottom);

                    Grid.SetColumn(box, x);
                    Grid.SetRow(box, y);
                    SudokuGrid.Children.Add(box);
                }
            }
        }

        /// <summary>
        /// Try making sure the typed character is a number before accepting it
        /// </summary>
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            if (UILocked)
            {
                return;
            }

            int num;

            // Check that input is a number
            if (!int.TryParse(e.Text, out num))
            {
                e.Handled = true;
                return;
            }

            // No zero in Sudoku
            if(num == 0)
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Push the solved puzzle into the UI (should actually be done with bindings)
        /// </summary>
        internal void UpdateCells()
        {
            foreach (UIElement elem in SudokuGrid.Children)
            {
                TextBox box = elem as TextBox;
                if (box != null)
                {
                    int x = Grid.GetColumn(box);
                    int y = Grid.GetRow(box);
                    box.Text = SudokuPuzzle.ThePuzzle.GetCell(x, y).ToString();
                }
            }
        }

        /// <summary>
        ///  We only want to accept values that are valid for the current puzzle
        ///  If the entered value is valid, update the puzzle
        /// </summary>
        private void NumberChanged(object sender, EventArgs e)
        {
            if (UILocked)
            {
                return;
            }

            int x, y, enteredNum;
            List<int> valids;

            TextBox box = sender as TextBox;
            if(box.Text == String.Empty)
            {
                x = Grid.GetColumn(box);
                y = Grid.GetRow(box);
                SudokuPuzzle.ThePuzzle.SetCell(x, y, 0);
                return;
            }
            if(box != null)
            {
                enteredNum = int.Parse(box.Text);

                x = Grid.GetColumn(box);
                y = Grid.GetRow(box);
                valids = SudokuPuzzle.ThePuzzle.GetValidValues(x, y);
                if(valids.Contains(enteredNum))
                {
                    SudokuPuzzle.ThePuzzle.SetCell(x, y, enteredNum);
                    return;
                }
            }

            box.Text = "";
        }

        private void ClearGrid(object sender, RoutedEventArgs e)
        {
            if(UILocked)
            {
                return;
            }

            foreach(UIElement elem in SudokuGrid.Children)
            {
                TextBox box = elem as TextBox;
                if(box != null)
                {
                    box.Text = "";
                }
            }
        }

        private void Solve(object sender, RoutedEventArgs e)
        {
            UILocked = true;

            SudokuPuzzle.ThePuzzle.Solve();

            UpdateCells();

            UILocked = false;
        }
    }
}
