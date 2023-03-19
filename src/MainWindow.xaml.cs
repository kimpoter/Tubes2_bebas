using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using src.Algorithms;
using src.Utils;

namespace src
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;

        // Slider value
        static int sliderValue = 500;

        // Visualized tag
        static bool isVisualized = false;

        // Solving flag
        static bool isSolved = false;

        // Error flag
        static bool isError = false;

        // Converting HEX Color
        static readonly BrushConverter bc = new();

        // Filename from user
        string? fileName;

        // Map
        List<List<string>> maps = new();

        public MainWindow()
        {
            InitializeComponent();

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;

            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseMove += OnMouseMove;
        }

        /**
         * 
         * Dragging the app 
         * 
         */
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        /**
         * 
         * Minimize the app
         *  
         */
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /**
         * 
         * Close the app
         * 
         */
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /**
         * 
         * Choose file and temporary get the data also some validation
         * 
         */
        private void ChooseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            VisualizeBtn.IsEnabled = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File|*.txt";

            FileNameLabel.Foreground = (Brush)bc.ConvertFrom("#71717A")!;

            FileNameTxt.Foreground = (Brush)bc.ConvertFrom("#71717A")!;

            if (openFileDialog.ShowDialog() == true)
            {

                FileNameTxt.Text = openFileDialog.SafeFileName;

                fileName = openFileDialog.FileName;

                validate doValidation = new();

                if (doValidation.validateData(fileName))
                {
                    FileNameLabel.Content = "\uf15b;";

                    VisualizeBtn.IsEnabled = true;
                }
                else
                {
                    FileNameLabel.Content = "\ue4eb;";
                    FileNameLabel.Foreground = (Brush)bc.ConvertFrom("#f87171")!;

                    FileNameTxt.Foreground = (Brush)bc.ConvertFrom("#f87171")!;

                    isError = true;
                }
            }
            else
            {
                FileNameLabel.Content = "\ue5a1;";
                FileNameTxt.Text = "No File Choosen";
            }
        }

        private async void SolveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isVisualized && !isSolved)
            {
                isSolved = true;
                VisualizeBtn.IsEnabled = false;
                SolveBtn.IsEnabled = false;

                string steps;

                // Check the algorithm
                if (BFSBtn.IsChecked == true)
                {
                    // do BFS
                    bfs bfsAlgo = new bfs();
                    steps = bfsAlgo.doBFS(maps);
                }
                else if (DFSBtn.IsChecked == true)
                {
                    // do DFS
                    dfs dfsAlgo = new dfs();
                    steps = dfsAlgo.doDFS(maps);
                }
                else
                {
                    // do TSP
                    tsp tspAlgo = new tsp();
                    steps = tspAlgo.doTSP(maps);
                }

                int i = 0, j = 0;

                foreach (var step in steps)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#7dd3fc")!;
                    }, System.Windows.Threading.DispatcherPriority.Background);
                    await Task.Delay(sliderValue);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#fde047")!;
                    }, System.Windows.Threading.DispatcherPriority.Background);
                    //((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#fde047")!;
                    if (step == 'L')
                    {
                        j--;
                    }
                    else if (step == 'R')
                    {
                        j++;
                    }
                    else if (step == 'U')
                    {
                        i--;
                    }
                    else if (step == 'D')
                    {
                        i++;
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#7dd3fc")!;
                }, System.Windows.Threading.DispatcherPriority.Background);
                await Task.Delay(sliderValue);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#fde047")!;
                }, System.Windows.Threading.DispatcherPriority.Background);

                isSolved = false;
                isVisualized = false;
                if (!isError)
                {
                    VisualizeBtn.IsEnabled = true;
                }
            }

        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(stR);
            e.Handled = true;
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y <
                scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                             scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, stR);
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                                         scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow =
                              scrollViewer.TranslatePoint(centerOfViewport, stR);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(stR);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / stR.Width;
                    double multiplicatorY = e.ExtentHeight / stR.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset -
                                        dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset -
                                        dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }

        private void VisualizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (fileName != null && !isSolved)
            {
                TimeSlider.Visibility = Visibility.Visible;

                TimeTxt.Visibility = Visibility.Visible;
                TimeTxt.Text = (sliderValue / (float)1000).ToString() + " s";

                stR.Children.Clear();

                List<string> lines = File.ReadAllLines(this.fileName).ToList();

                int i = 0;
                int nodes = 1;
                int rows = lines.Count;
                int columns = 0;

                foreach (var line in lines)
                {
                    string[] elements = line.Split(' ');
                    maps.Add(elements.ToList());

                    int j = 0;
                    columns = elements.Length;

                    var stpR = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                    };

                    var size = Math.Max(30, 400 / elements.Length);
                    stpR.Height = size;
                    stR.Children.Insert(i, stpR);

                    foreach (var element in elements)
                    {
                        var bc = new BrushConverter();

                        var brdr = new Border()
                        {
                            CornerRadius = new CornerRadius(0.2 * size),
                            Width = size - 5,
                            Height = size - 5,
                            Margin = new Thickness(2),
                        };

                        var lB = new Label()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Foreground = (Brush)bc.ConvertFrom("#71717A")!,
                            FontSize = size / 2.0,
                        };

                        // Check element value
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
                            lB.Content = "\uf81d;";
                            brdr.Background = (Brush)bc.ConvertFrom("#E4E4E7")!;
                            nodes++;
                        }

                        brdr.Child = lB;
                        stpR.Children.Add(brdr);
                    }
                    i++;
                }

                scrollViewer.HorizontalAlignment = HorizontalAlignment.Center;
                MatrixSizeTxt.Text = rows.ToString() + " X " + columns.ToString();
                NodeCountTxt.Text = nodes.ToString() + " nodes";

                isVisualized = true;
                SolveBtn.IsEnabled = true;
            }
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderValue = (int)TimeSlider.Value;
            if (TimeTxt != null)
                TimeTxt.Text = (sliderValue / (float)1000).ToString() + " s";
        }
    }
}
