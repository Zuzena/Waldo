using UnityEngine;
using UnityEngine.InputSystem;

public class HoverHighlighter : MonoBehaviour
{
    [SerializeField] private LayerMask collectibleMask; // The 'Collectible' layer

    private Camera cam;
    private Highlightable current;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Gets the current mouse position on the screen
        // Works with both the new Input System and the old one
        Vector3 screenPos = Mouse.current != null
            ? (Vector3)Mouse.current.position.ReadValue()
            : Input.mousePosition;

        // Converts the mouse position from screen space to world space,
        // so we can detect objects in the scene where the mouse is pointing
        Vector3 world = cam.ScreenToWorldPoint(screenPos);
        world.z = 0f;

        // Checks if there’s any 2D collider under the mouse cursor
        // on the specified collectible layers
        Collider2D hit = Physics2D.OverlapPoint(world, collectibleMask);

        // Tries to find a “Highlightable” component on the object we hit
        // If the collider itself doesn’t have one, check its parent object
        Highlightable next = null;
        if (hit != null)
        {
            hit.TryGetComponent(out next);
            if (next == null) next = hit.GetComponentInParent<Highlightable>();
        }

        // If we’re now hovering a different object than before,
        // turn off the old highlight and turn on the new one
        if (current != next)
        {
            if (current != null) current.SetHighlight(false);
            current = next;
            if (current != null) current.SetHighlight(true);
        }
    }
}
