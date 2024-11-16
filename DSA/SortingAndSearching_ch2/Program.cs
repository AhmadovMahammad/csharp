﻿using SortingAndSearching_ch2;

internal class Program
{
    private static void Main(string[] args)
    {
        /* Sorting Basics
         
        Sorting is the process of arranging elements in a specific order, typically ascending or descending. 
        Understanding sorting is foundational in computer science as 
        it forms the basis for efficient data management and searching.

        --- One key concept in sorting is stability.
        
        Stability in sorting algorithms means that if two elements have the same value, 
        their relative order remains the same in the sorted result as it was in the input. 
        ...
        For instance, in a list [2a, 1, 2b] where 2a and 2b are identical, 
        a stable algorithm ensures that 2a appears before 2b in the output. 


        --- The efficiency of sorting algorithms is often evaluated using time complexity, 
        which measures the number of operations required as a function of the input size. 
        
        For example, algorithms like Bubble Sort have a time complexity of O(n^2), 
        meaning they perform poorly on large datasets, 
        while Quick Sort and Merge Sort, with O(nlogn) complexity, are more efficient for such cases.


        --- Another crucial characteristic is whether the sorting algorithm is in-place or out-of-place. 

        1) An in-place sorting algorithm uses constant or minimal additional memory beyond the input array, 
        directly modifying the array to achieve the sorted order. 
        Examples include Quick Sort and Bubble Sortw. 
        
        2) Conversely, out-of-place algorithms require additional memory to store temporary data or a copy of the array. 
        Merge Sort is a classic example of an out-of-place algorithm due to its auxiliary space requirements.

        */

        /* Bubble Sort
         
        Bubble Sort is one of the simplest sorting algorithms, often used for educational purposes due to its straightforward approach. 
        It sorts a list by repeatedly stepping through the array, 
        comparing adjacent elements, and swapping them if they are in the wrong order. 
        This process is repeated until the entire array is sorted.

        public void BubbleSort(int[] nums)
        {
            int n = nums.Length;
            bool swapped = false;

            // { 5, 4, 3, 2, 1 }

            // i = 0, j until 4, j max val is 3
            // j:0 -> { 4, 5, 3, 2, 1 } => j:1 -> { 4, 3, 5, 2, 1 } => j:2 -> { 4, 3, 2, 5, 1 } => j:3 -> { 4, 3, 2, 1, 5 }

            // i = 1, j until 3, j max val is 2
            // j:0 -> { 3, 4, 2, 1, 5 } => j:1 -> { 3, 2, 4, 1, 5 } => j:2 -> { 3, 2, 1, 4, 5 }

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        (nums[j], nums[j + 1]) = (nums[j + 1], nums[j]);
                        swapped = true;
                    }
                }

                if (!swapped) break;
            }
        }

        In each pass of the outer loop (i iterations), the largest unsorted element "bubbles up" to 
        its correct position at the end of the array. 
        This means after the first pass, the largest element is guaranteed to be in the last position; 
        after the second pass, the second largest element is in the second-to-last position, and so on. 
        
        Thus, with each successive pass, 
        there’s no need to compare elements that have already been sorted and placed in their correct positions.

        */

        Solution solution = new Solution();

        // bubble sort
        //int[] nums = new int[] { 5, 4, 3, 2, 1 };

        //Console.WriteLine(string.Join(',', nums));
        //solution.BubbleSort(nums);

        //Console.WriteLine(string.Join(',', nums));


    }
}