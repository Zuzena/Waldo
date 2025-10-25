using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController I { get; private set; }
    private CameraManager cameraManager;

    [Header("Goal")]
    [Tooltip("IDs required to finish this level (must match CollectibleItem.itemId).")]
    public List<string> requiredItemIds = new();

    [Header("Interactable")]
    public List<ItemInteract> interactableItems = new();

    [Header("Next Scene")]
    [Tooltip("Scene to load after zoom completes.")]
    public string nextSceneName;

    // Set of collected IDs (HashSet avoids duplicates)
    private readonly HashSet<string> collected = new();

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;

        if (cameraManager == null)
        {
            cameraManager = Object.FindAnyObjectByType<CameraManager>();
        }

        // Uncomment if you want to persist this controller across scenes !!not sure how we will handle this yet!!
        //DontDestroyOnLoad(gameObject);
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

    // Called by ClickCollector when a interactable was clicked
    public void Interact(ItemInteract item)
    {
        if (!interactableItems.Contains(item)) return;
        item.Clicked();
    }

    private IEnumerator CompleteLevel()
    {
        yield return StartCoroutine(cameraManager.ZoomToPainting());
        SceneManager.LoadScene(nextSceneName);
    }
}

