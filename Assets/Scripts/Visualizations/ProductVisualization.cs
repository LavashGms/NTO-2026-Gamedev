using UnityEngine;
using System.Collections.Generic;
using System;

public class ProductVisualization : MonoBehaviour, IBarcodeActivable, IPickable
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private int barCodesCount;

    private BasketVisualization basket;
    public Product product { get; private set; }
    public Transform targetTransform => transform;
    private int pickupLayer;
    public int PickupLayer { get => pickupLayer; }

    public void Set(Product product, BasketVisualization basket, int pickupLayer = 0)
    {
        this.product = product;
        this.basket = basket;
        this.pickupLayer = pickupLayer;
        spriteRenderer.sprite = product.sprite;
    }

    public void Activate()
    {
        Destroy(gameObject);
        basket.RemoveProduct(product);
    }

    public void SetSortingLevel(int level) => spriteRenderer.sortingOrder = level;
}

[Serializable]
public class Product
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int Weight { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    [SerializeField] public List<Vector3> availableBarcodePositions;

    public Product() { }
    public Product(string name, int price)
    {
        this.Name = name;
        this.Price = price;
    }
}