using UnityEngine;
using System.Collections;

public class PuzzleReveal : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Glow Settings")]
    public float glowIntensity = 2f;   // How bright the glow gets
    public float glowDuration = 0.5f;  // How long the glow lasts

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            // Start invisible
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
            sr.enabled = true;
        }
    }

    public void Reveal()
    {
        if (sr != null)
        {
            StopAllCoroutines();
            StartCoroutine(GlowUp());
        }
    }

    private IEnumerator GlowUp()
    {
        // Instantly make visible first
        Color baseColor = sr.color;
        baseColor.a = 1f;
        sr.color = baseColor;

        float elapsed = 0f;

        // Brighten up (glow up)
        while (elapsed < glowDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (glowDuration / 2f);
            Color glowColor = baseColor * Mathf.Lerp(1f, glowIntensity, t);
            glowColor.a = 1f;
            sr.color = glowColor;
            yield return null;
        }

        // Then return to normal
        elapsed = 0f;
        while (elapsed < glowDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (glowDuration / 2f);
            Color glowColor = baseColor * Mathf.Lerp(glowIntensity, 1f, t);
            glowColor.a = 1f;
            sr.color = glowColor;
            yield return null;
        }

        sr.color = baseColor; // Reset to normal at end
    }
}
