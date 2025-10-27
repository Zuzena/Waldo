using UnityEngine;

public class Highlightable : MonoBehaviour
{
    [Tooltip("No need to assign this. Automatically searchs a child named Glow")]
    public GameObject glow;

    void Awake()
    {
        // Finds child named "Glow" if not assigned
        if (glow == null)
        {
            Transform t = transform.Find("Glow");
            if (t != null) glow = t.gameObject;
        }

        // Makes sure the Glow GameObject starts disabled (hidden)
        if (glow != null) glow.SetActive(false);
    }

    // Show or hide the highlight (Glow object)
    public void SetHighlight(bool on)
    {
        if (glow != null) glow.SetActive(on);
    }
}
