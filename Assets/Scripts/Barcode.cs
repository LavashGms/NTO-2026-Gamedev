using UnityEngine;

public class Barcode : MonoBehaviour
{
    private IBarcodeActivable activable;
    public virtual bool Read(Vector2 readerPos, Vector2 readerScale, out IBarcodeActivable activable)
    {
        activable = this.activable;
        return activable != null;
    }
}

public interface IBarcodeActivable
{
    void Activate();
}