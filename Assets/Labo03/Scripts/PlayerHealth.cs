using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _pvs = 3; 
    [SerializeField] private Renderer armatureRenderer; 
    private Material[] playerMaterials;
    private float damageLevel = 0f;
    private bool _peutRecevoirDommage = true;
    private ScreenDamageEffect screenDamageEffect;

    private void Start()
    {
        if (armatureRenderer != null)
        {
            playerMaterials = armatureRenderer.materials;
            foreach (var material in playerMaterials)
            {
                material.SetFloat("_DamageLevel", 0f); 
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Armature Renderer not assigned.");
        }

        screenDamageEffect = FindObjectOfType<ScreenDamageEffect>();
    }

    private void PrendreDuDommage(int dommage)
    {
        if (!_peutRecevoirDommage) return;
        _peutRecevoirDommage = false;
        _pvs -= dommage;
        if (_pvs <= 0)
        {
            _pvs = 0;
            Mourir();
        }
        else
        {
            UiManager.Instance.UpdatePv(_pvs);
            screenDamageEffect.TriggerDamageEffect();
            TriggerHitEffect();

            if (_pvs == 1)
            {
                screenDamageEffect.StartHeartbeatEffect();
            }

            StartCoroutine(AttendreIFrames());
        }
    }

    private void TriggerHitEffect()
    {
        damageLevel = 1f; 
        foreach (var material in playerMaterials)
        {
            material.SetFloat("_DamageLevel", damageLevel); 
        }
        StartCoroutine(FadeHitEffect());
    }

    private IEnumerator FadeHitEffect()
    {
        while (damageLevel > 0f)
        {
            damageLevel -= Time.deltaTime / 2f;
            foreach (var material in playerMaterials)
            {
                material.SetFloat("_DamageLevel", Mathf.Clamp01(damageLevel));
            }
            yield return null;
        }
    }

    private void Mourir()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ennemi"))
        {
            PrendreDuDommage(1);
            _peutRecevoirDommage = false;
        }
    }

    private IEnumerator AttendreIFrames()
    {
        yield return new WaitForSeconds(2f);
        _peutRecevoirDommage = true;
    }
}
