using UnityEngine;

public class ClickCollector : MonoBehaviour
{
    [SerializeField] private LayerMask collectibleMask; // Only these layers are clickable collectibles
    [SerializeField] private LayerMask interactableMask; // Only these layers are clickable collectibles
    private Camera cam;

    void Awake() => cam = Camera.main;

    void Update()
    {
        // React on left mouse button press
        if (!Input.GetMouseButtonDown(0)) return;

        // Convert screen mouse position to world space
        Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);

        // OverlapPoint checks 2D colliders at the click position filtered by the collectibleMask
        Collider2D hit = Physics2D.OverlapPoint(world, collectibleMask);
        if (!hit) return;

        // If the clicked object is a CollectibleItem, try collecting it
        if (hit.TryGetComponent(out CollectibleItem item))
            GameController.I.TryCollect(item);

        // OverlapPoint checks 2D colliders at the click position filtered by the interactableMask
        Collider2D hitObj = Physics2D.OverlapPoint(world, interactableMask);
        if (!hitObj) return;

        // If the clicked object is a CollectibleItem, try collecting it
        if (hit.TryGetComponent(out ItemInteract i))
            GameController.I.Interact(i);
    }
}
