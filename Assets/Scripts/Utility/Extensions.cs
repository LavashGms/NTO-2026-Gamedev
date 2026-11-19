using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class ListExtensions
{
    public static void Add<T>(this List<T> list, params T[] values) => list.AddRange(values);
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