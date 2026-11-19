using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class SpriteRenderingArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    [SerializeField] private Vector2 areaSize = new Vector2(1, 1);
    [SerializeField] private Color visualiztionColor = Color.white;
    [SerializeField] private bool normalizeAtStart = true;

    public bool keepNativeIfInBounds;
    [SerializeField] private Vector2 nativeSize = new Vector2(1, 1);

    public bool autoResize;
    [SerializeField] private Vector2 defaultParentGlobalScale;
    [SerializeField] private Vector2 defaultAreaSize;

    private void Awake()
    {
        if (targetSpriteRenderer == null)
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
        targetSpriteRenderer.RegisterSpriteChangeCallback(NormalizeSize);
        if (normalizeAtStart)
            NormalizeSize();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = visualiztionColor;
        Gizmos.DrawLine(new Vector3(transform.position.x - areaSize.x / 2, transform.position.y - areaSize.y / 2, transform.position.z),
            new Vector3(transform.position.x - areaSize.x / 2, transform.position.y + areaSize.y / 2, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x + areaSize.x / 2, transform.position.y - areaSize.y / 2, transform.position.z),
            new Vector3(transform.position.x + areaSize.x / 2, transform.position.y + areaSize.y / 2, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x - areaSize.x / 2, transform.position.y - areaSize.y / 2, transform.position.z),
            new Vector3(transform.position.x + areaSize.x / 2, transform.position.y - areaSize.y / 2, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x - areaSize.x / 2, transform.position.y + areaSize.y / 2, transform.position.z),
            new Vector3(transform.position.x + areaSize.x / 2, transform.position.y + areaSize.y / 2, transform.position.z));
    }

    private void Update()
    {
        if (autoResize && transform.parent != null && transform.parent.GetGlobalScale() != defaultParentGlobalScale)
        {
            areaSize = transform.parent.GetGlobalScale() * defaultAreaSize / defaultParentGlobalScale;
            NormalizeSize();
        }
    }

    public void SetArea(float x, float y)
    {
        areaSize.Set(x, y);
        NormalizeSize();
    }
    public void SetArea(Vector2 size)
    {
        areaSize = size;
        NormalizeSize();
    }
    public void SetArea(float x)
    {
        areaSize.Set(x, x);
        NormalizeSize();
    }

    public void Set(Sprite sptire, float areaX, float  areaY)
    {
        areaSize.Set(areaX, areaY);
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            targetSpriteRenderer.RegisterSpriteChangeCallback(NormalizeSize);
        }
        targetSpriteRenderer.sprite = sptire;
        NormalizeSize();
    }
    public void Set(Sprite sptire, Vector2 size)
    {
        areaSize.Set(size.x, size.y);
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            targetSpriteRenderer.RegisterSpriteChangeCallback(NormalizeSize);
        }
        targetSpriteRenderer.sprite = sptire; ;
        NormalizeSize();
    }
    public void Set(Sprite sptire, float areaSize)
    {
        this.areaSize.Set(areaSize, areaSize);
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
            targetSpriteRenderer.RegisterSpriteChangeCallback(NormalizeSize);
        }
        targetSpriteRenderer.sprite = sptire; ;
        NormalizeSize();
    }

    public void SetNativeBoundsKeeping(float nativeX, float nativeY)
    {
        keepNativeIfInBounds = true;
        nativeSize.Set(nativeX, nativeY);
        NormalizeSize();
    }
    public void SetNativeBoundsKeeping(Vector2 nativeSize)
    {
        keepNativeIfInBounds = true;
        this.nativeSize = nativeSize;
        NormalizeSize();
    }
    public void SetNativeBoundsKeeping(float nativeSize)
    {
        keepNativeIfInBounds = true;
        this.nativeSize.Set(nativeSize, nativeSize);
        NormalizeSize();
    }

    public void NormalizeSize(SpriteRenderer renderer)
    {
        if (renderer == null)
            renderer = GetComponent<SpriteRenderer>();
        if (renderer.sprite == null)
            return;
        Vector2 globalScale = transform.GetGlobalScale();
        float currentSpriteWidth = renderer.sprite.rect.width / renderer.sprite.pixelsPerUnit * globalScale.x;
        float currentSpriteHeight = renderer.sprite.rect.height / renderer.sprite.pixelsPerUnit * globalScale.y;

        if (!keepNativeIfInBounds || currentSpriteWidth / transform.localScale.x * nativeSize.x > areaSize.x || currentSpriteHeight / transform.localScale.y * nativeSize.y > areaSize.y)
        {
            float scaleFactor = 1;
            if (currentSpriteWidth - areaSize.x > currentSpriteHeight - areaSize.y)
                scaleFactor = areaSize.x / currentSpriteWidth;
            else
                scaleFactor = areaSize.y / currentSpriteHeight;
            transform.localScale = new Vector2(transform.localScale.x * scaleFactor, transform.localScale.y * scaleFactor);
        }
        else
            transform.localScale = nativeSize;
    }
    public void NormalizeSize() => NormalizeSize(targetSpriteRenderer);
}

public static class TransformExtension
{
    public static Vector2 GetGlobalScale(this Transform transform)
    {
        Vector2 globalScale = transform.localScale;
        Transform parent = transform.parent;
        while (parent != null)
        {
            globalScale.Set(globalScale.x * parent.localScale.x, globalScale.y * parent.localScale.y);
            parent = parent.parent;
        }
        return globalScale;
    }
}