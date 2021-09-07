using System;
//The problem:
//The Dijikstra algorithm spends a lot of time just trying to find the minimum member of the array of cells
//Using a minimum heap will make it easier to just pop the first member of the heap, and then use that as the currently inspected
//cell.
//About min heaps:
//Min heaps consist of values, a simple heap might just contain ints
//These ints could be the value of the heap member. Each member has a left and right child, the heaps must always be filled left to right!
//The first index of the min heap also must be the smallest heap member. If this is violated, the heap is sorted with swapping until this is true.
//Each heap member is smaller than its two children, they must swap if the child is bigger than the parent, until all violations are fixed.
class MinHeap<T> where T : IMinimumHeapable
{
    public T[] heapArray;
    private int currentHeapSize;
    public int CurrentHeapSize { get => currentHeapSize; set => currentHeapSize = value; }

    public MinHeap(int heapSize)
    {
        heapArray = new T[heapSize];
        CurrentHeapSize = 0;
    }
    public void ClearHeap()
    {
        Array.Clear(heapArray,0, CurrentHeapSize);
        currentHeapSize = 0;
    }
    public static void SwapMember(ref T swap, ref T swapto)
    {
        T temp = swap;
        swap = swapto;
        swapto = temp;
    }
    public int ParentKey(int key)
    {
        return (key - 1) / 2;

    }
    public int LeftMemberKey(int key)
    {
        return (2 * key + 1);
    }
    public int RightMemberKey(int key)
    {
        return (2 * key + 2);
    }
    public void InsertKey(T key)
    {
        int heapNumber = CurrentHeapSize;
        heapArray[heapNumber] = key;
        CurrentHeapSize++;
        //Fix min heap violations
        while (heapNumber != 0 && heapArray[heapNumber].getElementValue() < heapArray[ParentKey(heapNumber)].getElementValue())
        {
            SwapMember(ref heapArray[heapNumber], ref heapArray[ParentKey(heapNumber)]);
            heapNumber = ParentKey(heapNumber);
        }
        
    }
    public T getMin()
    {
        return heapArray[0];
    }
    public void CheckTreeFromKey(int key)
    {
        while (key != 0 && (heapArray[key].getElementValue() < heapArray[ParentKey(key)].getElementValue()))
        {
            SwapMember(ref heapArray[key], ref heapArray[ParentKey(key)]);
            key = ParentKey(key);
        }
    }
    public T extractMinKey()
    {
        if (CurrentHeapSize == 1)
        {
            CurrentHeapSize--;
            return heapArray[0];
        }
        T root = heapArray[0];

        heapArray[0] = heapArray[CurrentHeapSize - 1];
        CurrentHeapSize--;
        MinHeapify(0);
        return root;
    }
    public void MinHeapify(int key)
    {
        int left = LeftMemberKey(key);
        int right = RightMemberKey(key);

        int smallestKey = key;
        if (left < CurrentHeapSize && heapArray[left].getElementValue() < heapArray[smallestKey].getElementValue())
        {
            smallestKey = left;
        }
        if (right < CurrentHeapSize && heapArray[right].getElementValue() < heapArray[smallestKey].getElementValue())
        {
            smallestKey = right;
        }
        if (smallestKey != key)
        {
            SwapMember(ref heapArray[key], ref heapArray[smallestKey]);
            MinHeapify(smallestKey);
        }
    }

}

