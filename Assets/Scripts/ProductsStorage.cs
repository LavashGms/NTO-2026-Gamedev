using UnityEngine;
using System.Collections.Generic;
using System;

public class ProductsStorage : MonoBehaviour
{
    public static ProductsStorage singleton;
    public List<Product> defaultProducts;
    public Product GetRandomProduct() => defaultProducts[UnityEngine.Random.Range(0, defaultProducts.Count)];

    private void Awake()
    {
        if (singleton == null) singleton = this;
        else throw new Exception();
    }
}