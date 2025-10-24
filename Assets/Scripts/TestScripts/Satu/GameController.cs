using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    public static GameController I { get; private set; }

    [Header("Goal")]
    [Tooltip("IDs required to finish this level (must match CollectibleItem.itemId).")]
    public List<string> requiredItemIds = new();

    [Header("Zoom Into Painting")]
    [Tooltip("Transform at the center of the painting to zoom to.")]
    public Transform paintingTarget;

    [Tooltip("Final orthographic size (smaller = closer).")]
    public float targetOrthoSize = 2.5f;

    [Tooltip("Zoom animation duration (seconds).")]
    public float zoomDuration = 1.2f;

    [Header("Next Scene")]
    [Tooltip("Scene to load after zoom completes.")]
    public string nextSceneName;

    // Set of collected IDs (HashSet avoids duplicates)
    private readonly HashSet<string> collected = new();

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        // Uncomment if you want to persist this controller across scenes !!not sure how we will handle this yet!!
        // DontDestroyOnLoad(gameObject);
    }

    // Called by ClickCollector when a collectible was clicked
    public void TryCollect(CollectibleItem item)
    {
        // Ignore items that are not part of the goal for this level
        if (!requiredItemIds.Contains(item.itemId)) return;

        // Ignore duplicates
        if (!collected.Add(item.itemId)) return;

        // Trigger item feedback and optionally hide it
        item.Collected();

        

        // If all required items are collected, finish the level
        if (collected.Count >= requiredItemIds.Count)
            StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel()
    {
        yield return StartCoroutine(ZoomToPainting());
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator ZoomToPainting()
    {
        Camera cam = Camera.main;
        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        // Keep camera Z (Need to check with cameracontroller) !!Need to check how will work with the camera manager!!
        Vector3 targetPos = new Vector3(
            paintingTarget.position.x,
            paintingTarget.position.y,
            cam.transform.position.z);

        float t = 0f;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / zoomDuration);

            cam.transform.position = Vector3.Lerp(startPos, targetPos, k);
            cam.orthographicSize = Mathf.Lerp(startSize, targetOrthoSize, k);

            yield return null;
        }
    }
}

