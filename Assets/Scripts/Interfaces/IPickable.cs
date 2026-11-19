using UnityEngine;

public interface IPickable
{
    int PickupLayer { get => 0; }
    void Pick() { }
    IPickable Drop() { return null; }
    Transform targetTransform { get; }
}
