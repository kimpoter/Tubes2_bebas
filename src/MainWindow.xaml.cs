using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace src
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChooseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                stR.Children.Clear();
                stR.HorizontalAlignment = HorizontalAlignment.Center;

                FileNameLabel.Content = "\uf15b;";
                FileNameTxt.Text = openFileDialog.SafeFileName;

                List<string> lines = File.ReadAllLines(openFileDialog.FileName).ToList();

                int i = 0;
                int nodes = 1;
                foreach (var line in lines)
                {
                    string[] elements = line.Split(' ');
                    int j = 0;
                    var stpR = new StackPanel()
                    {
                        Width = 400,
                        Orientation = Orientation.Horizontal,
                    };
                    var size = (stpR.Width - 4 * elements.Length) / elements.Length;
                    stpR.Height = size;
                    stR.Children.Insert(i, stpR);
                    foreach (var element in elements)
                    {
                        var bc = new BrushConverter();
                        var brdr = new Border()
                        {
                            CornerRadius = new CornerRadius(10, 10, 10, 10),
                            Width = size - 4,
                            Height = size - 4,
                            Margin = new Thickness(2),
                        };
                        var lB = new Label()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Foreground = (Brush)bc.ConvertFrom("#71717A")!,
                            FontSize = size / 2.0,
                        };
                        if (element == "K")
                        {
                            lB.Content = "\uf805;";
                            brdr.Background = (Brush)bc.ConvertFrom("#E4E4E7")!;
                        }
                        else if (element == "R")
                        {
                            brdr.Background = (Brush)bc.ConvertFrom("#E4E4E7")!;
                            nodes++;
                        }
                        else if (element == "X")
                        {
                            brdr.Background = (Brush)bc.ConvertFrom("#52525b")!;
                        }
                        else if (element == "T")
                        {
                            lB.Content = "\uf3a5;";
                            brdr.Background = (Brush)bc.ConvertFrom("#E4E4E7")!;
                            nodes++;
                        }

                        brdr.Child = lB;
                        stpR.Children.Add(brdr);
                    }
                    i++;
                }
                NodeCountTxt.Text = nodes.ToString() + " nodes";
            }
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            stR.HorizontalAlignment = HorizontalAlignment.Left;
        }
    }
}
