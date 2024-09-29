using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Audio;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Audio")]
public class EnemyAudioSO : ScriptableObject
{
    public RandomContainer<AudioClipData> spawnClips;
    public RandomContainer<AudioClipData> explosionClips;
}
