using System;
using System.Collections.Concurrent;
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

        private async void Async_Sequential_Click(object sender, RoutedEventArgs e)
        {
            threads.Clear();

            threads.Add(Thread.CurrentThread.ManagedThreadId);

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


            await Task.Run(() =>
            {
                sw = Stopwatch.StartNew();

                BubbleSortSync(bubbleSortArray);
                SelectionSortSync(selectionSortArray);
                QuickSortSync(quickSortArray);
                ShellSortSync(shellSortArray);
                MergeSortSync(mergeSortArray);

                sw.Stop();

                UpdateText(threads.Distinct().Count().ToString(), txtThreadUsed);
                UpdateText(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
            });

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

            var bubbleSortTask = BubbleSortAsync(bubbleSortArray);
            var selectionSortTask = SelectionSortAsync(selectionSortArray);
            var quickSortTask = QuickSortAsync(quickSortArray);
            var shellSortTask = ShellSortAsync(shellSortArray);
            var mergeSortTask = MergeSortAsync(mergeSortArray);

            Task.Run(() =>
            {
                threads.Add(Thread.CurrentThread.ManagedThreadId);

                sw.Start();
                Task.WhenAll(bubbleSortTask, selectionSortTask, quickSortTask, shellSortTask, mergeSortTask);
                sw.Stop();

                UpdateText(threads.Distinct().Count().ToString(), txtThreadUsed);
                UpdateText(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
            });



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

            UpdateText(threads.Distinct().Count().ToString(), txtThreadUsed);
            UpdateText(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
        }

        private async void Sync_Parallel_Click(object sender, RoutedEventArgs e)
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


            await Task.Run(() =>
            {
                sw.Start();

                Parallel.Invoke(() => BubbleSortSync(bubbleSortArray),
                    () => SelectionSortSync(selectionSortArray),
                    () => QuickSortSync(quickSortArray),
                    () => MergeSortSync(mergeSortArray),
                    () => ShellSortSync(shellSortArray));
                sw.Stop();

                UpdateText(threads.Distinct().Count().ToString(), txtThreadUsed);
                UpdateText(sw.ElapsedMilliseconds.ToString() + "ms", txtTotal);
            });


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
            Dispatcher.BeginInvoke(() =>
            {
                txtResult.Text = ArrayToString(arr);
            });
        }

        private void UpdateText(string text, TextBlock txt)
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

            UpdateList(arr, txtShell_Result);
            UpdateText(result, txtShellSort);
        }
        private void MergeSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            var result = SortMethods.MergeSort(arr);

            UpdateList(arr, txtMerge_Result);
            UpdateText(result, txtMergeSort);

        }
        private void QuickSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.QuickSort(arr);

            UpdateList(arr, txtSQuick_Result);
            UpdateText(result, txtQuickSort);
        }

        private void BubbleSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.BubbleSort(arr);

            UpdateList(arr, txtBubble_Result);

            UpdateText(result, txtBubbleSort);
        }

        private void SelectionSortSync(int[] arr)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);

            string result = SortMethods.SelectionSort(arr);

            UpdateList(arr, txtSelection_Result);
            UpdateText(result, txtSelectionSort);
        }
        #endregion

        #region Sort Async Methods
        private async Task BubbleSortAsync(int[] arr)
        {

            await Task.Run(() => BubbleSortSync(arr));
        }

        private async Task SelectionSortAsync(int[] arr)
        {

            await Task.Run(() => SelectionSortSync(arr));
        }
        private async Task QuickSortAsync(int[] arr)
        {

            await Task.Run(() => QuickSortSync(arr));
        }
        private async Task ShellSortAsync(int[] arr)
        {

            await Task.Run(() => ShellSortSync(arr));
        }
        private async Task MergeSortAsync(int[] arr)
        {

            await Task.Run(() => MergeSortSync(arr));
        }
        #endregion


        private string ArrayToString(int[] arr)
        {
            return string.Join(",", arr);
        }
    }


}