using System;
using UnityEngine;

public class Flag : MonoBehaviour, IMoverTarget
{
    [SerializeField] private GameObject _graphics;
    [SerializeField] private Vector3 _sizeRaycast;
    [SerializeField] private LayerMask _layerCollisionCanNotBuild;
    [SerializeField] private LayerMask _layerResources;

    public event Action<StateFlag> ChangedState;

    public bool CanBuilding { get; private set; }
    public StateFlag State { get; private set; }
    public Vector3 Position => transform.position;

    private void Update()
    {
        if (Physics.CheckBox(transform.position, _sizeRaycast, Quaternion.identity, _layerCollisionCanNotBuild))
        {
            CanBuilding = false;
        }
        else
        {
            CanBuilding = true;
        }
    }

    public void Free()
    {
        _graphics.SetActive(false);
        transform.localPosition = Vector3.zero;
        UpdateState(StateFlag.Free);
    }

    public void Select()
    {
        _graphics.SetActive(true);
        UpdateState(StateFlag.Selected);
    }

    public void Assign()
    {
        DestroyResources();
        UpdateState(StateFlag.Assigned);
    }

    private void UpdateState(StateFlag state)
    {
        if (State == state)
            return;

        State = state;
        ChangedState?.Invoke(state);
    }


    private void DestroyResources()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, _sizeRaycast, Quaternion.identity, _layerResources);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ResourceItem resourceItem) == false)
            {
                continue;
            }

            resourceItem.Destroy();
        }
    }
}
