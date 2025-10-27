using UnityEngine;

public class PuzzleReveal : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false; // Hidden at start
    }

    public void Reveal()
    {
        if (sr != null)
            sr.enabled = true;
        else
            gameObject.SetActive(true);
    }
}
