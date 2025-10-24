using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraMovement: MonoBehaviour
{
    private new Camera camera;
    private Bounds bounds;

    private bool isZoomed = false;
    private bool canZoom = false;
    private float targetZoom;
    
    [Header("Camera Settings")]
    public float cursorFollowSpeed = 1.0f;

    public float zoomSpeed = 5.0f;
    public float zoomAmount = 0.6f;
    public float defaultZoom = 0.6f;

    [Header("Background info for camera bounds")]
    public SpriteRenderer background;

    [Header("Other")]
    public int level = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set camera and bounds from background
        camera = Camera.main;
        bounds = background.bounds;

        // zoom in at the begining
        camera.orthographicSize *= 0.6f;
        defaultZoom = camera.orthographicSize;
        targetZoom = defaultZoom;

        //prevent zoom in the begining
        canZoom = false;
    }

    // Update is called once per frame
    void Update()
    {
        // read value from scroll
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollValue) > 0.01f && canZoom)
        {
            // zoom in
            if (scrollValue > 0f && !isZoomed)
            {
                targetZoom = defaultZoom * zoomAmount;
                isZoomed = true;
            }
            // zoom out
            else if (scrollValue < 0f && isZoomed)
            {
                targetZoom = defaultZoom;
                isZoomed = false;
            }
            canZoom = false;
        }

        // enable zoom once back to original position
        if (Mathf.Abs(scrollValue) < 0.01f)
        {
            canZoom = true;
        }
 
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // get mouse position
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        mousePosition = camera.ScreenToWorldPoint(mousePosition);
        mousePosition.z = camera.transform.position.z;

        if (!isZoomed)
        {
            Vector3 cameraTarget = ClampCameraPosition(mousePosition);
            camera.transform.position = Vector3.Lerp(camera.transform.position, cameraTarget, Time.deltaTime * cursorFollowSpeed);
        }
    }

    private Vector3 ClampCameraPosition(Vector3 mousePosition)
    {
        Vector3 position = mousePosition;

        // get screen size
        float viewH = 2f * camera.orthographicSize;
        float viewW = viewH * camera.aspect;

        //limit camera movement from goin out of bounds
        float minX = bounds.min.x + viewW / 2f;
        float maxX = bounds.max.x - viewW / 2f;
        float minY = bounds.min.y + viewH / 2f;
        float maxY = bounds.max.y - viewH / 2f;

        if (bounds.size.x <= viewW)
        {
            position.x = bounds.center.x;
        }
        else
        {
            position.x = Mathf.Clamp(mousePosition.x, minX, maxX);
        }
            
        if (bounds.size.y <= viewH)
        {
            position.y = bounds.center.y;
        }
        else
        {
            position.y = Mathf.Clamp(mousePosition.y, minY, maxY);
        }
            
        return position;
    }
}
