using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Click events")]
    public UnityEvent onClicked;

    [Header("Moving objects")]
    public bool movingObject;
    public float moveDistance = 2f;
    public float moveSpeed = 3f;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool movedLeft = false;
    private Camera camera;

    // Called by GameController when item was interacted with
    public void Clicked()
    {
        onClicked?.Invoke();
        // TODO: add here object movement on click
        // TODO: add object movement back to its original position
    }
}

