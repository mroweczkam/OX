using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using OX.Model;

namespace OX.VM
{
    class createGrid
    {
        public static Grid drawGrid(int size, int[][] board, Position position1,Position position2 )
        {

            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.ShowGridLines = true;

            for (int i = 0; i < size; i++)
            {
                ColumnDefinition columnDefinition_i = new ColumnDefinition();
                columnDefinition_i.Width = new GridLength(1, GridUnitType.Star);

                grid.ColumnDefinitions.Add(columnDefinition_i);
            }

            for (int i = 0; i < size; i++)
            {
                RowDefinition rowDefinition_i = new RowDefinition();
                rowDefinition_i.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDefinition_i);
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Rectangle rec = new Rectangle();
                    Grid.SetRow(rec, i);
                    Grid.SetColumn(rec, j);
                    rec.StrokeThickness = 3.0; 

                    if (position1.x == i && position1.y == j)
                    {
                        rec.Stroke = Brushes.Red;
                    }

                    if (position2.x == i && position2.y == j)
                    {
                        rec.Stroke = Brushes.Blue;
                    }
                   
                    grid.Children.Add(rec);

                    StackPanel stcPanel = new StackPanel();
                    stcPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    stcPanel.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetRow(stcPanel, i);
                    Grid.SetColumn(stcPanel, j);

                    TextBlock txtBlock = new TextBlock();
                    txtBlock.Visibility = Visibility.Visible;
                   // txtBlock.Text = board[i][j].ToString();
                    txtBlock.FontSize = 30.00;

                    stcPanel.Children.Add(txtBlock);
                    grid.Children.Add(stcPanel);
                }
            }
            return grid;
        }




    }
}
