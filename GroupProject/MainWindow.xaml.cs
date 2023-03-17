using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace GroupProject
{

    public partial class MainWindow : Window
    {
        private readonly int[] _srcArray = new int[10000];
        private Stopwatch sw = new Stopwatch();
        private ConcurrentBag<int> threads = new ConcurrentBag<int>();


        public MainWindow()
        {
            InitializeComponent();
            _srcArray = AutoGen();
        }

        private int[] AutoGen()
        {
            int[] autoGen = new int[10000];
            Random random = new Random();
            for (int i = 0; i < autoGen.Length; i++)
            {
                autoGen[i] = random.Next(10000);
            }
            return autoGen;
        }        

        private void Async_Sequential_Click(object sender, RoutedEventArgs e)
        {
            threads.Clear();

            threads.Add(Thread.CurrentThread.ManagedThreadId);

            lbAlgorithm.Content = "Async - Sequential";

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);

            string bubbleSortResult = "", selectionSortResult = "", quickSortResult = "", shellSortResult = "";


            Thread sortThread = new Thread(() =>
            {
                sw.Start();

                bubbleSortResult = BubbleSortSync(bubbleSortArray);
                selectionSortResult = SelectionSortSync(selectionSortArray);
                quickSortResult = QuickSortSync(quickSortArray);
                shellSortResult = ShellSortSync(shellSortArray);

                sw.Stop();

                Dispatcher.Invoke(() =>
                {
                    txtBubbleSort.Text = bubbleSortResult;
                    txtSelectionSort.Text = selectionSortResult;
                    txtQuickSort.Text = quickSortResult;
                    txtShellSort.Text = shellSortResult;
                    txtTotal.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                    txtThreadUsed.Text = threads.Distinct().Count().ToString();

                });
            });


            sortThread.Start();

            
        }

        private void Async_Parallel_Click(object sender, RoutedEventArgs e)
        {
            threads.Clear();
            lbAlgorithm.Content = "Async - Parallel";

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);

            int uiThread = Thread.CurrentThread.ManagedThreadId;
            threads.Add(uiThread);
            Trace.WriteLine($"UI Thread: {uiThread}");

            var bubbleSortTask = BubbleSortAsync(bubbleSortArray);
            var selectionSortTask = SelectionSortAsync(selectionSortArray);
            var quickSortTask = QuickSortAsync(quickSortArray);
            var shellSortTask = ShellSortAsync(shellSortArray);

            Thread sortThread = new Thread(async () =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);
                sw.Start();
                var result = await Task.WhenAll(bubbleSortTask, selectionSortTask, quickSortTask, shellSortTask);
                sw.Stop();
                Dispatcher.Invoke(() =>
                {
                    txtBubbleSort.Text = result[0];
                    txtSelectionSort.Text = result[1];
                    txtQuickSort.Text = result[2];
                    txtShellSort.Text = result[3];
                    txtThreadUsed.Text = threads.Distinct().Count().ToString();
                });
            });

            sortThread.Start();


        }

        private void Sync_Sequential_Click(object sender, RoutedEventArgs e)
        {
            lbAlgorithm.Content = "Sync - Sequential";
            threads.Clear();

            threads.Add(Thread.CurrentThread.ManagedThreadId);

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);

            string bubbleSortResult = "", selectionSortResult = "", quickSortResult = "", shellSortResult = "";

            sw.Start();

            bubbleSortResult = BubbleSortSync(bubbleSortArray);
            selectionSortResult = SelectionSortSync(selectionSortArray);
            quickSortResult = QuickSortSync(quickSortArray);
            shellSortResult = ShellSortSync(shellSortArray);
            sw.Stop();

            Dispatcher.Invoke(() =>
            {
                txtBubbleSort.Text = bubbleSortResult;
                txtSelectionSort.Text = selectionSortResult;
                txtQuickSort.Text = quickSortResult;
                txtShellSort.Text = shellSortResult;
                txtTotal.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                txtThreadUsed.Text = threads.Distinct().Count().ToString();
            });
        }

        private void Sync_Parallel_Click(object sender, RoutedEventArgs e)
        {
            threads.Clear();

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);

            string bubbleSortResult = "", selectionSortResult = "", quickSortResult = "", shellSortResult = "";

            lbAlgorithm.Content = "Sync - Parallel";

            int uiThread = Thread.CurrentThread.ManagedThreadId;
            threads.Add(uiThread);
            Trace.WriteLine($"UI Thread: {uiThread}");


            Thread sortThread = new Thread(() =>
            {
                sw.Start();
                Parallel.Invoke(() => bubbleSortResult = BubbleSortSync(bubbleSortArray),
                () => selectionSortResult = SelectionSortSync(selectionSortArray),
                () => quickSortResult = QuickSortSync(quickSortArray),
                () => shellSortResult = ShellSortSync(shellSortArray));
                sw.Stop();
                Dispatcher.Invoke(() =>
                {
                    txtBubbleSort.Text = bubbleSortResult;
                    txtSelectionSort.Text = selectionSortResult;
                    txtQuickSort.Text = quickSortResult;
                    txtShellSort.Text = shellSortResult;
                    txtTotal.Text = sw.ElapsedMilliseconds.ToString() + "ms";

                    txtThreadUsed.Text = threads.Distinct().Count().ToString();
                });
            });

            sortThread.Start();

        }

        private void UpdateResult(ref ConcurrentBag<int> threads, TextBlock textBlock, TextBlock threadTextBlock, string result)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            threads.Add(threadId);
            Dispatcher.Invoke(() =>
            {
                threadTextBlock.Text = threadId.ToString();
                textBlock.Text = result;
            });
        }

        private void UpdateList(int[] arr, TextBlock txtResult)
        {
            Dispatcher.Invoke(() => 
            {
                txtResult.Text = ArrayToString(arr);
            });
        }

        #region Sort Sync Methods

        private string ShellSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            Stopwatch sw = Stopwatch.StartNew();
            SortMethods.ShellSort(arr);
            sw.Stop();

            UpdateList(arr, txtShell_Result);
            return sw.ElapsedMilliseconds.ToString() + "ms";
        }
        private string QuickSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            Stopwatch sw = Stopwatch.StartNew();
            SortMethods.QuickSort(arr);
            sw.Stop();

            UpdateList(arr, txtSQuick_Result);
            return sw.ElapsedMilliseconds.ToString() + "ms";
        }

        private string BubbleSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            Stopwatch sw = Stopwatch.StartNew();
            SortMethods.BubbleSort(arr);
            sw.Stop();

            UpdateList(arr, txtBubble_Result);
            return sw.ElapsedMilliseconds.ToString() + "ms";
        }

        private string SelectionSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            Stopwatch sw = Stopwatch.StartNew();
            SortMethods.SelectionSort(arr);
            sw.Stop();

            UpdateList(arr, txtSelection_Result);
            return sw.ElapsedMilliseconds.ToString() + "ms";
        }
        #endregion

        #region Sort Async Methods
        private async Task<string> BubbleSortAsync(int[] arr)
        {

            return await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                sw.Restart();

                SortMethods.BubbleSort(arr);
                sw.Stop();

                UpdateList(arr, txtBubble_Result);

                return sw.ElapsedMilliseconds.ToString() + "ms";

            });
        }

        private async Task<string> SelectionSortAsync(int[] arr)
        {

            return await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                sw.Restart();
                SortMethods.SelectionSort(arr);
                sw.Stop();

                UpdateList(arr, txtSelection_Result);

                Dispatcher.Invoke(() =>
                {
                    txtSelectionSort.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                });
                return sw.ElapsedMilliseconds.ToString() + "ms";

            });
        }
        private async Task<string> QuickSortAsync(int[] arr)
        {

            return await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                Stopwatch sw = Stopwatch.StartNew();
                SortMethods.QuickSort(arr);
                sw.Stop();

                UpdateList(arr, txtSQuick_Result);

                Dispatcher.Invoke(() =>
                {
                    txtQuickSort.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                });
                return sw.ElapsedMilliseconds.ToString() + "ms";
            });
        }
        private async Task<string> ShellSortAsync(int[] arr)
        {

            return await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                Stopwatch sw = Stopwatch.StartNew();
                SortMethods.ShellSort(arr);
                sw.Stop();

                UpdateList(arr, txtShell_Result);

                Dispatcher.Invoke(() =>
                {
                    txtShellSort.Text = sw.ElapsedMilliseconds.ToString() + "ms";
                });
                return sw.ElapsedMilliseconds.ToString() + "ms";
            });
        }
        #endregion


        private string ArrayToString(int[] arr)
        {
            return string.Join(",", arr);
        }
    }


}