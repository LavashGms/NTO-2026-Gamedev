using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PackmanAnimatorLol : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite phase1;
    [SerializeField] private Sprite phase2;
    [SerializeField] private Vector2 delay;
    [SerializeField] private Vector2 phraseDelay;

    private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();
    private void Start() => StartCoroutine(Speak(0));

    private IEnumerator Speak(int moveCount)
    {
        for (int i = 0; i < moveCount; i++)
        {
            spriteRenderer.sprite = phase2;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = phase1;
            yield return new WaitForSeconds(Random.Range(delay.x, delay.y));
        }
        yield return new WaitForSeconds(Random.Range(phraseDelay.x, phraseDelay.y));
        StartCoroutine(Speak(Random.Range(2, 6)));
    }
}