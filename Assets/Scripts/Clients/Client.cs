using UnityEngine;
using DG.Tweening;
using System.Collections;

public abstract class Client : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] Vector3 exitCoordinates;

    public virtual IEnumerator FollowToPlace(Vector3 position)
    {
        transform.DOKill();
        float time = Vector3.Distance(position, transform.position) / speed;
        transform.DOMove(position, time);
        yield return new WaitForSeconds(time);
    }
    public abstract void OnCheckout();
    public virtual void Finished()
    {
        transform.DOKill();
        float time = Vector3.Distance(exitCoordinates, transform.position) / speed;
        transform.DOMove(exitCoordinates, time).OnComplete(() => Destroy(gameObject));
    }
}