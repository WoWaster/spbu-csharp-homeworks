namespace Task1;

public static class ListInsertionSortExtension
{
    public static void InsertionSort<T>(this List<T> list, IComparer<T> comparer)
    {
        var i = 1;
        while (i < list.Count)
        {
            var temp = list[i];
            var j = i - 1;
            while (j >= 0 && comparer.Compare(list[j], temp) > 0)
            {
                list[j + 1] = list[j];
                j--;
            }

            list[j + 1] = temp;
            i++;
        }
    }

    public static void InsertionSort<T>(this List<T> list)
    {
        InsertionSort(list, Comparer<T>.Default);
    }
}