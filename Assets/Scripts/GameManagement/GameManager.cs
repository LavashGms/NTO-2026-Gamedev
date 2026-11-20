using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static System.Math;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Management parts")]
    public static GameManager singleton;
    [SerializeField] private ClientManager clientManager;

    [Header("Shop parts")]
    [SerializeField] public StoreCheckout checkout;

    [Header("Stats")]
    [SerializeField] private StatBar ratingBar;
    [SerializeField] private float rating = 100;

    [Header("UI")]
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject equipmentUpgradeMenu;

    [Header("Player movement")]
    [SerializeField] private Transform player;
    [SerializeField] private int playerSpeed;
    [SerializeField] private GameObject moveTargetParticles;

    [Header("Clients")]
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private float wrongBasketFactor;
    [SerializeField] private Transform[] clientWaitingAreas;
    [NonSerialized] public Basket currentClientBasket;
    private LinkedList<Client> queue = new LinkedList<Client>();

    public IPickable pickedItem { get; private set; }
    void Awake()
    {
        if (singleton == null) singleton = this;
        else throw new Exception();
    }
    private void Start()
    {
        ratingBar.SetMax(rating, rating);
        StartCoroutine(SetNewQueue());
    }

    private IEnumerator SetNewQueue()
    {
        for (int i = queue.Count; i < clientWaitingAreas.Length; i++)
            yield return StartCoroutine(AddClient());
    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mousePos.z = 0;
        if (Mouse.current.leftButton.wasPressedThisFrame && !UIExtensions.IsPointerOverContactableUI())
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector3.forward);
            if (hits.Length == 0)
            {
                ParticleSystem ps = Instantiate(moveTargetParticles, mousePos, moveTargetParticles.transform.rotation).GetComponent<ParticleSystem>();
                RaycastHit2D[] planehits = Physics2D.RaycastAll(player.position, mousePos - player.position, Vector3.Distance(mousePos, player.position));
                foreach (RaycastHit2D h in planehits)
                {
                    if (h && h.transform.CompareTag("Obstacle"))
                        return;
                }
                ParticleSystem.MainModule main = ps.main;
                main.startColor = Color.green;
                player.localScale = new Vector3(Math.Abs(player.localScale.x) * (1 - 2 * Convert.ToInt32(mousePos.x > player.position.x)),
                    player.localScale.y, player.localScale.z);
                player.DOKill();
                player.DOMove(mousePos, Vector3.Distance(mousePos, player.position) / playerSpeed);
            }
            else if (pickedItem == null)
            {
                IPickable maxLayerPickable = null;
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit && hit.transform.gameObject.TryGetComponent<IPickable>(out IPickable pickable))
                    {
                        if (maxLayerPickable == null || pickable.PickupLayer > maxLayerPickable.PickupLayer)
                            maxLayerPickable = pickable;
                    }
                }
                if (maxLayerPickable != null)
                {
                    maxLayerPickable.Pick();
                    pickedItem = maxLayerPickable;
                }
            }
        }

        if (pickedItem != null)
            pickedItem.targetTransform.position = mousePos;

        if (Mouse.current.leftButton.wasReleasedThisFrame && pickedItem != null)
            pickedItem = pickedItem.Drop();
    }

    public void DecreaseRating(int rating)
    {
        this.rating = Max(0, this.rating - rating);
        ratingBar.SetCurrent(this.rating);
    }

    public void PurchaseConfirmed(Basket registredProducts)
    {
        int difference = currentClientBasket.GetDifference(registredProducts);
        if (difference > 0)
        {
            DecreaseRating((int)(difference * wrongBasketFactor));
            if (rating <= 0)
            {
                loseScreen.SetActive(true);
                return;
            }
        }
        StartCoroutine(PushQueue());
    }
    private IEnumerator AddClient()
    {
        Client client = clientManager.GetClient();
        int queuePlace = queue.Count;
        queue.AddLast(client);
        yield return StartCoroutine(client.FollowToPlace(clientWaitingAreas[queuePlace].position));
        if (queuePlace == 0) client.OnCheckout();
    }
    public void PushQ() => StartCoroutine(PushQueue());
    public IEnumerator PushQueue()
    {
        if (queue.Count == 0) yield break;
        queue.First.Value.Finished();
        queue.RemoveFirst();
        int counter = 0;
        foreach (var client in queue)
        {
            yield return client.FollowToPlace(clientWaitingAreas[counter].position);
            if (counter == 0) client.OnCheckout();
            counter++;
        }
        yield return StartCoroutine(AddClient());
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void UnlockUpgradeMenu() => equipmentUpgradeMenu.SetActive(true);
    public void LockUpgradeMenu() { }
}