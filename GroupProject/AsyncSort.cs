using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    public class AsyncSort
    {
        public async Task<string> QuickSortCount(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Task.Run(() =>
            {
                QuickSort(numbers, 0, numbers.Length - 1);
            });
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }

        private void MergeMethod(int[] arr)
        {

            if (arr.Length < 2)
            {
                return;
            }

            int mid = arr.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[arr.Length - mid];

            for (int i = 0; i < mid; i++)
            {
                left[i] = arr[i];
            }

            for (int i = mid; i < arr.Length; i++)
            {
                right[i - mid] = arr[i];
            }

            MergeSort(left);
            MergeSort(right);
            SortMethod(left, right, arr);
        }

        private void SortMethod(int[] left, int[] right, int[] arr)
        {
            int i = 0, j = 0, k = 0;

            while (i < left.Length && j < right.Length)
            {
                if (left[i] <= right[j])
                {
                    arr[k++] = left[i++];
                }
                else
                {
                    arr[k++] = right[j++];
                }
            }

            while (i < left.Length)
            {
                arr[k++] = left[i++];
            }

            while (j < right.Length)
            {
                arr[k++] = right[j++];
            }
        }
        public async Task<string> MergeSort(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Task.Run(() =>
            {
                MergeMethod(numbers);

            });

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
        public async Task<string> BubbleSort(int[] arr)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int temp;
            await Task.Run(() =>
            {
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
            });
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }


        public async Task<string> SelectionSort(int[] arr)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int n = arr.Length;
            await Task.Run(() =>
            {
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
            });
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
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

        public async Task<string> ShellSortCount(int[] numbers)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Task.Run(() =>
            {
                ShellSort(numbers);
            });

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds.ToString();
        }
    }
}
