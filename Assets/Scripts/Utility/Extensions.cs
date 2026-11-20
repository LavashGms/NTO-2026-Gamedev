using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public static class ListExtensions
{
    public static void Add<T>(this List<T> list, params T[] values) => list.AddRange(values);
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public static T Randomize<T>(this List<T> items, List<double> weights)
    {
        double totalWeight = 0;
        weights.ForEach((x) => totalWeight += x);
        System.Random rnd = new System.Random();
        double result = rnd.NextDouble() * totalWeight;
        double current = 0;
        for (int i = 0; i < items.Count; i++)
        {
            current += weights[i];
            if (current > result)
                return items[i];
        }
        throw new Exception("randomization failed");
    }
}

public static class UIExtensions
{
    public static bool IsPointerOverContactableUI()
    {
        PointerEventData currentData = new PointerEventData(EventSystem.current);
        currentData.position = Mouse.current.position.value;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(currentData, results);
        return results.Count > 0;
    }
}