using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions 
{
    private static List<object> tempList = new List<object>(128);

    public static void ShuffleInPlace<T>(this List<T> zList)
    {
        tempList.Clear();
        foreach(var element in zList)
        {
            tempList.Add(element);
        }

        zList.Clear();

        int size = tempList.Count;
        for(int i = 0 ; i < size; i++)
        {
            var rand = Random.Range(0, tempList.Count);
            zList.Add((T)tempList[rand]);

            tempList.RemoveAt(i);
        }

        tempList.Clear();
    }

    public static List<T> GetRandom<T>(this List<T> zList, int zCount)
    {
        tempList.Clear();
        foreach(var element in zList)
        {
            tempList.Add(element);
        }

        var newList = new List<T>();

        int count = tempList.Count;
        for(int i = 0 ; i < count && i < zCount; i++)
        {
            var rand = Random.Range(0, tempList.Count);
            newList.Add((T)tempList[rand]);
            tempList.RemoveAt(rand);
        }

        tempList.Clear();
        return newList;
    }
}
