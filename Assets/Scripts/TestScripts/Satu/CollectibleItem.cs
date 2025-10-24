using UnityEngine;
using UnityEngine.Events;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("Identifier that must match an entry in GameController.requiredItemIds.")]
    public string itemId;

    [Tooltip("If true, the GameObject is hidden after being collected.")]
    public bool autoHideOnCollect = true;

    // !!Later use for effects!!
    public UnityEvent onCollected;

    // Called by GameController when this item is successfully collected
    public void Collected()
    {
        onCollected?.Invoke();
        if (autoHideOnCollect)
            gameObject.SetActive(false);
    }
}
