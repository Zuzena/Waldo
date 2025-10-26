using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
  [Header("Movement Setting")]
  [SerializeField] private float moveDistance = 2f;
  [SerializeField] private float moveSpeed = 3f;

[Header("Movement Direction")]
  public bool moveLeft;
  public bool moveRight;
  public bool moveUp;
  public bool moveDown;
  // public bool moveBackToStart; 


  private Vector3 startingPosition;
  private Vector3 targetPosition;
  private bool isMoving = false;
  // private bool movedLeft = false;
  private bool isAtStart = true;
  private Camera mainCamera;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    mainCamera = Camera.main;
    startingPosition = transform.position;
    targetPosition = startingPosition;
  }

  // Update is called once per frame


  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      //gets mouseposition to world space
      Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      // mousePosition.z = 0f;

      Collider2D overLap = Physics2D.OverlapPoint(mousePosition);

      if (overLap != null)
        {
            Debug.Log("Clicked object: " + overLap.name);
        }
        else
        {
            Debug.Log("Nothing clicked");
        }
    
      if (overLap != null && overLap.gameObject == gameObject && !isMoving)

      {
        if (isAtStart)
        {
          Vector3 moveDirection = Vector3.zero;

          if (moveLeft) moveDirection = Vector3.left;
          else if (moveRight) moveDirection = Vector3.right;
          else if (moveUp) moveDirection = Vector3.up;
          else if (moveDown) moveDirection = Vector3.down;

          if (moveDirection != Vector3.zero)
          {
            targetPosition = startingPosition + moveDirection * moveDistance;
            isMoving = true;
            isAtStart = false;
            Debug.Log("Moving Away");
          }
          else
          {
            Debug.LogWarning("No movement direction selected in Inspector!");
          }
        }
        else
        {
          targetPosition = startingPosition;
          isMoving = true;
          isAtStart = true;
          Debug.Log("Moving Back");

        }
            }
        }
     
      
      if (isMoving)
      {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
          isMoving = false;
        Debug.Log("Arrived at target");
        }
      }
    }
  }



