using UnityEngine;
using UnityEngine.Events;

public class ItemInteract : MonoBehaviour
{
    // !!Later use for effects!!
    public UnityEvent onClicked;

    // Called by GameController when item was interacted with
    public void Clicked()
    {
        onClicked?.Invoke();
        // TODO: add here object movement on click
        // TODO: add object movement back to its original position
    }
}

