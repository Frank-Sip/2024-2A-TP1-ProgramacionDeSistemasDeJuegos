using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, IDeathLogic
    {
        public NavMeshAgent agent;
        private Transform townCenter;
        private PoolManager poolManager;
        public HealthComponent healthComponent;
        private Structures structure;
        [SerializeField] private float dmg;
        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };
    
        private void Reset() => FetchComponents();

        private void Awake()
        {
            structure = LookForClosestStructure()?.GetComponent<Structures>();
            FetchComponents();
            poolManager = PoolManager.Instance;
        }
    
        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
            healthComponent ??= GetComponent<HealthComponent>();
        }
        
        public void SetPoolManager(PoolManager manager)
        {
            poolManager = manager;
        }
        
        public void SpawnObject(Vector3 spawnPosition)
        {
            if (!agent.isActiveAndEnabled)
            {
                agent.enabled = true;
            }
            agent.Warp(spawnPosition);
            
            healthComponent.HealToMax();
            SetTargetToAliveStructure();
            StartCoroutine(AlertSpawn());
        }

        private Transform LookForClosestStructure()
        {
            Structures[] structures = FindObjectsOfType<Structures>();
            Transform closestStructure = null;
            float minDistance = Mathf.Infinity; //Infinity para evitar errores por la distancia
            
            foreach (Structures structure in structures)
            {
                if (structure.healthComponent.CurrentHealth > 0)
                {
                    float distance = Vector3.Distance(structure.transform.position, transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestStructure = structure.transform;
                    }
                }
            }

            return closestStructure;
        }
        
        private void SetTargetToAliveStructure()
        {
            Transform nearestStructure = LookForClosestStructure();
            if (nearestStructure != null)
            {
                structure = nearestStructure.GetComponent<Structures>();
                Vector3 destination = nearestStructure.position;
                destination.y = transform.position.y;
                agent.SetDestination(destination);
            }
        }

        private IEnumerator AlertSpawn()
        {
            yield return null;
            OnSpawn();
        }

        private void Update()
        {
            if (structure != null && structure.healthComponent.CurrentHealth > 0)
            {
                if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
                {
                    healthComponent.TakeDamage(dmg);
                    structure.healthComponent.TakeDamage(dmg);

                    if (!agent.hasPath)
                    {
                        healthComponent.TakeDamage(dmg);
                    }
                }
            }
        }

        public void Die()
        {
            OnDeath();
            Recycle();
        }
        
        private void Recycle()
        {
            if (poolManager != null)
            {
                poolManager.RecycleObject(gameObject);
            }
        }
        
        private void OnDisable()
        {
            if (agent != null)
            {
                agent.enabled = false;
            }
        }
    }
}
