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
            Collection<TagVisualizationDefinition> dc =  new Collection<TagVisualizationDefinition>();
            for (int i = 0; i < 32; i++)
            {
                TagVisualizationDefinition td = new TagVisualizationDefinition();
                td.Value = i;
                dc.Add(td);
            }
            
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    // Black and white rectangles
                    Rectangle rect = new Rectangle();
                    if ((i + j) % 2 == 0) rect.Fill = new SolidColorBrush(Colors.Black);
                    else rect.Fill = new SolidColorBrush(Colors.White);
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    boardCanvas.Children.Add(rect);
                    // Tag Visualizer
                    TagVisualizer tv = new TagVisualizer();
                    tv.SetValue(Grid.RowProperty, i);
                    tv.SetValue(Grid.ColumnProperty, j);
                    tv.VisualizationAdded += VisAdded;
                    tv.VisualizationRemoved += VisRemoved;
                    tv.Definitions.Clear();
                    tv.Definitions.Concat(dc);
                    boardCanvas.Children.Add(tv);
                }
            }  
        }

        private void VisAdded(object sender, TagVisualizerEventArgs e)
        {
            int x, y;
            y = (int)((Control)sender).GetValue(Grid.RowProperty);
            x = (int)((Control)sender).GetValue(Grid.RowProperty);
            textblock.Text = "x:" + x + " y:" + y;
        }

        private void VisRemoved(object sender, TagVisualizerEventArgs e)
        {
            textblock.Text = "";
        }
    }
}
