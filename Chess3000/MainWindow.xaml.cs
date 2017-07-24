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
        private Rectangle[,] tiles = new Rectangle[8, 8];
        private Collection<TagVisualizationDefinition> TagDefs = new Collection<TagVisualizationDefinition>();

        public enum BoardState {
            SETUP,
            FINE,
            WRONG_MOVE,
            ADDITIONAL_MOVE,
            CHECK,
            CHECKMATE
        };
        private BoardState state;
        private Image rookImg;
        private Pos moveFrom, moveTo;
        private Piece movingPiece;

        public enum AdditionalMove{
            NONE,
            CASTLING_WHITE_LONG,
            CASTLING_WHITE_SHORT,
            CASTLING_BLACK_LONG,
            CASTLING_BLACK_SHORT
        }
        private AdditionalMove additionalMove;

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
            state = BoardState.SETUP;
            deactivateVisualizer(Color.Black);
        }

        private int AddedPiecesCount()
        {
            int count = 0;
            for (int y = 0; y < 8; y++)
            {
                if (AddedPieces[0, y]) count++;
                if (AddedPieces[1, y]) count++;
                if (AddedPieces[6, y]) count++;
                if (AddedPieces[7, y]) count++;

            }
            return count;
        }

        private void VisAddedSetup(object sender, TagVisualizerEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)((Control)sender).GetValue(Grid.RowProperty);
            xView = (int)((Control)sender).GetValue(Grid.ColumnProperty);
            xMaster = 7 - xView;
            yMaster = 7 - yView;
            AddedPieces[xView, yView] = true;
            var PieceImg = (Image)this.FindName("p"+ xView+yView);
            if(PieceImg != null) PieceImg.Visibility = Visibility.Hidden;
            if(AddedPiecesCount() == 16)
            {
                deactivateVisualizer(Color.White);
            }
            if (AddedPiecesCount() == 32)
            {
                StartBtn.Visibility = Visibility.Visible;
                StartBtn.IsEnabled = true;
            }
            player1.Text = "Tag:" + "x:" + xMaster + " y:" + yMaster;
            player2.Text = "Added Pieces: " + AddedPiecesCount();
        }

        private void VisRemovedSetup(object sender, TagVisualizerEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)((Control)sender).GetValue(Grid.RowProperty);
            xView = (int)((Control)sender).GetValue(Grid.ColumnProperty);
            xMaster = 7 - xView;
            yMaster = 7 - yView;
            AddedPieces[xView, yView] = false;
            var PieceImg = (Image)this.FindName("p" + xView + yView);
            if (PieceImg != null) PieceImg.Visibility = Visibility.Visible;
            if (AddedPiecesCount() != 5)
            {
                StartBtn.Visibility = Visibility.Hidden;
                StartBtn.IsEnabled = true;
            }
            player1.Text = "";
            player2.Text = "Added Pieces: " + AddedPiecesCount();
        }

        private void VisAddedPlay(object sender, TagVisualizerEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)((Control)sender).GetValue(Grid.RowProperty);
            xView = (int)((Control)sender).GetValue(Grid.ColumnProperty);
            xMaster = 7 - xView;
            yMaster = 7 - yView;
            moveTo = new Pos(yMaster, xMaster);
            
        }

        private void VisRemovedPlay(object sender, TagVisualizerEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)((Control)sender).GetValue(Grid.RowProperty);
            xView = (int)((Control)sender).GetValue(Grid.ColumnProperty);
            xMaster = 7 - xView;
            yMaster = 7 - yView;
            moveFrom = new Pos(yMaster, xMaster);
            movingPiece = master.getPiece(moveFrom);
            if (state == BoardState.FINE) // start a new move
            {
                movingPiece = null;
                moveTo = null;
                if(movingPiece.Color == master.Drawing) // right start
                {
                    resetSquares();
                    tiles[xView, yView].Fill = Brushes.LightGreen;
                }
                else
                {
                    state = BoardState.WRONG_MOVE;
                    if( master.Drawing == Color.Black)
                    {

                    }
                    else if(master.Drawing == Color.White)
                    {

                    }
                }
            }
            else if(state == BoardState.ADDITIONAL_MOVE)
            {

            }
            else if(state == BoardState.WRONG_MOVE)
            {

            }
        }

        private void switchTagEvents(bool on)
        {
            if(state == BoardState.SETUP)
            {
                if(on)
                {
                    foreach (TagVisualizer tagvis in tvcollection)
                    {
                        tagvis.VisualizationAdded += VisAddedSetup;
                        tagvis.VisualizationRemoved += VisRemovedSetup;
                    }
                }
                else
                {
                    foreach (TagVisualizer tagvis in tvcollection)
                    {
                        tagvis.VisualizationAdded -= VisAddedSetup;
                        tagvis.VisualizationRemoved -= VisRemovedSetup;
                    }
                }
            }
            else
            {
                if (on)
                {
                    foreach (TagVisualizer tagvis in tvcollection)
                    {
                        tagvis.VisualizationAdded += VisAddedPlay;
                        tagvis.VisualizationRemoved += VisRemovedPlay;
                    }
                }
                else
                {
                    foreach (TagVisualizer tagvis in tvcollection)
                    {
                        tagvis.VisualizationAdded -= VisAddedPlay;
                        tagvis.VisualizationRemoved -= VisRemovedPlay;
                    }
                }
            }
        }

        private void deactivateVisualizer(Color player)
        {
            switchTagEvents(false);
            foreach(TagVisualizer tagvis in tvcollection)
            {
                Pos tagvispos = new Pos(7 - (int)tagvis.GetValue(Grid.ColumnProperty), 7 - (int)tagvis.GetValue(Grid.RowProperty));
                if (master.getPositions(player).Contains(tagvispos))
                {
                    tagvis.IsEnabled = false;
                }
                else
                {
                    tagvis.IsEnabled = true;
                }
                
            }
            switchTagEvents(true);
        }

        private void resetSquares()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0) tiles[i,j].Fill = new SolidColorBrush(Colors.Black);
                    else tiles[i, j].Fill = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void updateView()
        {
            switch (state)
            {
                case BoardState.FINE:
                    if(master.Drawing == Color.Black) player2.Text = player1.Text = "Schwarz ist am Zug";
                    else player2.Text = player1.Text = "Weiß ist am Zug";
                    break;
                
                case BoardState.WRONG_MOVE:

                    break;
                case BoardState.ADDITIONAL_MOVE:
                    Pos rookPos = null;
                    rookImg = null;
                    if(additionalMove == AdditionalMove.CASTLING_WHITE_LONG)
                    {
                        rookPos = master.WHITE_ROOK_LONG_CASTLING_POS;
                        rookImg = p70;
                    }
                    else if(additionalMove == AdditionalMove.CASTLING_WHITE_SHORT)
                    {
                        rookPos = master.WHITE_ROOK_SHORT_CASTLING_POS;
                        rookImg = p70;
                    }
                    else if (additionalMove == AdditionalMove.CASTLING_BLACK_LONG)
                    {
                        rookPos = master.BLACK_ROOK_LONG_CASTLING_POS;
                        rookImg = p00;
                    }
                    else if(additionalMove == AdditionalMove.CASTLING_BLACK_SHORT)
                    {
                        rookPos = master.BLACK_ROOK_SHORT_CASTLING_POS;
                        rookImg = p00;
                    }
                    if(rookImg != null && rookPos != null)
                    {
                        rookImg.SetValue(Grid.RowProperty, 7 - rookPos.x);
                        rookImg.SetValue(Grid.ColumnProperty, 7 - rookPos.y);
                        rookImg.Visibility = Visibility.Visible;
                    }
                    
                    break;
                
                case BoardState.CHECK:

                    break;
                
                case BoardState.CHECKMATE:

                    break;
            }
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            StartBtn.Visibility = Visibility.Hidden;
            foreach (TagVisualizer tv in tvcollection)
            {
                tv.VisualizationAdded -= VisAddedSetup;
                tv.VisualizationRemoved -= VisRemovedSetup;
                tv.VisualizationAdded += VisAddedPlay;
                tv.VisualizationRemoved += VisRemovedPlay;
            }
            state = BoardState.FINE;
            additionalMove = AdditionalMove.NONE;
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
            CreateVisualizerDefinitions();
            
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

                    TagVisualizer tv = CreateVisualizer(i, j, TagDefs);
                    tvcollection.Add(tv);
                    tiles[i, j] = rect;
                    // Add to view
                    boardCanvas.Children.Add(rect);
                    Grid.SetZIndex(rect, 0);
                    boardCanvas.Children.Add(tv);
                    Grid.SetZIndex(tv, 99);
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

        private void CreateVisualizerDefinitions()
        {
            for (int i = 0; i < 16; i++)
            {
                TagVisualizationDefinition td = new TagVisualizationDefinition();
                td.Source = null;
                td.Value = 64 + i;
                td.LostTagTimeout = 500;
                TagDefs.Add(td);
            }
            for (int i = 16; i < 32; i++)
            {
                TagVisualizationDefinition td = new TagVisualizationDefinition();
                td.Source = null;
                td.Value = 64 + i;
                td.LostTagTimeout = 500;
                TagDefs.Add(td);
            }

        }

        private TagVisualizer CreateVisualizer(int x, int y, Collection<TagVisualizationDefinition> dc)
        {
            TagVisualizer tv = new TagVisualizer();
            tv.SetValue(Grid.RowProperty, x);
            tv.SetValue(Grid.ColumnProperty, y);
            tv.VerticalAlignment = VerticalAlignment.Stretch;
            tv.HorizontalAlignment = HorizontalAlignment.Stretch;
            tv.VisualizationAdded += VisAddedSetup;
            tv.VisualizationRemoved += VisRemovedSetup;
            foreach(TagVisualizationDefinition foo in dc)
            {
                tv.Definitions.Add(foo);
            }
            return tv;
        }
    }
}
