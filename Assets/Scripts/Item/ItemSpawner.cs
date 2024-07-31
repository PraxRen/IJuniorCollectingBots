using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    private int NaxCountAttemptsFindPosition = 100;

    [SerializeField] private Item[] _prefabs;
    [SerializeField] private int _count;
    [SerializeField] private Vector3 _spawnAreaSize;
    [SerializeField] private float _maxSampleDistance;
    [NavMeshMask][SerializeField] private int _navMeshArea;

    private int counterAttemptsFindPosition;

    private void Start()
    {
        SpawnResources();
    }

    private void SpawnResources()
    {
        for (int i = 0; i < _count; i++)
        {
            Vector3 randomPosition;
            NavMeshHit hit = default;
            bool result = false;
            Item prefab = GetRandomPrefab();

            while (result == false)
            {
                if (counterAttemptsFindPosition >= NaxCountAttemptsFindPosition)
                {
                    throw new InvalidOperationException($"Невозможно найти место для спауна!");
                }

                randomPosition = GetRandomPosition();
                result = NavMesh.SamplePosition(randomPosition, out hit, _maxSampleDistance, _navMeshArea);
                counterAttemptsFindPosition++;
            }

            counterAttemptsFindPosition = 0;
            Instantiate(prefab, hit.position, Quaternion.identity);
        }
    }

    private Item GetRandomPrefab()
    {
        int randomIndex = UnityEngine.Random.Range(0, _prefabs.Length);
        return _prefabs[randomIndex];
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(-_spawnAreaSize.x, _spawnAreaSize.x), 0, UnityEngine.Random.Range(-_spawnAreaSize.z, _spawnAreaSize.z));
    }
}