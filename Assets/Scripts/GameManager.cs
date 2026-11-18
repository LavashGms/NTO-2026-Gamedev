using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    [Header("Player movement")]
    [SerializeField] private Transform player;
    [SerializeField] private int playerSpeed;
    [SerializeField] private GameObject moveTargetParticles;
    void Awake()
    {
        if (singleton == null) singleton = this;
        else throw new Exception();
    }
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            clickPos.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector3.forward);
            if (hit && hit.transform.gameObject.TryGetComponent<IContactable>(out IContactable contactable)) 
                contactable.Contact();
            else
            {
                ParticleSystem ps = Instantiate(moveTargetParticles, clickPos, moveTargetParticles.transform.rotation).GetComponent<ParticleSystem>();
                RaycastHit2D[] hits = Physics2D.RaycastAll(player.position, clickPos - player.position, Vector3.Distance(clickPos, player.position));
                foreach (RaycastHit2D h in hits)
                {
                    if (h && h.transform.CompareTag("Obstacle"))
                        return;
                }
                ParticleSystem.MainModule main = ps.main;
                main.startColor = Color.green;
                player.localScale = new Vector3(Math.Abs(player.localScale.x) * (1 - 2 * Convert.ToInt32(clickPos.x > player.position.x)), 
                    player.localScale.y, player.localScale.z);
                player.DOKill();
                player.DOMove(clickPos, Vector3.Distance(clickPos, player.position) / playerSpeed);
            }
        }
    }
}