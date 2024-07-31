using System;
using System.Collections;
using UnityEngine;

public class StorageSettlementResources : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private LayerMask _itemMask;
    [SerializeField] private Transform _pointCenter;
    [SerializeField] private Vector3 _size = new Vector3(10, 10, 10);
    [SerializeField] private Vector3 _sizeResource = new Vector3(2, 2, 2);
    [SerializeField] private float _timeWaitFindResource;

    private Coroutine _jobFindResource;
    private WaitForSeconds _waitFindResource;
    private Vector3[,,] _hashPositions;
    private Vector3 _indexesPositions = new Vector3(0, 0, 0);

    private void Awake()
    {
        _waitFindResource = new WaitForSeconds(_timeWaitFindResource);
        CalculatePositions();
    }

    private void OnEnable()
    {
        _jobFindResource = StartCoroutine(FindResource());
    }

    private void OnDisable()
    {
        if (_jobFindResource != null)
        {
            StopCoroutine(_jobFindResource);
            _jobFindResource = null;
        }
    }

    private IEnumerator FindResource()
    {
        while (true)
        {
            Collider[] result = Physics.OverlapBox(_pointCenter.position, _size / 2, Quaternion.identity, _itemMask);

            foreach (Collider collider in result)
            {
                if (collider.TryGetComponent(out ResourceItem resourceItem)== false)
                {
                    continue;
                }

                if (resourceItem.IsProcessed == false)
                {
                    continue;
                }

                Add(resourceItem);
            }

            yield return _waitFindResource;
        }
    }

    private void Add(ResourceItem resourceItem)
    {
        resourceItem.PickUp(transform);
        resourceItem.transform.localPosition = GetPosition();
        resourceItem.transform.localEulerAngles = new Vector3(-90, 0, 0);
        _storage.AddItem(resourceItem);
    }

    private Vector3 GetPosition()
    {
        Vector3 newPosition = _hashPositions[(int)_indexesPositions.x, (int)_indexesPositions.y, (int)_indexesPositions.z];
        _indexesPositions.x++;

        if (_indexesPositions.x > _hashPositions.GetLength(0) - 1)
        {
            _indexesPositions.x = 0;
            _indexesPositions.z++;

            if (_indexesPositions.z > _hashPositions.GetLength(2) - 1)
            {
                _indexesPositions.z = 0;
                _indexesPositions.y++;

                if (_indexesPositions.y > _hashPositions.GetLength(1) - 1)
                {
                    _size.y++;
                    CalculatePositions();
                    Debug.LogWarning("Внимание произведено автоматическое расширение графического хранилища ресурсов!");
                }
            }
        }

        return newPosition;
    }

    private void CalculatePositions()
    {
        Vector3 centerResource = (_sizeResource / 2);
        _hashPositions = new Vector3[(int)(_size.x / _sizeResource.x), (int)(_size.y / _sizeResource.y), (int)(_size.z / _sizeResource.z)];

        for (int x = 0; x < _hashPositions.GetLength(0); x++)
        {
            for (int y = 0; y < _hashPositions.GetLength(1); y++)
            {
                for (int z = 0; z < _hashPositions.GetLength(2); z++)
                {
                    _hashPositions[x, y, z] = new Vector3((x + 1) * _sizeResource.x - centerResource.x, (y + 1) * _sizeResource.y - centerResource.y, (z + 1) * _sizeResource.z - centerResource.z);
                }
            }
        }
    }
}
