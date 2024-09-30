using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureService : MonoBehaviour
{
    private List<Structures> structures = new List<Structures>();

    private void Awake()
    {
        ServiceLocator.Instance.SetService("StructureService", this);
    }

    public void RegisterStructure(Structures structure)
    {
        if (!structures.Contains(structure))
        {
            structures.Add(structure);
        }
    }

    public void UnregisterStructure(Structures structure)
    {
        if (structures.Contains(structure))
        {
            structures.Remove(structure);
        }
    }

    public Structures LookForClosestStructure(Vector3 position)
    {
        Structures closestStructure = null;
        float closestDistance = Mathf.Infinity; //Infinity para evitar errores por la distancia

        foreach (var structure in structures)
        {
            float distance = Vector3.Distance(position, structure.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestStructure = structure;
            }
        }

        return closestStructure;
    }
}
