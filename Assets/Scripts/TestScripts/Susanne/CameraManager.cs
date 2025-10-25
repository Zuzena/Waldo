using System.Collections;         
using UnityEngine;                
using UnityEngine.InputSystem;    
using UnityEngine.SceneManagement;

public class CameraManager: MonoBehaviour
{
    private static CameraManager instance;
    private Camera camera;
    private Bounds bounds;

    private float StartZoom;
    private float targetZoom;
    private int level = 1;

    private bool isZoomed = false;
    private bool canZoom = false;
    private bool followUnlocked = false;
    private bool zoomUnlocked = false;

    private Transform paintingTarget;

    [Header("Zoom Into Painting")]
    [Tooltip("Final orthographic size (smaller = closer).")]
    public float targetOrthoSize = 2.5f;

    [Tooltip("Zoom animation duration (seconds).")]
    public float zoomDuration = 1.2f;

    [Header("Camera Settings")]
    public float cursorFollowSpeed = 1.0f;
    public float zoomSpeed = 5.0f;
    public float zoomAmount = 0.6f;
    public float defaultZoom = 0.6f;

    [Header("Background info for camera bounds")]
    public SpriteRenderer background;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //dont destroy camera! also set camera and safe startzoom for later use
        DontDestroyOnLoad(gameObject);
        camera = Camera.main;
        StartZoom = camera.orthographicSize;
    }

    void Start()
    {
        if (background != null)
        {
            bounds = background.bounds;
        }

        targetZoom = defaultZoom;
        canZoom = false;

        // uncomment if we get aspect problem solved
        //OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnEnable()
    {
        //scene change listener
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //update camera reference
        camera = Camera.main;

        //find the painting target
        GameObject targetObj = GameObject.FindWithTag("PaintingTarget");
        if (targetObj != null)
        {
            paintingTarget = targetObj.transform;
        }
        else
        {
            paintingTarget = null;
        }

        //find a new background and update bounds
        GameObject backgroundObj = GameObject.FindWithTag("Background");
        if (backgroundObj != null)
        {
            background = backgroundObj.GetComponent<SpriteRenderer>();
            bounds = background.bounds;
        }
        else
        {
            background = null;
        }

        // uncomment if we get aspect problem solved
        //switch (scene.name)
        //{
        //    case "MainMenu":
        //        // 4:3 for menus/UI, add other scene names if needed
        //        Screen.SetResolution(1024, 768, false);
        //        break;
        //    default:
        //        // 16:9 for levels
        //        Screen.SetResolution(1920, 1080, false);
        //        break;
        //}
    }
    
    void Update()
    {
        if (zoomUnlocked)
        {
            handleZoom();
        }
            
        if (followUnlocked)
        {
            followCursor();
        }
    }

    private void handleZoom()
    {
        // read scroll value
        float scrollValue = Mouse.current.scroll.ReadValue().y;

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
            // dont let player zoom while zoomed in
            canZoom = false;
        }

        // enable zoom once back to original position
        if (Mathf.Abs(scrollValue) < 0.01f)
        {
            canZoom = true;
        }

        // zoom in and out smoothly
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }

    private void followCursor()
    {
        // get mouse position (pixels)
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        //get mouse position (world space)
        mousePosition = camera.ScreenToWorldPoint(mousePosition);
        // keep z same for our camera
        mousePosition.z = camera.transform.position.z;

        if (!isZoomed)
        {
            // keep camera inside backround bounds
            Vector3 cameraTarget = ClampCameraPosition(mousePosition);
            // move camera towards mouse
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

    private void unlockCameraEffect()
    {
        //update level
        level++;
        switch (level)
        {
            // level 2 unlocks cursor following camera
            // level 3 unlocks camera zoom in and lock position during that
            case 2:
                // zoom camera a little to allow room for moving camera arround
                camera.orthographicSize = StartZoom * 0.6f; ;
                defaultZoom = camera.orthographicSize;
                targetZoom = defaultZoom;
                followUnlocked = true;
                break;
            case 3:
                zoomUnlocked = true;
                break;
        }
    }

    public IEnumerator ZoomToPainting()
    {
        Vector3 startPos = camera.transform.position;
        float startSize = camera.orthographicSize;

        // Keep camera Z (Need to check with cameracontroller) !!Need to check how will work with the camera manager!!
        Vector3 targetPos = new Vector3(
            paintingTarget.position.x,
            paintingTarget.position.y,
            camera.transform.position.z);

        float t = 0f;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / zoomDuration);

            camera.transform.position = Vector3.Lerp(startPos, targetPos, k);
            camera.orthographicSize = Mathf.Lerp(startSize, targetOrthoSize, k);

            yield return null;
        }

        unlockCameraEffect();
    }
}
