using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool movedLeft = false;
    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
        startingPosition = transform.position;
        targetPosition = startingPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // gets mouseposition to world space
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Collider2D overlap = Physics2D.OverlapPoint(mousePosition);
            if (overlap != null && !isMoving)
            {
                isMoving = true;
                targetPosition = transform.position + Vector3.left * moveDistance;
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }
}
