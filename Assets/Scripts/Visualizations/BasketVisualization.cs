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
    private Dictionary<Product, int> products = new Dictionary<Product, int>();
    public int ProductCount { get => products.Count; }
    public int Price { get; private set; }
    public void AddProduct(Product product)
    {
        if (products.ContainsKey(product))
            products[product] += 1;
        else
            products.Add(product, 1);
        Price += product.Price;
    }
    public void Clear()
    {
        products.Clear();
        Price = 0;
    }
    public Basket(int productCount)
    {
        for (int i = 0; i < productCount; i++)
            AddProduct(ProductsStorage.singleton.GetRandomProduct());
    }
    private List<Product> GetProducts()
    {
        List<Product> result = new List<Product>();
        foreach (var kvp in products)
        {
            for (int i = 0; i < kvp.Value; i++)
                result.Add(kvp.Key);
        }
        return result;
    }
    public IEnumerator<Product> GetEnumerator() => GetProducts().GetEnumerator();
    public bool SameWith(Basket basket)
    {
        foreach (var kvp in basket.products)
        {
            if (!products.TryGetValue(kvp.Key, out int count)) return false;
            else if (count != kvp.Value) return false;
        }
        foreach (var kvp in products)
        {
            if (!basket.products.TryGetValue(kvp.Key, out int count)) return false;
        }
        return true;
    }
    public int GetDifference(Basket basket)
    {
        int difference = 0;
        foreach (var kvp in basket.products)
        {
            if (!products.TryGetValue(kvp.Key, out int count))
                difference += kvp.Key.Price * kvp.Value;
            else if (count != kvp.Value)
                difference += kvp.Key.Price * System.Math.Abs(count - kvp.Value);
        }
        foreach (var kvp in products)
        {
            if (!basket.products.TryGetValue(kvp.Key, out int count)) difference += kvp.Key.Price * kvp.Value;
        }
        return difference;
    }
}