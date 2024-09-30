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
        private StructureService structureService;
        private Transform target;
        [SerializeField] private float dmg;
        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };
    
        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            structureService = ServiceLocator.Instance.GetService("StructureService") as StructureService;
            poolManager = PoolManager.Instance;
            LookForClosestStructure();
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
            StartCoroutine(AlertSpawn());
        }

        private void LookForClosestStructure()
        {
            if (structureService != null)
            {
                Structures closestStructure = structureService.LookForClosestStructure(transform.position);
                if (closestStructure != null)
                {
                    target = closestStructure.transform;
                    Vector3 destination = target.position;
                    destination.y = transform.position.y;
                    StartCoroutine(SetDestinationToClosestBuildingAfterWaiting(destination));
                }
            }
        }
        
        private IEnumerator SetDestinationToClosestBuildingAfterWaiting(Vector3 destination)
        {
            if (target != null)
            {
                yield return new WaitForSeconds(2);
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
                    Debug.Log($"{name}: I'll die for my people!");
                    healthComponent.TakeDamage(dmg);
                    structure.healthComponent.TakeDamage(dmg);
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
