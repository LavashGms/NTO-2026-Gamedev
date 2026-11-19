using UnityEngine;
using System.Collections.Generic;
using System;

public class ProductVisualization : MonoBehaviour, IBarcodeActivable, IPickable
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private int barCodesCount;
    public Product product { get; private set; }

    private BasketVisualization basket;
    [NonSerialized] public bool placedToBasket;

    public Transform targetTransform => transform;
    private int pickupLayer;
    public int PickupLayer { get => pickupLayer; }

    public void Set(Product product, BasketVisualization basket)
    {
        this.product = product;
        this.basket = basket;
        spriteRenderer.sprite = product.sprite;
    }

    public bool Activate(StoreCheckout register)
    {
        if (placedToBasket) return false;
        register.RegisterPurchasingProduct(product);
        return true;
    }

    public void SetPickingLevel(int pickupLayer)
    {
        spriteRenderer.sortingOrder = pickupLayer;
        this.pickupLayer = pickupLayer;
    }

    public void Pick() => placedToBasket = false;
    public IPickable Drop()
    {
        if (InBasket()) basket.PlaceProduct(this);
        return null;
    }
    private bool InBasket()
    {
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector2.zero))
        {
            if (hit.transform.gameObject.TryGetComponent<BasketVisualization>(out BasketVisualization vis) && vis == basket)
                return true;
        }
        return false;
    }
}

[Serializable]
public class Product
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int Weight { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    public string StringFormat { get => $"{Name} {Weight}г {Price}руб."; }
    [SerializeField] public List<Vector3> availableBarcodePositions;
    public Product() { }
    public Product(string name, int price)
    {
        this.Name = name;
        this.Price = price;
    }
}