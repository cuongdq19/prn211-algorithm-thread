using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GroupProject
{

    public partial class MainWindow : Window
    {
        public AsyncSort asyncSort = new AsyncSort();
        public MainWindow()
        {
            InitializeComponent();
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
        private string BubbleSort(int[] arr)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int temp;
            for (int j = 0; j <= arr.Length - 2; j++)
            {
                for (int i = 0; i <= arr.Length - 2; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        temp = arr[i + 1];
                        arr[i + 1] = arr[i];
                        arr[i] = temp;
                    }
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }

        private string SelectionSort(int[] arr)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int n = arr.Length;
            int temp, smallest;
            for (int i = 0; i < n - 1; i++)
            {
                smallest = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[smallest])
                    {
                        smallest = j;
                    }
                }
                temp = arr[smallest];
                arr[smallest] = arr[i];
                arr[i] = temp;
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }
        private void MergeMethod(int[] arr)
        {
            int left = 0;
            int right = arr.Length - 1;
            int mid = left + (right - left) / 2;
            int[] temp = new int[arr.Length];

            int i = left;
            int j = mid + 1;
            int k = left;
            while (i <= mid && j <= right)
            {
                if (arr[i] <= arr[j])
                    temp[k++] = arr[i++];
                else
                    temp[k++] = arr[j++];
            }
            while (i <= mid)
                temp[k++] = arr[i++];
            while (j <= right)
                temp[k++] = arr[j++];
            for (i = left; i <= right; i++)
            {
                arr[i] = temp[i];
            }
        }
        private void SortMethod(int[] numbers, int left, int right)
        {
            int mid;
            if (right > left)
            {
                mid = (right + left) / 2;
                SortMethod(numbers, left, mid);
                SortMethod(numbers, (mid + 1), right);
                MergeMethod(numbers);
            }
        }
        private string MergeSort(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            SortMethod(numbers, 0, numbers.Length - 1);

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }
        public void QuickSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);
                QuickSort(arr, left, pivot - 1);
                QuickSort(arr, pivot + 1, right);
            }
        }
        private int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            int temp2 = arr[i + 1];
            arr[i + 1] = arr[right];
            arr[right] = temp2;

            return i + 1;
        }
        public string QuickSortCount(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task.Run(() =>
            {
                QuickSort(numbers, 0, numbers.Length - 1);
            }).ConfigureAwait(false);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }

        public string QuickSortCountString(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            QuickSort(numbers, 0, numbers.Length - 1);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }

        public void ShellSort(int[] array)
        {
            int n = array.Length;
            int gap = n / 2;

            while (gap > 0)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = array[i];
                    int j;
                    for (j = i; j >= gap && array[j - gap] > temp; j -= gap)
                    {
                        array[j] = array[j - gap];
                    }
                    array[j] = temp;
                }
                gap /= 2;
            }
        }

        public string ShellSortCount(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task.Run(() =>
            {
                ShellSort(numbers);
            }).ConfigureAwait(false);

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }

        public string ShellSortCountString(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ShellSort(numbers);

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }

        private async void Async_Sequential_Click(object sender, RoutedEventArgs e)
        {
            int[] array1 = AutoGen();
            int[] array2 = AutoGen();
            int[] array3 = AutoGen();
            int[] array4 = AutoGen();
            int[] array5 = AutoGen();
            lbAlgorithm.Content = "Async - Sequential";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            txtBubbleSort.Text = await asyncSort.BubbleSort(array1) + "ms";
            txtSelectionSort.Text = await asyncSort.SelectionSort(array2) + "ms";
            txtMergeSort.Text = await asyncSort.MergeSort(array3) + "ms";
            txtQuickSort.Text = await asyncSort.QuickSortCount(array4) + "ms";
            txtShellSort.Text = await asyncSort.ShellSortCount(array5) + "ms";
            stopwatch.Stop();
            txtTotal.Text = stopwatch.ElapsedMilliseconds.ToString() + "ms";
        }

        private async void Async_Parallel_Click(object sender, RoutedEventArgs e)
        {
            int[] array1 = AutoGen();
            int[] array2 = AutoGen();
            int[] array3 = AutoGen();
            int[] array4 = AutoGen();
            int[] array5 = AutoGen();
            lbAlgorithm.Content = "Async - Parallel";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task<string> task1 = Task.Run(() => BubbleSort(array1));
            Task<string> task2 = Task.Run(() => SelectionSort(array2));
            Task<string> task3 = Task.Run(() => MergeSort(array3));
            Task<string> task4 = Task.Run(() => QuickSortCount(array4));
            Task<string> task5 = Task.Run(() => ShellSortCount(array5));
            await Task.WhenAll(task1, task2, task3, task4, task5);
            txtBubbleSort.Text = task1.Result + "ms";
            txtSelectionSort.Text = task2.Result + "ms";
            txtMergeSort.Text = task3.Result + "ms";
            txtQuickSort.Text = task4.Result + "ms";
            txtShellSort.Text = task5.Result + "ms";
            stopwatch.Stop();
            txtTotal.Text = stopwatch.ElapsedMilliseconds.ToString() + "ms";
        }

        private void Sync_Sequential_Click(object sender, RoutedEventArgs e)
        {
            int[] array1 = AutoGen();
            int[] array2 = AutoGen();
            int[] array3 = AutoGen();
            int[] array4 = AutoGen();
            int[] array5 = AutoGen();
            lbAlgorithm.Content = "Sync - Sequential";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            txtBubbleSort.Text = BubbleSort(array1) + "ms";
            txtSelectionSort.Text = SelectionSort(array2) + "ms";
            txtMergeSort.Text = MergeSort(array3) + "ms";
            txtQuickSort.Text = QuickSortCountString(array4) + "ms";
            txtShellSort.Text = ShellSortCountString(array5) + "ms";
            stopwatch.Stop();
            txtTotal.Text = stopwatch.ElapsedMilliseconds.ToString() + "ms";
        }

        private void Sync_Parallel_Click(object sender, RoutedEventArgs e)
        {
            int[] array1 = AutoGen();
            int[] array2 = AutoGen();
            int[] array3 = AutoGen();
            int[] array4 = AutoGen();
            int[] array5 = AutoGen();
            lbAlgorithm.Content = "Sync - Parallel";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task<string> task1 = Task.Run(() => BubbleSort(array1));
            Task<string> task2 = Task.Run(() => SelectionSort(array2));
            Task<string> task3 = Task.Run(() => MergeSort(array3));
            Task<string> task4 = Task.Run(() => QuickSortCount(array4));
            Task<string> task5 = Task.Run(() => ShellSortCount(array5));
            ThreadPool.GetAvailableThreads(out int availableWorker, out int _);
            ThreadPool.GetMaxThreads(out int maxWorker, out int _);
            int threadUsed = maxWorker - availableWorker;
            txtBubbleSort.Text = task1.Result + "ms";
            txtSelectionSort.Text = task2.Result + "ms";
            txtMergeSort.Text = task3.Result + "ms";
            txtQuickSort.Text = task4.Result + "ms";
            txtShellSort.Text = task5.Result + "ms";
            stopwatch.Stop();
            txtTotal.Text = stopwatch.ElapsedMilliseconds.ToString() + "ms";
        }
    }
}