using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Click events")]
    public UnityEvent onClicked;

    [Header("Movement Setting")]
    public bool movingObject;
    public float moveDistance = 2f;
    public float moveSpeed = 3f;
    
    [Header("Movement Direction")]
    public bool moveLeft;
    public bool moveRight;
    public bool moveUp;
    public bool moveDown;

    [Header("Rotation Setting")]
    public bool canRotate;
    public float tiltAngle = 15f;
    public float rotationSpeed = 180f;

    [Header("Rotation Direction")]
    public bool rotateLeft;
    public bool rotateRight;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private Vector3 moveDirection;
    private bool isMoving = false;
    private bool isAtStart = true;
    private float targetAngle;
    private float startingAngle;
    private Camera mainCamera;
    private Collider2D myCol;

    void Start()
    {
        mainCamera = Camera.main;
        startingPosition = transform.position;
        startingAngle = transform.eulerAngles.z;
        targetPosition = startingPosition;
        targetAngle = startingAngle;
        myCol = GetComponent<Collider2D>();   // Save my collider
    }

    private void HandleMovement()
    {
        if (isAtStart)
        {
            if (moveLeft) moveDirection = Vector3.left;
            else if (moveRight) moveDirection = Vector3.right;
            else if (moveUp) moveDirection = Vector3.up;
            else if (moveDown) moveDirection = Vector3.down;
            else
            {
                Debug.LogWarning("No movement direction selected in Inspector!");
                moveDirection = Vector3.zero;
            }

            if (moveDirection != Vector3.zero)
            {
                targetPosition = startingPosition + moveDirection * moveDistance;
                isAtStart = false;
                //Debug.Log("Moving Away");
            }
        }
        else
        {
            targetPosition = startingPosition;
            //Debug.Log("Moving Back");
        }

        isMoving = true;
    }

    private void HandleRotation()
    {
        float tiltDir = 0f;

        if (rotateLeft) tiltDir = tiltAngle;
        else if (rotateRight) tiltDir = -tiltAngle;

        if (isAtStart)
        {
            targetAngle = startingAngle + tiltDir;
        }
        else
        {
            targetAngle = startingAngle;
        }

        isMoving = true;
    }

    private void Clicked()
    {
        // Unity event if one is set in the inspector
        onClicked?.Invoke();

        // fallback if camera reference is missing
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        //gets mouseposition to world space
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // prefer checking this object's own collider so clicks don't pass through other objects
        var selfCol = GetComponent<Collider2D>();
        bool hitSelf = selfCol != null && selfCol.OverlapPoint(mousePosition);

        Collider2D overLap = Physics2D.OverlapPoint(mousePosition);
        //if (overLap != null)
        //{
        //    Debug.Log("Clicked object: " + overLap.name);
        //}
        //else
        //{
        //    Debug.Log("Nothing clicked");
        //}

        // accept click if we hit our own collider OR (fallback) Physics2D reported this exact GameObject
        if ((hitSelf || (overLap != null && overLap.gameObject == gameObject)) && !isMoving)
        {
            // audio safety: only play if manager exists and the index is valid
            if (S_AudioManager.instance != null && S_AudioManager.instance.sfxSounds != null)
            {
                if (isAtStart && S_AudioManager.instance.sfxSounds.Length > 4)
                    S_AudioManager.instance.PlaySFX(S_AudioManager.instance.sfxSounds[4]);
                else if (!isAtStart && S_AudioManager.instance.sfxSounds.Length > 3)
                    S_AudioManager.instance.PlaySFX(S_AudioManager.instance.sfxSounds[3]);
            }

            if (movingObject) HandleMovement();
            else if (canRotate) HandleRotation();
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // handle input
            Clicked();
        }

        if (isMoving)
        {
            if (movingObject)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    isMoving = false;
                    if (targetPosition == startingPosition) isAtStart = true;

                    //Debug.Log("Arrived at target");
                }
            }
            else if (canRotate)
            {
                float currentZ = transform.eulerAngles.z;
                float newAngle = Mathf.MoveTowardsAngle(currentZ, targetAngle, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, newAngle);

                if (Mathf.Abs(Mathf.DeltaAngle(currentZ, targetAngle)) < 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                    isMoving = false;
                    isAtStart = (Mathf.Abs(Mathf.DeltaAngle(targetAngle, startingAngle)) < 0.1f);
                }
            }
        }
    }
}