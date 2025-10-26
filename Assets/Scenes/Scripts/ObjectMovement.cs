using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
  [SerializeField] private float moveDistance = 2f;
  [SerializeField] private float moveSpeed = 3f;

  private Vector3 startingPosition;
  private Vector3 targetPosition;
  private bool isMoving = false;
  private bool movedLeft = false;
  private Camera 

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    startingPosition = transform.position;
    targetPosition = startingPosition;
  }

  // Update is called once per frame
 

    void Update()
  {
      if (Input.GetMouseButtonDown(0))
    {
        Vector3 mousePosition = 
       } 

      if (isMoving)
    {
        
        transform.position += Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //stop moving once we reach the target
      }
      if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
      {
        isMoving = false;

      }
    }
  }

  


