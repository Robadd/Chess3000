using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Surface.Presentation.Controls;

namespace Chess3000
{
    public partial class MainWindow : SurfaceWindow
    {
        private ChessMaster master;
        private Rectangle[,] tiles = new Rectangle[8, 8];
        private enum BoardState {
            IDLE,
            MOVE_PENDING,
            CHECK,
            CHECKMATE
        };
        private BoardState state;
        private Pos moveFrom, moveTo;

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
                   
                    tiles[i, j] = rect;
                    // Add to view
                    boardCanvas.Children.Add(rect);
                    Grid.SetZIndex(rect, 0);
                }
            }
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
                    bodgeBlackWhiteReset(moveFrom);
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
                        bodgeBlackWhiteReset(moveFrom);
                        state = BoardState.IDLE;
                        break;
                    case Result.ERROR_INVALID_DES:
                        msgBox.Text = "Ungültiges Zielfeld!";
                        bodgeBlackWhiteReset(moveFrom);
                        state = BoardState.IDLE;
                        break;
                    case Result.ERROR_NULL_PIECE:
                        msgBox.Text = "Wähle zuerst eine Figur aus!";
                        bodgeBlackWhiteReset(moveFrom);
                        state = BoardState.IDLE;
                        break;
                    case Result.ERROR_WRONG_COLOR:
                        msgBox.Text = "Diese Figur gehört dem Gegner!";
                        bodgeBlackWhiteReset(moveFrom);
                        state = BoardState.IDLE;
                        break;
                    default:
                        msgBox.Text = "ARMAGEDDON !!!!!!";
                        bodgeBlackWhiteReset(moveFrom);
                        state = BoardState.IDLE;
                        break;
                }
            }
        }

        private void bodgeBlackWhiteReset(Pos moveFrom)
        {
            if (moveFrom.Equals(master.LastFrom) || moveFrom.Equals(master.LastTo))
            {
                tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Green;
            }
            else
            {
                if ((moveFrom.x + moveFrom.y) % 2 == 0)
                {
                    tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.Black;
                }
                else
                {
                    tiles[7 - moveFrom.x, 7 - moveFrom.y].Fill = Brushes.White;
                }
            }
        }

        private bool Sure()
        {
            return MessageBox.Show("Sind Sie sicher?", "Sicher?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private void Reset(object sender, TouchEventArgs e)
        {
            if (Sure())
            {
                master.reset();
                resetSquares();
                updateView();
                state = BoardState.IDLE;
                AufgabeBlack.IsEnabled = true;
                AufgabeWhite.IsEnabled = true;
                player1.Text = "";
                player2.Text = "";
            }
            else return;
        }

        private void GiveUpWhite(object sender, TouchEventArgs e)
        {
            if (Sure())
            {
                state = BoardState.CHECKMATE;
                AufgabeBlack.IsEnabled = false;
                AufgabeWhite.IsEnabled = false;
                player2.Text = "Glückwunsch! Sie haben gewonnen.";
            }
            else return;
        }

        private void GiveUpBlack(object sender, TouchEventArgs e)
        {
            if (Sure())
            {
                state = BoardState.CHECKMATE;
                AufgabeBlack.IsEnabled = false;
                AufgabeWhite.IsEnabled = false;
                player1.Text = "Glückwunsch! Sie haben gewonnen.";
            }
            else return;
        }

        private TextBlock MakeNotationLabel(int rotate, int row, int coloumn, string text)
        {
            TextBlock label = new TextBlock();
            label.Text = text;
            label.Foreground = Brushes.Black;
            label.SetValue(Grid.RowProperty, row);
            label.SetValue(Grid.ColumnProperty, coloumn);
            label.TextAlignment = TextAlignment.Center;
            label.TextWrapping = TextWrapping.Wrap;
            label.LayoutTransform = new RotateTransform(rotate);
            return label;
        }

        private void AddNotationLabels()
        {
            // A-Z WhitePlayer
            for(int i = 0; i < 8; i++)
            {
                LabelWhite.Children.Add(MakeNotationLabel(-90, 7 - i, 0, ((char)(65 + i)).ToString()));
            }
            // A-Z BlackPlayer
            for (int i = 0; i < 8; i++)
            {
                LabelBlack.Children.Add(MakeNotationLabel(90, i, 0, ((char)(65 + (7 - i))).ToString()));
            }
            // 1-8 Up
            for (int i = 0; i < 8; i++)
            {
                LabelRight.Children.Add(MakeNotationLabel(90, 0, 7 - i, (i + 1).ToString()));
            }
            // 1-8 Down
            for (int i = 0; i < 8; i++)
            {
                LabelLeft.Children.Add(MakeNotationLabel(-90, 2, 7 - i, (i + 1).ToString()));
            }
        }
    }  
}