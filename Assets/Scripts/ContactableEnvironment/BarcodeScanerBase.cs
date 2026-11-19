using UnityEngine;
using UnityEngine.InputSystem;

public class BarcodeScanerBase : MonoBehaviour, IPickable
{
    [SerializeField] private StoreCheckout storeCheckout;
    [SerializeField] private Transform scanner;
    [SerializeField] private Transform scanLine;

    private bool scanMode;
    private IBarcodeActivable scanningObject;

    public Transform targetTransform => scanner;
    public void Pick()
    {
        scanMode = true;
        scanLine.gameObject.SetActive(true);
    }
    public IPickable Drop()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mousePosition.z = transform.position.z;
        if (GetComponent<BoxCollider2D>().OverlapPoint(mousePosition))
        {
            scanner.position = transform.position;
            scanMode = false;
            scanLine.gameObject.SetActive(false);
            return null;
        }
        return this;
    }

    void Update()
    {
        if (scanMode)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(scanLine.transform.position, Vector3.forward);
                if (hits.Length > 0)
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.transform.gameObject.TryGetComponent<IBarcodeActivable>(out IBarcodeActivable barcodeActivable))
                        {
                            scanningObject = barcodeActivable;
                            break;
                        }
                    }
                }
            }
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                Scan();
        }
    }

    private void Scan()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(scanLine.transform.position, Vector3.forward);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.TryGetComponent<IBarcodeActivable>(out IBarcodeActivable barcodeActivable))
                {
                    if (barcodeActivable == scanningObject)
                    {
                        barcodeActivable.Activate(storeCheckout);
                        break;
                    }
                }
            }
        }
    }
}