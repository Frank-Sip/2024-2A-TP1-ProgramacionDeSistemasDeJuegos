using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Audio;
using Enemies;

public class ServiceLocatorSfx : MonoBehaviour
{
    [SerializeField] private EnemyAudioSO audioSO;
    [SerializeField] private AudioPlayer audioPrefab;

    private void OnEnable()
    {
        ServiceLocator.Instance.SetService("ServiceLocatorSfx", this);
    }

    public RandomContainer<AudioClipData> GetSpawn()
    {
        return audioSO.spawnClips;
    }
    
    public RandomContainer<AudioClipData> GetExplosion()
    {
        return audioSO.explosionClips;
    }

    public AudioPlayer GetAudioPrefab()
    {
        return audioPrefab;
    }
}
