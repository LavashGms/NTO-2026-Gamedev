using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        int counter = 0;
        foreach (Product product in basket)
        {
            GameObject visualization = Instantiate(productVisualizationPrefab, transform);
            visualization.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            ProductVisualization visScript = visualization.GetComponent<ProductVisualization>();
            PlaceProduct(visScript);
            visScript.Set(product, this);
            visScript.SetPickingLevel(counter + 4);
            counter++;
        }
    }

    public void PlaceProduct(ProductVisualization product)
    {
        product.transform.localPosition = new Vector3(Random.Range(-productpositionRange.x, productpositionRange.x), Random.Range(-productpositionRange.y, productpositionRange.y), transform.position.z);
        product.placedToBasket = true;
    }
}

public class Basket
{
    private List<Product> products = new List<Product>();
    public int ProductCount { get => products.Count; }
    public int Price { get; private set; }
    public void Clear()
    {
        products.Clear();
        Price = 0;
    }

    public Basket(int productCount)
    {
        for (int i = 0; i < productCount; i++)
        {
            products.Add(ProductsStorage.singleton.GetRandomProduct());
            Price += products[^1].Price;
        }
    }
    public IEnumerator<Product> GetEnumerator() => products.GetEnumerator();
}