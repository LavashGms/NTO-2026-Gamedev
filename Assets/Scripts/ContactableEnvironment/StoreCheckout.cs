using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;
using TMPro;

public class StoreCheckout : MonoBehaviour
{
    [Header("Baskets")]
    [SerializeField] private GameObject basketVisualizationPrefab;
    [SerializeField] private Vector2 minBasketPosition;
    [SerializeField] private Vector2 maxBasketPosition;
    [SerializeField] private float basketZRotationDiapason;

    private BasketVisualization currentBasket;

    [Header("Products list")]
    [SerializeField] private GameObject cashRegister;
    [SerializeField] private RectTransform registerLogList;
    [SerializeField] private GameObject listLogPrefab;
    [SerializeField] private float logsDistance;
    private Basket productListBacket = new Basket(0);

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomPower;
    [SerializeField] private Vector2 zoomCoordinates;
    private bool zoomed;

    private void Start()
    {
        ActivateProductList(false);
        AddBasket(new Basket(UnityEngine.Random.Range(2, 4)));
    }

    private void OnMouseDown()
    {
        if (!zoomed && GameManager.singleton.pickedItem == null)
        {
            Vector3 currCameraPos = Camera.main.transform.position;
            float currZoom = Camera.main.orthographicSize;
            Camera.main.DOOrthoSize(zoomPower, zoomSpeed);
            Camera.main.transform.DOMove(new Vector3(zoomCoordinates.x, zoomCoordinates.y, currCameraPos.z), zoomSpeed);
            zoomed = true;
            ExitMultitaskButton.instance.AddAction(() =>
            {
                Camera.main.DOOrthoSize(currZoom, zoomSpeed);
                Camera.main.transform.DOMove(currCameraPos, zoomSpeed);
                zoomed = false;
            });
        }
    }

    public void AddBasket(Basket basket)
    {
        if (currentBasket != null) throw new Exception("basket already setted");
        GameObject visualization = Instantiate(basketVisualizationPrefab, transform);
        visualization.transform.localPosition = new Vector3(UnityEngine.Random.Range(minBasketPosition.x, maxBasketPosition.x), UnityEngine.Random.Range(minBasketPosition.y, maxBasketPosition.y), transform.position.z);
        visualization.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-basketZRotationDiapason, basketZRotationDiapason));
        currentBasket = visualization.GetComponent<BasketVisualization>();
        currentBasket.Set(basket, this);
        ActivateProductList(true);
    }

    public void RemoveBasket()
    {
        ActivateProductList(false);
        Destroy(currentBasket.gameObject);
        StartCoroutine(NextClient());
    }

    private IEnumerator NextClient()
    {
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1, 3));
        AddBasket(new Basket(UnityEngine.Random.Range(3, 6)));
    }

    private void ActivateProductList(bool activate)
    {
        productListBacket.Clear();
        foreach (Transform child in registerLogList)
            Destroy(child.gameObject);
        cashRegister.SetActive(activate);
    }
    public void RegisterPurchasingProduct(Product product)
    {
        GameObject log = Instantiate(listLogPrefab, registerLogList.transform);
        log.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -logsDistance * (registerLogList.childCount - 1));
        log.GetComponent<TextMeshProUGUI>().text = product.StringFormat + " 1רע";
    }
}