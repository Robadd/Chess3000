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
    public partial class MainWindow : SurfaceWindow
    {
        public Collection<TagVisualizer> tvcollection = new Collection<TagVisualizer>();
        private ChessMaster master;
        private bool[,] AddedPieces = new bool[8,8];

        public MainWindow()
        {
            master = new ChessMaster();
            InitializeComponent();
            FillBoardWithSquares();
            AddNotationLabels();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    AddedPieces[x, y] = false;
                }
            }
        }



        private int AddedPiecesCount()
        {
            int count = 0;
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    if (AddedPieces[x, y]) count++;
                }
            }
            return count;
        }

        private void VisAdded(object sender, TagVisualizerEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)((Control)sender).GetValue(Grid.RowProperty);
            xView = (int)((Control)sender).GetValue(Grid.ColumnProperty);
            xMaster = 7 - xView;
            yMaster = 7 - yView;
            AddedPieces[xView, yView] = true;
            if (AddedPiecesCount() == 32)
            {
                Grid.SetZIndex(StartBtn, 99);
                StartBtn.Visibility = Visibility.Visible;
                StartBtn.IsEnabled = true;
            }
            player1.Text = "Tag:" + "x:" + xMaster + " y:" + yMaster;
            player2.Text = "Added Pieces: " + AddedPiecesCount();
        }

        private void VisRemoved(object sender, TagVisualizerEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)((Control)sender).GetValue(Grid.RowProperty);
            xView = (int)((Control)sender).GetValue(Grid.ColumnProperty);
            xMaster = 7 - xView;
            yMaster = 7 - yView;
            AddedPieces[xView, yView] = true;
            player1.Text = "";
            player2.Text = "Added Pieces: " + AddedPiecesCount();

        }

        private void FillBoardWithSquares()
        {
            Rectangle border = new Rectangle();
            border.SetValue(Grid.RowProperty, 0);
            border.SetValue(Grid.ColumnProperty, 0);
            border.SetValue(Grid.RowSpanProperty, 8);
            border.SetValue(Grid.ColumnSpanProperty, 8);
            border.Stroke = Brushes.Black;
            border.StrokeThickness = 2;
            boardCanvas.Children.Add(border);
            Collection<TagVisualizationDefinition> dc = CreateVisualizerDefinitions(32);
            
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

                    TagVisualizer tv = CreateVisualizer(i, j, dc);
                    tvcollection.Add(tv);
                    // Add to view
                    boardCanvas.Children.Add(rect);
                    boardCanvas.Children.Add(tv);
                }
            }
            // Make Border of Chessboard
            
        }

        private void AddNotationLabels()
        {
            // A-Z WhitePlayer
            for(int i = 0; i < 8; i++)
            {
                TextBlock label = new TextBlock();
                label.Text = ((char)(65 + i)).ToString();
                label.Foreground = Brushes.Black;
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
                label.Foreground = Brushes.Black;
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
                label.Foreground = Brushes.Black;
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
                label.Foreground = Brushes.Black;
                label.SetValue(Grid.RowProperty, 2);
                label.SetValue(Grid.ColumnProperty, 7-i);
                label.TextAlignment = TextAlignment.Center;
                label.TextWrapping = TextWrapping.Wrap;
                label.LayoutTransform = new RotateTransform(-90);
                LabelLeft.Children.Add(label);
            }
        }

        private Collection<TagVisualizationDefinition> CreateVisualizerDefinitions(int count)
        {
            Collection<TagVisualizationDefinition> dc = new Collection<TagVisualizationDefinition>();
            for (int i = 0; i < count; i++)
            {
                TagVisualizationDefinition td = new TagVisualizationDefinition();
                td.Value = i;
                td.LostTagTimeout = 200;
                dc.Add(td);
            }
            return dc;
        }

        private TagVisualizer CreateVisualizer(int x, int y, Collection<TagVisualizationDefinition> dc)
        {
            TagVisualizer tv = new TagVisualizer();
            tv.SetValue(Grid.RowProperty, x);
            tv.SetValue(Grid.ColumnProperty, y);
            tv.VerticalAlignment = VerticalAlignment.Stretch;
            tv.HorizontalAlignment = HorizontalAlignment.Stretch;
            tv.VisualizationAdded += VisAdded;
            tv.VisualizationRemoved += VisRemoved;
            foreach(TagVisualizationDefinition foo in dc)
            {
                tv.Definitions.Add(foo);
            }
            return tv;
        }

        private void Board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void TagVisualizer_VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            return;
        }
    }
}
