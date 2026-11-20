using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class StatBar : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private RectTransform backGround;
    [SerializeField] private float horizontalIndent;
    [SerializeField] private float verticalIndent;

    [SerializeField] private float horizontalSize;
    [SerializeField] private float verticalSize;

    private float maxValue;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (backGround != null)
        {
            horizontalSize = backGround.sizeDelta.x - 2 * horizontalIndent;
            verticalSize = backGround.sizeDelta.y - 2 * verticalIndent;
        }
    }
    public void SetMax(float maxValue, float currentValue)
    {
        this.maxValue = maxValue;
        SetCurrent(currentValue);
    }
    public void SetCurrent(float currentValue)
    {
        rectTransform.sizeDelta = new Vector2(horizontalSize * currentValue / maxValue, verticalSize);
        rectTransform.anchoredPosition = new Vector2(-horizontalSize / 2 + (currentValue * horizontalSize / 2) / maxValue, 0);
    }
}