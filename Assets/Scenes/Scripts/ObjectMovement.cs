using UnityEngine;

public class ObjectMovement : MonoBehaviour

     [SerializeField] private float moveDistance = 2f;
     [SerializeField] private float moveSpeed = 3f;
    
    private Vector3 targetPosition;
    private bool isMoving = false;
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {           
    targetPosition = transform.position;
  }

  // Update is called once per frame
  void onMouseDown()
  {
    targetPosition = transform.position + Vector3.left * moveDistance;
    isMoving = true;
  }
  void Update()
  {
    if (isMoving)
    {
      transform.position += Vector3.left * moveDistance;
      }
    }
  }


