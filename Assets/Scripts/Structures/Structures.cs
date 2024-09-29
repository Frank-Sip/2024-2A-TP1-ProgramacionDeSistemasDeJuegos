using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : MonoBehaviour, IDeathLogic
{
    public HealthComponent healthComponent;
    [SerializeField] private float respawnCooldown = 3f;
    private MeshRenderer mesh;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        mesh = GetComponent<MeshRenderer>();
    }

    public void Die()
    {
        gameObject.SetActive(false);
        //mesh.enabled = false;
        //StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnCooldown);
        mesh.enabled = true;
        healthComponent.HealToMax();
    }
}
