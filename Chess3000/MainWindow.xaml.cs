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
using System.IO;

namespace Chess3000
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        private ChessMaster master;
        private Rectangle[,] tiles = new Rectangle[8, 8];

        public enum BoardState {
            IDLE,
            MOVE_PENDING,
            CHECK,
            CHECKMATE
        };
        private BoardState state;
        private Pos moveFrom, moveTo;
        private Piece movingPiece;

        public MainWindow()
        {
            master = new ChessMaster();
            InitializeComponent();
            FillBoardWithSquares();
            AddNotationLabels();
            state = BoardState.IDLE;
            updateView();
        }

        private void updateView()
        {
            for (int i = boardCanvas.Children.Count - 1; i >= 0; i--)
            {
                var el = boardCanvas.Children[i];
                if (Grid.GetZIndex(el) == 98)
                {
                    boardCanvas.Children.RemoveAt(i);
                }
            } 

            Piece actPiece = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    actPiece = master.getPiece(new Pos(i, j));
                    if (actPiece != null) {
                        Image PieceImg = new Image();
                        BitmapImage bmpimg = new BitmapImage();
                        switch (actPiece.PieceType)
                        {
                            case PieceType.Bishop:
                                if(actPiece.Color == Color.Black) bmpimg = new BitmapImage(new Uri("/images/laeuferSchwarz.png", UriKind.Relative));
                                else bmpimg = new BitmapImage(new Uri("/images/laeuferWeiss.png", UriKind.Relative));
                                break;
                            case PieceType.King:
                                if (actPiece.Color == Color.Black) bmpimg = new BitmapImage(new Uri("/images/koenigSchwarz.png", UriKind.Relative));
                                else bmpimg = new BitmapImage(new Uri("/images/koenigWeiss.png", UriKind.Relative));
                                break;
                            case PieceType.Knight:
                                if (actPiece.Color == Color.Black) bmpimg = new BitmapImage(new Uri("/images/springerSchwarz.png", UriKind.Relative));
                                else bmpimg = new BitmapImage(new Uri("/images/springerWeiss.png", UriKind.Relative));
                                break;
                            case PieceType.Pawn:
                                if (actPiece.Color == Color.Black) bmpimg = new BitmapImage(new Uri("/images/bauerSchwarz.png", UriKind.Relative));
                                else bmpimg = new BitmapImage(new Uri("/images/bauerWeiss.png", UriKind.Relative));
                                break;
                            case PieceType.Queen:
                                if (actPiece.Color == Color.Black) bmpimg = new BitmapImage(new Uri("/images/dameSchwarz.png", UriKind.Relative));
                                else bmpimg = new BitmapImage(new Uri("/images/dameWeiss.png", UriKind.Relative));
                                break;
                            case PieceType.Rook:
                                if (actPiece.Color == Color.Black) bmpimg = new BitmapImage(new Uri(@"/images/turmSchwarz.png", UriKind.Relative));
                                else bmpimg = new BitmapImage(new Uri(@"images\turmWeiss.png", UriKind.Relative));
                                break;
                        }
                        PieceImg.Source = bmpimg;
                        PieceImg.Stretch = Stretch.Fill;
                        PieceImg.SetValue(Grid.RowProperty, 7 - j);
                        PieceImg.SetValue(Grid.ColumnProperty, 7 - i);
                        boardCanvas.Children.Add(PieceImg);
                        Grid.SetZIndex(PieceImg, 98);
                    }
                }
            }
        }

        /*
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
        */
      
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
        /*
        private void updateViewOld()
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
        */

        

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
           // CreateVisualizerDefinitions();
            
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

                    Rectangle TouchRect = new Rectangle();
                    TouchRect.Fill = Brushes.Transparent;
                    TouchRect.SetValue(Grid.RowProperty, i);
                    TouchRect.SetValue(Grid.ColumnProperty, j);
                    TouchRect.TouchDown += TouchEvent;
                    boardCanvas.Children.Add(TouchRect);
                    Grid.SetZIndex(TouchRect, 99);
                    /*
                    TagVisualizer tv = CreateVisualizer(i, j, TagDefs);
                    tvcollection.Add(tv);
                    boardCanvas.Children.Add(tv);
                    Grid.SetZIndex(tv, 99);
                    */
                    tiles[i, j] = rect;
                    // Add to view
                    boardCanvas.Children.Add(rect);
                    Grid.SetZIndex(rect, 0);
                    
                }
            }
            // Make Border of Chessboard
            
        }

        private void TouchEvent(object sender, TouchEventArgs e)
        {
            int xMaster, yMaster, xView, yView;
            yView = (int)(sender as Rectangle).GetValue(Grid.RowProperty);
            xView = (int)(sender as Rectangle).GetValue(Grid.ColumnProperty);
            xMaster = 7 - yView;
            yMaster = 7 - xView;
            player1.Text = "";
            player2.Text = "";
            if(state == BoardState.IDLE)
            {
                moveFrom = new Pos(yMaster, xMaster);
                state = BoardState.MOVE_PENDING;
                //resetSquares();
                tiles[yView, xView].Fill = Brushes.Orange;
            }
            else if(state == BoardState.MOVE_PENDING)
            {
                TextBlock msgBox = null;
                if (master.Drawing == Color.White) msgBox = player1;
                else if (master.Drawing == Color.Black) msgBox = player2;
                moveTo = new Pos(yMaster, xMaster);
                if(moveFrom.Equals(moveTo))
                {
                    state = BoardState.IDLE;
                    //resetSquares();
                    if ((moveFrom.x + moveFrom.y) % 2 == 0) tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                    else tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                    return;
                }
                Result res = master.move(moveFrom, moveTo);
                switch (res)
                {
                    case Result.SUCCESS:
                        updateView();
                        resetSquares();
                        tiles[7 - master.LastFrom.x, 7 - master.LastFrom.y].Fill = Brushes.Green;
                        tiles[7 - master.LastTo.x, 7 - master.LastTo.y].Fill = Brushes.Green;
                        state = BoardState.IDLE;
                        break; 
                    case Result.ERROR_CHECK:
                        msgBox.Text = "Du stehst im Schach!";
                        if ((moveFrom.x + moveFrom.y) % 2 == 0) tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                        else tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                        state = BoardState.IDLE;
                        break;
                    case Result.ERROR_INVALID_DES:
                        msgBox.Text = "Ungültiges Zielfeld!";
                        if ((moveFrom.x + moveFrom.y) % 2 == 0) tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                        else tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                        state = BoardState.IDLE;
                        break;
                    case Result.ERROR_NULL_PIECE:
                        msgBox.Text = "Wähle zuerst eine Figur aus!";
                        if ((moveFrom.x + moveFrom.y) % 2 == 0) tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                        else tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                        state = BoardState.IDLE;
                        break;
                    case Result.ERROR_WRONG_COLOR:
                        msgBox.Text = "Diese Figur gehört dem Gegner!";
                        if ((moveFrom.x + moveFrom.y) % 2 == 0) tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                        else tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                        state = BoardState.IDLE;
                        break;
                    default:
                        msgBox.Text = "ARMAGEDDON !!!!!!";
                        if ((moveFrom.x + moveFrom.y) % 2 == 0) tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                        else tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                        state = BoardState.IDLE;
                        break;
                }
            }
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


/*
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
        */
    }  
}
