using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GroupProject
{
    public static class SortMethods
    {
        public static async Task<string> SortAsync(Func<string> method)
        {
            return await Task.Run(method);
        }

        public static string BubbleSort(int[] arr)
        {
            Trace.WriteLine($"Bubble sort thread: {Thread.CurrentThread.ManagedThreadId}");
            Stopwatch sw = Stopwatch.StartNew();
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

            sw.Stop();


            return sw.ElapsedMilliseconds.ToString() + "ms";
        }

        public static string SelectionSort(int[] arr)
        {
            Trace.WriteLine($"Selection sort thread: {Thread.CurrentThread.ManagedThreadId}");
            Stopwatch sw = Stopwatch.StartNew();

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
            sw.Stop();

            return sw.ElapsedMilliseconds.ToString() + "ms";
        }

        public static string QuickSort(int[] arr)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Trace.WriteLine($"Quick sort thread: {Thread.CurrentThread.ManagedThreadId}");

            QuickSort(arr, 0, arr.Length - 1);
            sw.Stop();
            return sw.ElapsedMilliseconds.ToString() + "ms";

        }

        private static void QuickSort(int[] arr, int left, int right)
        {
            int Partition(int[] arr, int left, int right)
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

            if (left < right)
            {
                int pivot = Partition(arr, left, right);
                QuickSort(arr, left, pivot - 1);
                QuickSort(arr, pivot + 1, right);
            }

        }

       



        public static string ShellSort(int[] array)
        {
            Trace.WriteLine($"Shell sort thread: {Thread.CurrentThread.ManagedThreadId}");
            Stopwatch sw = Stopwatch.StartNew();

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

            sw.Stop();

            return sw.ElapsedMilliseconds.ToString() + "ms";
        }
    }
}
