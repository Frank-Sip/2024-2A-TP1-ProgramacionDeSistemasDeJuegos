using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRespawnManager : MonoBehaviour
{
    public void HandleBuildingDeath(Structures building)
    {
        StartCoroutine(RespawnBuilding(building));
    }

    private IEnumerator RespawnBuilding(Structures building)
    {
        yield return new WaitForSeconds(building.respawnCooldown);

        building.Respawn();
    }
}
