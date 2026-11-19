using UnityEngine;
using System.Collections.Generic;

public class BasketVisualization : MonoBehaviour
{
    [SerializeField] private GameObject productVisualizationPrefab;
    [SerializeField] private Vector2 productpositionRange;
    private StoreCheckout checkout;
    public Basket basket { get; private set; }
    public void Set(Basket basket, StoreCheckout checkout)
    {
        this.basket = basket;
        this.checkout = checkout;
        for (int i = 0; i < basket.products.Count; i++)
        {
            GameObject visualization = Instantiate(productVisualizationPrefab, transform);
            visualization.transform.localPosition = new Vector3(Random.Range(-productpositionRange.x, productpositionRange.x), Random.Range(-productpositionRange.y, productpositionRange.y), transform.position.z);
            visualization.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            visualization.GetComponent<ProductVisualization>().Set(basket.products[i], this, i);
            visualization.GetComponent<ProductVisualization>().SetSortingLevel(i + 4);
        }
    }

    public void RemoveProduct(Product product)
    {
        basket.products.Remove(product);
        if (basket.products.Count == 0)
            checkout.RemoveBasket();
    }
}

public class Basket
{
    public List<Product> products = new List<Product>();
    public int Price { get; private set; }
    public void Clear() => products.Clear();
    public Basket(int productCount)
    {
        for (int i = 0; i < productCount; i++)
        {
            products.Add(ProductsStorage.singleton.GetRandomProduct());
            Price += products[^1].Price;
        }
    }
}