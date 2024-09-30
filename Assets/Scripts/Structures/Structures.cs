using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Structures : MonoBehaviour, IDeathLogic
{
    public HealthComponent healthComponent;
    public float respawnCooldown = 3f;
    
    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    private void OnEnable()
    {
        var structureService = ServiceLocator.Instance.GetService("StructureService") as StructureService;
        structureService.RegisterStructure(this);
    }

    public void Die()
    {
        var structureService = ServiceLocator.Instance.GetService("StructureService") as StructureService;
        structureService.UnregisterStructure(this);
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnCooldown);

        gameObject.SetActive(true);
        healthComponent.HealToMax();
    }
}
