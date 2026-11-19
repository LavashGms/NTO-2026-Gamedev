using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    [Header("Shop parts")]
    [SerializeField] private StoreCheckout checkout;
    [Header("Player movement")]
    [SerializeField] private Transform player;
    [SerializeField] private int playerSpeed;
    [SerializeField] private GameObject moveTargetParticles;

    private IPickable pickedItem;
    void Awake()
    {
        if (singleton == null) singleton = this;
        else throw new Exception();
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
}