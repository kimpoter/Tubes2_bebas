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

        // Available aa for ARGB color
        static List<string> aa = new() { "20", "42", "64", "90", "a2", "c4", "e6", "ff" };

        // Count visited node
        static List<List<int>> countVisitedNode = new();

        // Map
        List<List<string>> map = new();

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

                    isError = false;
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
                StepsIcon.Visibility = Visibility.Visible;

                string steps;

                // Execution time
                Stopwatch stopwatch = new();

                // Check the algorithm
                if (BFSBtn.IsChecked == true)
                {
                    // do BFS
                    if (BFS_TSPCheckBox.IsChecked == true)
                    {
                        stopwatch.Start();
                        steps = bfs.doBFS(map, true);
                        stopwatch.Stop();
                    }
                    else
                    {
                        stopwatch.Start();
                        steps = bfs.doBFS(map, false);
                        stopwatch.Stop();
                    }
                }
                else if (DFSBtn.IsChecked == true)
                {
                    // do DFS
                    if (DFS_TSPCheckBox.IsChecked == true)
                    {
                        stopwatch.Start();
                        steps = dfs.doDFS(map, true);
                        stopwatch.Stop();
                    }
                    else
                    {
                        stopwatch.Start();
                        steps = dfs.doDFS(map, false);
                        stopwatch.Stop();
                    }

                }
                else
                {
                    // do TSP
                    Algorithms.TravellingSalesman tspAlgo = new();
                    stopwatch.Start();
                    steps = tspAlgo.doTSP(map);
                    stopwatch.Stop();
                }

                Trace.WriteLine(steps);

                Point start = maputils.getStartPoint(map);
                int i = (int)start.X, j = (int)start.Y;
                Trace.WriteLine(start.ToString());

                foreach (var step in steps)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#7dd3fc")!;
                    }, System.Windows.Threading.DispatcherPriority.Background);
                    countVisitedNode[i][j]++;
                    await Task.Delay(sliderValue);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#" + aa[countVisitedNode[i][j] + 1] + "fde047")!;
                    }, System.Windows.Threading.DispatcherPriority.Background);

                    var lB = new Label()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = (Brush)bc.ConvertFrom("#A1A1AA")!,
                        Margin = new Thickness(1, 0, 1, 0),
                        FontSize = 20,
                    };

                    if (step == 'L')
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            lB.Content = "\uf30a;";
                            StepsStP.Children.Add(lB);
                        }, System.Windows.Threading.DispatcherPriority.Background);
                        j--;
                    }
                    else if (step == 'R')
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            lB.Content = "\uf30b;";
                            StepsStP.Children.Add(lB);
                        }, System.Windows.Threading.DispatcherPriority.Background);
                        j++;
                    }
                    else if (step == 'U')
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            lB.Content = "\uf30c;";
                            StepsStP.Children.Add(lB);
                        }, System.Windows.Threading.DispatcherPriority.Background);
                        i--;
                    }
                    else if (step == 'D')
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            lB.Content = "\uf309;";
                            StepsStP.Children.Add(lB);
                        }, System.Windows.Threading.DispatcherPriority.Background);
                        i++;
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#7dd3fc")!;
                }, System.Windows.Threading.DispatcherPriority.Background);
                countVisitedNode[i][j]++;
                await Task.Delay(sliderValue);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ((Border)((StackPanel)stR.Children[i]).Children[j]).Background = (Brush)bc.ConvertFrom("#" + aa[countVisitedNode[i][j] + 1] + "fde047")!;
                }, System.Windows.Threading.DispatcherPriority.Background);

                isSolved = false;
                isVisualized = false;
                DetailsStP.Visibility = Visibility.Visible;
                RuntimeTxt.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";
                StepsCountTxt.Text = steps.Length.ToString();

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
                countVisitedNode.Clear();

                TimeSlider.Visibility = Visibility.Visible;

                TimeTxt.Visibility = Visibility.Visible;
                TimeTxt.Text = (sliderValue / (float)1000).ToString() + " s";

                stR.Children.Clear();
                StepsStP.Children.Clear();
                StepsIcon.Visibility = Visibility.Hidden;
                DetailsStP.Visibility = Visibility.Hidden;

                List<string> lines = File.ReadAllLines(this.fileName).ToList();

                int i = 0;
                int nodes = 1;
                int rows = lines.Count;
                int columns = 0;

                map.Clear();

                foreach (var line in lines)
                {
                    string[] elements = line.Trim().Split(' ');
                    map.Add(elements.ToList());

                    List<int> tempRowCountVisitedNode = new();

                    int j = 0;
                    columns = elements.Length;

                    var stpR = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                    };

                    var size = Math.Max(30, 400 / Math.Max(elements.Length, lines.Count));
                    stpR.Height = size;
                    stR.Children.Insert(i, stpR);

                    foreach (var element in elements)
                    {
                        tempRowCountVisitedNode.Add(0);

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
                    countVisitedNode.Add(tempRowCountVisitedNode);
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

        private void BFSBtn_Click(object sender, RoutedEventArgs e)
        {
            if (BFSBtn.IsChecked == true)
            {
                BFS_TSPWrapper.Visibility = Visibility.Visible;
                DFS_TSPWrapper.Visibility = Visibility.Hidden;
            }
        }

        private void DFSBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DFSBtn.IsChecked == true)
            {
                DFS_TSPWrapper.Visibility = Visibility.Visible;
                BFS_TSPWrapper.Visibility = Visibility.Hidden;
            }
        }

        private void TSPBtn_Click(object sender, RoutedEventArgs e)
        {
            DFS_TSPWrapper.Visibility = Visibility.Hidden;
            BFS_TSPWrapper.Visibility = Visibility.Hidden;
        }
    }
}
