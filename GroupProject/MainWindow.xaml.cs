using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtUi_ThreadID);

            lbAlgorithm.Content = "Async - Sequential";

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            int[] mergeSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);
            Array.Copy(_srcArray, mergeSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);
            txtMerge_Result.Text = ArrayToString(mergeSortArray);

            new Thread(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                sw.Start();

                BubbleSortSync(bubbleSortArray);
                SelectionSortSync(selectionSortArray);
                QuickSortSync(quickSortArray);
                ShellSortSync(shellSortArray);
                MergeSortSync(mergeSortArray);

                sw.Stop();
                UpdateTextAsync(threads.Distinct().Count().ToString(), txtThreadUsed);
                UpdateTextAsync(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
            }).Start();

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
            int[] mergeSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);
            Array.Copy(_srcArray, mergeSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);
            txtMerge_Result.Text = ArrayToString(mergeSortArray);

            int uiThread = Thread.CurrentThread.ManagedThreadId; // UI Thread
            threads.Add(uiThread);
            Trace.WriteLine($"UI Thread: {uiThread}");
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtUi_ThreadID);

            new Thread(async () =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                sw = Stopwatch.StartNew();

                await Task.WhenAll(BubbleSortAsync(bubbleSortArray),
                                    SelectionSortAsync(selectionSortArray),
                                    QuickSortAsync(quickSortArray),
                                    ShellSortAsync(shellSortArray),
                                    MergeSortAsync(mergeSortArray));
                sw.Stop();

                UpdateTextAsync(threads.Distinct().Count().ToString(), txtThreadUsed);
                UpdateTextAsync(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
            }).Start();
        }

        private void Sync_Sequential_Click(object sender, RoutedEventArgs e)
        {
            lbAlgorithm.Content = "Sync - Sequential";
            threads.Clear();

            threads.Add(Thread.CurrentThread.ManagedThreadId);
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtUi_ThreadID);

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            int[] mergeSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);
            Array.Copy(_srcArray, mergeSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);
            txtMerge_Result.Text = ArrayToString(mergeSortArray);

            sw.Start();
            BubbleSortSync(bubbleSortArray);
            SelectionSortSync(selectionSortArray);
            QuickSortSync(quickSortArray);
            ShellSortSync(shellSortArray);
            MergeSortSync(mergeSortArray);
            sw.Stop();

            UpdateTextAsync(threads.Distinct().Count().ToString(), txtThreadUsed);
            UpdateTextAsync(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
        }

        private void Sync_Parallel_Click(object sender, RoutedEventArgs e)
        {
            threads.Clear();

            sw.Reset();

            int[] bubbleSortArray = new int[10000];
            int[] selectionSortArray = new int[10000];
            int[] quickSortArray = new int[10000];
            int[] shellSortArray = new int[10000];
            int[] mergeSortArray = new int[10000];
            Array.Copy(_srcArray, bubbleSortArray, _srcArray.Length);
            Array.Copy(_srcArray, selectionSortArray, _srcArray.Length);
            Array.Copy(_srcArray, quickSortArray, _srcArray.Length);
            Array.Copy(_srcArray, shellSortArray, _srcArray.Length);
            Array.Copy(_srcArray, mergeSortArray, _srcArray.Length);

            txtBubble_Result.Text = ArrayToString(bubbleSortArray);
            txtSelection_Result.Text = ArrayToString(selectionSortArray);
            txtSQuick_Result.Text = ArrayToString(quickSortArray);
            txtShell_Result.Text = ArrayToString(shellSortArray);
            txtMerge_Result.Text = ArrayToString(mergeSortArray);

            lbAlgorithm.Content = "Sync - Parallel";

            int uiThread = Thread.CurrentThread.ManagedThreadId;
            threads.Add(uiThread);
            Trace.WriteLine($"UI Thread: {uiThread}");
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtUi_ThreadID);

            Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                sw.Start();

                Parallel.Invoke(() => BubbleSortSync(bubbleSortArray),
                () => SelectionSortSync(selectionSortArray),
                () => QuickSortSync(quickSortArray),
                () => MergeSortSync(mergeSortArray),
                () => ShellSortSync(shellSortArray));


                sw.Stop();

                UpdateTextAsync(threads.Distinct().Count().ToString(), txtThreadUsed);
                UpdateTextAsync(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
            });


        }

        private void UpdateList(int[] arr, TextBlock txtResult)
        {
            Dispatcher.Invoke(() =>
            {
                txtResult.Text = ArrayToString(arr);
            });
        }

        private void UpdateListAsync(int[] arr, TextBlock txtResult)
        {
            Dispatcher.BeginInvoke(() =>
            {
                txtResult.Text = ArrayToString(arr);
            });
        }

        private void UpdateText(string text, TextBlock txt)
        {
            Dispatcher.Invoke(() =>
            {
                txt.Text = text;
            });
        }
        private void UpdateTextAsync(string text, TextBlock txt)
        {
            Dispatcher.BeginInvoke(() =>
            {
                txt.Text = text;
            });
        }

        #region Sort Sync Methods

        private void ShellSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.ShellSort(arr);

            UpdateListAsync(arr, txtShell_Result);
            UpdateTextAsync(result, txtShellSort);
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtShellSort_ThreadID);
        }
        private void MergeSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            var result = SortMethods.MergeSort(arr);

            UpdateListAsync(arr, txtMerge_Result);
            UpdateTextAsync(result, txtMergeSort);
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtMergeSort_ThreadID);


        }
        private void QuickSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.QuickSort(arr);

            UpdateListAsync(arr, txtSQuick_Result);
            UpdateTextAsync(result, txtQuickSort);
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtQuickSort_ThreadID);

        }

        private void BubbleSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.BubbleSort(arr);

            UpdateListAsync(arr, txtBubble_Result);

            UpdateTextAsync(result, txtBubbleSort);
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtBubbleSort_ThreadID);

        }

        private void SelectionSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.SelectionSort(arr);

            UpdateListAsync(arr, txtSelection_Result);
            UpdateTextAsync(result, txtSelectionSort);
            UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtSelectionSort_ThreadID);

        }
        #endregion

        #region Sort Async Methods
        private async Task BubbleSortAsync(int[] arr)
        {
            await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                string result = SortMethods.BubbleSort(arr);

                UpdateListAsync(arr, txtBubble_Result);

                UpdateTextAsync(result, txtBubbleSort);
                UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtBubbleSort_ThreadID);

            });
        }

        private async Task SelectionSortAsync(int[] arr)
        {
            await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                string result = SortMethods.SelectionSort(arr);

                UpdateListAsync(arr, txtSelection_Result);
                UpdateTextAsync(result, txtSelectionSort);
                UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtSelectionSort_ThreadID);

            });
        }
        private async Task QuickSortAsync(int[] arr)
        {
            await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                string result = SortMethods.QuickSort(arr);

                UpdateListAsync(arr, txtSQuick_Result);
                UpdateTextAsync(result, txtQuickSort);
                UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtQuickSort_ThreadID);

            });
        }
        private async Task ShellSortAsync(int[] arr)
        {
            await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                string result = SortMethods.ShellSort(arr);

                UpdateListAsync(arr, txtShell_Result);
                UpdateTextAsync(result, txtShellSort);
                UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtShellSort_ThreadID);

            });
        }
        private async Task MergeSortAsync(int[] arr)
        {
            await Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                var result = SortMethods.MergeSort(arr);

                UpdateListAsync(arr, txtMerge_Result);
                UpdateTextAsync(result, txtMergeSort);
                UpdateTextAsync(Thread.CurrentThread.ManagedThreadId.ToString(), txtMergeSort_ThreadID);

            });
        }
        #endregion


        private string ArrayToString(int[] arr)
        {
            return string.Join(",", arr);
        }
    }


}