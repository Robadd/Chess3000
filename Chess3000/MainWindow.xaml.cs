using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
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
using Microsoft.Surface.Presentation.Controls;

namespace Chess3000
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillBoardWithSquares();
            AddNotationLabels();
        }

        private void VisAdded(object sender, TagVisualizerEventArgs e)
        {
            int x, y;
            y = (int)((Control)sender).GetValue(Grid.RowProperty);
            x = (int)((Control)sender).GetValue(Grid.RowProperty);
            player1.Text = "x:" + x + " y:" + y;
        }

        private void VisRemoved(object sender, TagVisualizerEventArgs e)
        {
            player1.Text = "";
        }

        private void FillBoardWithSquares()
        {
            Collection<TagVisualizationDefinition> dc = VisDefinitions(32);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // Black and white rectangles
                    Rectangle rect = new Rectangle();
                    if ((i + j) % 2 == 0) rect.Fill = new SolidColorBrush(Colors.Black);
                    else rect.Fill = new SolidColorBrush(Colors.White);
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    
                    // Tag Visualizer
                    TagVisualizer tv = new TagVisualizer();
                    tv.Height = boardCanvas.ActualHeight / 8;
                    tv.Width = boardCanvas.ActualWidth / 8;
                    tv.SetValue(Grid.RowProperty, i);
                    tv.SetValue(Grid.ColumnProperty, j);
                    tv.VisualizationAdded += VisAdded;
                    tv.VisualizationRemoved += VisRemoved;
                    tv.Definitions.Clear();
                    tv.Definitions.Concat(dc);

                    // Add to view
                    boardCanvas.Children.Add(rect);
                    TVC.Children.Add(tv);
                }
            }
            // Make Border of Chessboard
            Rectangle border = new Rectangle();
            border.SetValue(Grid.RowProperty, 0);
            border.SetValue(Grid.ColumnProperty, 0);
            border.SetValue(Grid.RowSpanProperty, 8);
            border.SetValue(Grid.ColumnSpanProperty, 8);
            border.Stroke = Brushes.Black;
            border.StrokeThickness = 2;
            boardCanvas.Children.Add(border);
        }

        private void AddNotationLabels()
        {
            // A-Z WhitePlayer
            for(int i = 0; i < 8; i++)
            {
                TextBlock label = new TextBlock();
                label.Text = ((char)(65 + i)).ToString();
                label.SetValue(Grid.RowProperty, 7 - i);
                label.SetValue(Grid.ColumnProperty, 0);
                label.TextAlignment = TextAlignment.Center;
                label.TextWrapping = TextWrapping.Wrap;
                label.LayoutTransform = new RotateTransform(-90);
                LabelWhite.Children.Add(label);
            }
            // A-Z BlackPlayer
            for (int i = 0; i < 8; i++)
            {
                TextBlock label = new TextBlock();
                label.Text = ((char)(65 + (7 - i))).ToString();
                label.SetValue(Grid.RowProperty,  i);
                label.SetValue(Grid.ColumnProperty, 0);
                label.TextAlignment = TextAlignment.Center;
                label.TextWrapping = TextWrapping.Wrap;
                label.LayoutTransform = new RotateTransform(90);
                LabelBlack.Children.Add(label);
            }
            // 1-8 Up
            for (int i = 0; i < 8; i++)
            {
                TextBlock label = new TextBlock();
                label.Text = (i+1).ToString();
                label.SetValue(Grid.RowProperty, 0);
                label.SetValue(Grid.ColumnProperty, 7-i);
                label.TextAlignment = TextAlignment.Center;
                label.TextWrapping = TextWrapping.Wrap;
                label.LayoutTransform = new RotateTransform(90);
                LabelRight.Children.Add(label);
            }
            // 1-8 Down
            for (int i = 0; i < 8; i++)
            {
                TextBlock label = new TextBlock();
                label.Text = (i+1).ToString();
                label.SetValue(Grid.RowProperty, 2);
                label.SetValue(Grid.ColumnProperty, 7-i);
                label.TextAlignment = TextAlignment.Center;
                label.TextWrapping = TextWrapping.Wrap;
                label.LayoutTransform = new RotateTransform(-90);
                LabelLeft.Children.Add(label);
            }
        }

        private Collection<TagVisualizationDefinition> VisDefinitions(int count)
        {
            Collection<TagVisualizationDefinition> dc = new Collection<TagVisualizationDefinition>();
            for (int i = 0; i < count; i++)
            {
                TagVisualizationDefinition td = new TagVisualizationDefinition();
                td.Value = i;
                dc.Add(td);
            }
            return dc;
        }
    }
}
