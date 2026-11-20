using UnityEngine;
using TMPro;

public class PurchaseLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    private Product product;
    private int count;

    public void Set(Product product)
    {
        this.product = product;
        count = 1;
        Log();
    }
    public void Add()
    {
        count++;
        Log();
    }
    public bool ProductEquals(Product product) => this.product == product;
    private void Log() => logText.text = $"{product.StringFormat} {count}רע";
}