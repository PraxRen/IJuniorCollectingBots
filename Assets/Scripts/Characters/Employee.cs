using System;
using UnityEngine;

public class Employee : AICharacter
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Picker _picker;
    [SerializeField] private Thrower _thrower;

    public event Action ChangedTarget;

    public ISettlement Settlement { get; private set; }
    public ResourceItem TargetItem { get; private set; }

    public void Initialize(ISettlement settlement)
    {
        Settlement = settlement;
    }

    public bool HasRestState()
    {
        return State.Status == StatusState.Completed && State is IRestState;
    }

    public void SetTargetItem(ResourceItem resourceItem) 
    {
        TargetItem = resourceItem;
        _mover.SetTarget(resourceItem);
        _picker.SetCurrentPickUpItem(resourceItem);
        _thrower.SetCurrentDropItem(resourceItem);
        ChangedTarget?.Invoke();
    }

    public void ClearTargetItem() 
    {
        TargetItem = null;
    }

    protected override void HandleExitState(IReadOnlyState state)
    {
        switch (state)
        {
            case StateFarming:
                _mover.SetTarget(TargetItem.PointPickUp);
                break;
            case StatePickUp:
                _mover.SetTarget(Settlement);
                break;
        }
    }
}