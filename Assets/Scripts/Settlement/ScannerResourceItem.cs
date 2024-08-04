using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerResourceItem : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMaskItem;
    [SerializeField] private float _minRadiusFindItems;
    [SerializeField] private float _maxRadiusFindItems;
    [SerializeField] private float _stepRadiusFindItems;
    [SerializeField] private int _sizeResultForFindItems;
    [Range(0, 10)][SerializeField] private float _timeUpdateFindItems;

    private Coroutine _jobFindItems;
    private WaitForSeconds _waitUpdateFindItems;
    private float _radiusFindItems;
    private List<ResourceItem> _foundResourceItems = new List<ResourceItem>();

    public IReadOnlyCollection<ResourceItem> ResourceItems => _foundResourceItems;

    private void Awake()
    {
        _waitUpdateFindItems = new WaitForSeconds(_timeUpdateFindItems);
        _radiusFindItems = _minRadiusFindItems;
    }

    private void OnEnable()
    {
        _jobFindItems = StartCoroutine(FindResourceItems());
    }

    private void OnDisable()
    {
        CancelFindResourceItems();
    }

    private IEnumerator FindResourceItems()
    {
        Collider[] result = new Collider[_sizeResultForFindItems];

        while (true)
        {
            UpdatePotentialItems(result);

            yield return _waitUpdateFindItems;
        }
    }

    private void UpdatePotentialItems(Collider[] result)
    {
        int countItems = Physics.OverlapSphereNonAlloc(transform.position, _radiusFindItems, result, _layerMaskItem);

        if (countItems == 0)
        {
            ExpandRadius();
        }

        for (int i = 0; i < countItems; i++)
        {
            if (result[i].TryGetComponent(out ResourceItem resourceItem) == false)
            {
                Debug.LogWarning($"Вероятность неверной настройки слоя предметов! \"{nameof(ResourceItem)}\" не найден!");
                continue;
            }

            if (resourceItem.IsProcessed)
            {
                continue;
            }

            if (_foundResourceItems.Contains(resourceItem))
            {
                continue;
            }

            resourceItem.StartProcess += RemoveResourceItem;
            resourceItem.Destroyed += RemoveResourceItem;
            _foundResourceItems.Add(resourceItem);
        }
    }

    private void ExpandRadius()
    {
        _radiusFindItems = Mathf.Clamp(_radiusFindItems + _stepRadiusFindItems, _minRadiusFindItems, _maxRadiusFindItems);
    }

    private void RemoveResourceItem(ResourceItem resourceItem)
    {
        resourceItem.StartProcess -= RemoveResourceItem;
        resourceItem.Destroyed -= RemoveResourceItem;
        _foundResourceItems.Remove(resourceItem);
    }

    private void CancelFindResourceItems()
    {
        if (_jobFindItems != null)
        {
            StopCoroutine(_jobFindItems);
            _jobFindItems = null;
        }
    }
}
