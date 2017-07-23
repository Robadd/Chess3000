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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess3000
{
    /// <summary>
    /// Interaktionslogik für TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        Chess3000.ChessMaster chessMaster;

        public TestWindow()
        {
            InitializeComponent();
            chessMaster = new ChessMaster();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int y = 0; y <= 7; y++)
            {
                for (int x = 0; x <= 7; x++)
                {
                    Pos pos = new Pos(y, x);
                    FromComboBox.Items.Add(pos);
                    ToComboBox.Items.Add(pos);
                }
            }
            FromComboBox.SelectedIndex = ToComboBox.SelectedIndex = 0;

            showDrawingPlayer();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            // Pos from = new Pos(-1, -1);
            // Pos to = new Pos(-1, -1);



            Result res = chessMaster.move(
                FromComboBox.Items[FromComboBox.SelectedIndex] as Chess3000.Pos,
                ToComboBox.Items[ToComboBox.SelectedIndex] as Chess3000.Pos
                );
            if (res != Result.SUCCESS)
            {
                switch (res)
                {
                    case Result.ERROR_CHECK:
                        MessageBox.Show("ERROR_CHECK");
                        break;
                    case Result.ERROR_CHECK_UNHANDLED:
                        MessageBox.Show("ERROR_CHECK_UNHANDLED");
                        break;
                    case Result.ERROR_INVALID_DES:
                        MessageBox.Show("ERROR_INVALID_DES");
                        break;
                    case Result.ERROR_NULL_PIECE:
                        MessageBox.Show("ERROR_NULL_PIECE");
                        break;
                    case Result.ERROR_WRONG_COLOR:
                        MessageBox.Show("ERROR_WRONG_COLOR");
                        break;
                    default:
                        MessageBox.Show("UNSPECIFIED ERROR RETURNED");
                        break;
                }
            }

            showDrawingPlayer();
            showLastDraw();
        }

        private void showDrawingPlayer()
        {
            if (chessMaster == null) { return; }
            if (chessMaster.Drawing == Farbe.WEISS)
            {
                DrawingRect.Fill = new SolidColorBrush(Colors.White);
            }
            else
            {
                DrawingRect.Fill = new SolidColorBrush(Colors.Black);
            }
        }

        private void showLastDraw()
        {
            if (chessMaster.LastFrom != null)
            {
                LastFromTextBlock.Text = chessMaster.LastFrom.ToString();
            }
            else
            {
                LastFromTextBlock.Text = "";
            }

            if (chessMaster.LastTo != null)
            {
                LastToTextBlock.Text = chessMaster.LastTo.ToString();
            }
            else
            {
                LastToTextBlock.Text = "";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            chessMaster.reset();
            showDrawingPlayer();
            showLastDraw();
        }
    }
}