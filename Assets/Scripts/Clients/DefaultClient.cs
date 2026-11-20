using UnityEngine;

public class DefaultClient : Client
{
    [SerializeField] private Sprite[] skins;
    [SerializeField] private GameObject miniBasket;

    public Basket basket { get; private set; }
    private void Start()
    {
        if (skins.Length > 0)
            GetComponent<SpriteRenderer>().sprite = skins[UnityEngine.Random.Range(0, skins.Length)];
        basket = new Basket(UnityEngine.Random.Range(3, 6));
    }

    public override void OnCheckout()
    {
        miniBasket.SetActive(false);
        GameManager.singleton.checkout.AddBasket(basket);
    }
    public override void Finished()
    {
        miniBasket.SetActive(true);
        base.Finished();
    }
}
