using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using static System.Net.Mime.MediaTypeNames;

public class ScreenDamageEffect : MonoBehaviour
{
    [SerializeField] private PostProcessVolume damageVolume;
    [SerializeField] private UnityEngine.UI.Image bloodOverlay;
    [SerializeField] private float effectDuration = 1f;
    [SerializeField] private float pulseSpeed = 0.5f;

    private Coroutine damageEffectCoroutine;
    private Coroutine heartbeatCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (damageVolume != null)
        {
            damageVolume.enabled = false;
        }

        if (bloodOverlay != null)
        {
            bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, 0f); 
        }
    }

    public void TriggerDamageEffect()
    {
        if (damageEffectCoroutine != null)
        {
            StopCoroutine(damageEffectCoroutine);
        }
        damageEffectCoroutine = StartCoroutine(ApplyDamageEffect());
    }

    public void StartHeartbeatEffect()
    {
        heartbeatCoroutine = StartCoroutine(PulseDamageEffect());
    }


    private IEnumerator ApplyDamageEffect()
    {
        if (damageVolume != null)
        {
            damageVolume.enabled = true;
        }

        if (bloodOverlay != null)
        {
            Color overlayColor = bloodOverlay.color;
            overlayColor.a = 0.5f; 
            bloodOverlay.color = overlayColor;
        }

        // Wait
        yield return new WaitForSeconds(effectDuration);

        float fadeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            if (bloodOverlay != null)
            {
                Color overlayColor = bloodOverlay.color;
                overlayColor.a = Mathf.Lerp(0.5f, 0f, elapsed / fadeDuration);
                bloodOverlay.color = overlayColor;
            }
            yield return null;
        }

        if (damageVolume != null)
        {
            damageVolume.enabled = false;
        }

        if (bloodOverlay != null)
        {
            bloodOverlay.color = new Color(bloodOverlay.color.r, bloodOverlay.color.g, bloodOverlay.color.b, 0f); 
        }
    }

    private IEnumerator PulseDamageEffect()
    {
        while (true)
        {
            if (damageVolume != null)
            {
                damageVolume.enabled = true;
            }

            if (bloodOverlay != null)
            {
                Color overlayColor = bloodOverlay.color;
                overlayColor.a = 0.5f; 
                bloodOverlay.color = overlayColor;
            }

            yield return new WaitForSeconds(pulseSpeed);

            if (damageVolume != null)
            {
                damageVolume.enabled = false;
            }

            if (bloodOverlay != null)
            {
                Color overlayColor = bloodOverlay.color;
                overlayColor.a = 0f;
                bloodOverlay.color = overlayColor;
            }

            yield return new WaitForSeconds(pulseSpeed);
        }
    }
}
