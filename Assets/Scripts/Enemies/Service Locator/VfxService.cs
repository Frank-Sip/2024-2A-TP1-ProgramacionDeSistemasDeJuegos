using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class VfxService : MonoBehaviour
{
    [SerializeField] private VfxSO vfxSO;
    
    private void OnEnable()
    {
        ServiceLocator.Instance.SetService("VfxService", this);
    }

    public RandomContainer<ParticleSystem> GetVfx()
    {
        return vfxSO.vfx;
    }
}