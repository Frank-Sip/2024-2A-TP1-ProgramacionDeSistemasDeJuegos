using UnityEngine;
using Enemies;

[CreateAssetMenu(fileName = "VfxSO", menuName = "Vfx")]
public class VfxSO : ScriptableObject
{
    public RandomContainer<ParticleSystem> vfx;
}