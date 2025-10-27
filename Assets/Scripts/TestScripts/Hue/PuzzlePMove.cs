using UnityEngine;

public class PuzzlePMove : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Collider2D col;

    void Start()
    {
        mainCamera = Camera.main;
        col = GetComponent<Collider2D>();
        if (col == null)
            Debug.LogError("Collider2D missing on puzzle piece!");
    }

    void Update()
    {
        // ---- TOUCH INPUT ----
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = mainCamera.ScreenToWorldPoint(touch.position);
            touchPos.z = transform.position.z;

            if (touch.phase == TouchPhase.Began)
            {
                if (col.OverlapPoint(touchPos))
                {
                    isDragging = true;
                    offset = transform.position - touchPos;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                transform.position = touchPos + offset;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
                CheckReveal();
            }
        }

        // ---- MOUSE INPUT (works for touchpad click) ----
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        if (Input.GetMouseButtonDown(0))
        {
            if (col.OverlapPoint(mousePos))
            {
                isDragging = true;
                offset = transform.position - mousePos;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mousePos + offset;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            CheckReveal();
        }
    }

    void CheckReveal()
    {
        // Check for any colliders overlapping this piece
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, col.bounds.size, 0f);
        foreach (var hit in hits)
        {
            // If the object has a PuzzleReveal component, reveal it
            PuzzleReveal reveal = hit.GetComponent<PuzzleReveal>();
            if (reveal != null)
            {
                reveal.Reveal();
            }
        }
    }
}
