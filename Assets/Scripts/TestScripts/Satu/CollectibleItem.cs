using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CollectibleItem : MonoBehaviour
{
    [Header("Identity")]
    public string itemId;

    [Header("Destination")]
    public Transform destination;                 // if null, tries tag "PaintingTarget"
    public Vector3 destinationOffset = Vector3.zero;
    public bool keepCurrentZ = true;

    [Header("Motion")]
    public float moveSpeed = 0f;                  // if > 0, duration = distance / speed
    public float moveDuration = 0.6f;             // used when moveSpeed <= 0
    public AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Scale")]
    public float endScaleMultiplier = 0.3f;

    [Header("After Arrival")]
    public bool autoHideOnCollect = true;
    public UnityEvent onCollected;

    [Header("Rendering while collecting")]
    public bool setSortingOnCollect = true;
    public int orderOnCollect = 4;
    public bool affectChildrenRenderers = true;

    [Header("Confetti (optional)")]
    [Tooltip("ParticleSystem prefab to spawn when the item arrives in the painting.")]
    public ParticleSystem confettiPrefab;
    
    public Vector3 confettiOffset = Vector3.zero;
    [Tooltip("Sorting Order for the confetti renderer (so it shows on top must be at least 5 or more).")]
    public int confettiOrderInLayer = 5;
    
    public float confettiLifetime = 0f;

    
    private bool isCollecting;
    private Collider2D col;
    private SpriteRenderer[] renderers;
    private int[] originalOrders;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        if (destination == null)
        {
            var t = GameObject.FindWithTag("PaintingTarget");
            if (t) destination = t.transform;
        }
    }

    public void Collected()
    {
        if (isCollecting) return;
        onCollected?.Invoke();
        StartCoroutine(CollectRoutine());
    }

    private IEnumerator CollectRoutine()
    {
        if (destination == null)
        {
            Debug.LogError($"{name}: No destination set. Assign one or add a 'PaintingTarget' tagged object.");
            if (autoHideOnCollect) gameObject.SetActive(false);
            yield break;
        }

        isCollecting = true;
        if (col) col.enabled = false;

        // Raise render order during the flight
        if (setSortingOnCollect)
        {
            renderers = affectChildrenRenderers
                ? GetComponentsInChildren<SpriteRenderer>(true)
                : new[] { GetComponent<SpriteRenderer>() };

            if (renderers != null && renderers.Length > 0)
            {
                originalOrders = new int[renderers.Length];
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] == null) continue;
                    originalOrders[i] = renderers[i].sortingOrder;
                    renderers[i].sortingOrder = orderOnCollect; // put on Order in Layer 4 while flying
                }
            }
        }

        // Start/End for pos & scale
        Vector3 startPos = transform.position;
        Vector3 endPos = destination.position + destinationOffset;
        if (keepCurrentZ) endPos.z = startPos.z;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * Mathf.Max(0f, endScaleMultiplier);

        // Duration by speed or fixed
        float duration = moveDuration;
        if (moveSpeed > 0f)
        {
            float dist = Vector3.Distance(startPos, endPos);
            duration = Mathf.Max(0.01f, dist / moveSpeed);
        }

        // Animate
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float e = easing.Evaluate(k);

            transform.position = Vector3.LerpUnclamped(startPos, endPos, e);
            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, e);

            yield return null;
        }

        // Snap to final
        transform.position = endPos;
        transform.localScale = endScale;

        // Confetti burst at arrival
        if (confettiPrefab != null)
        {
            var ps = Instantiate(confettiPrefab, endPos + confettiOffset, Quaternion.identity);

            // Make sure it renders on top
            var rdr = ps.GetComponent<ParticleSystemRenderer>();
            if (rdr != null)
            {
                // match the item's sorting layer if possible
                var baseSr = GetComponent<SpriteRenderer>();
                if (baseSr != null)
                {
                    rdr.sortingLayerID = baseSr.sortingLayerID;
                    rdr.sortingOrder = confettiOrderInLayer;
                }
                else
                {
                    // Default layer
                    rdr.sortingOrder = confettiOrderInLayer;
                }
            }

            ps.Play();

            float life = confettiLifetime > 0f ? confettiLifetime : ps.main.duration + ps.main.startLifetime.constantMax;
            Destroy(ps.gameObject, life);
        }

        if (autoHideOnCollect)
        {
            gameObject.SetActive(false);
        }
        else if (renderers != null && originalOrders != null)
        {
            for (int i = 0; i < renderers.Length; i++)
                if (renderers[i] != null) renderers[i].sortingOrder = originalOrders[i];
        }

        isCollecting = false;
    }
}
