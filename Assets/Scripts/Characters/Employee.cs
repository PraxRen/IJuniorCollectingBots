using System;
using UnityEngine;

public class Employee : AICharacter
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Picker _picker;
    [SerializeField] private Thrower _thrower;

    public event Action ChangedTargetResourceItem;
    public event Action ChangedFlag;
    public event Action ChangedSettlement;

    public ISettlement Settlement { get; private set; }
    public ResourceItem CurrentResourceItem { get; private set; }
    public Flag Flag { get; private set; }

    public void Initialize(ISettlement settlement)
    {
        Settlement = settlement;
    }

    public bool HasRestState()
    {
        return State != null && State.Status == StatusState.Completed && State is IRestState;
    }

    public void TransitSettlement(ISettlement settlement)
    {
        Settlement.RemoveEmployee(this);
        settlement.AddEmployee(this);
        ChangedSettlement?.Invoke();
    }

    public void SetTargetItem(ResourceItem resourceItem)
    {
        CurrentResourceItem = resourceItem;
        _mover.SetTarget(resourceItem);
        _picker.SetCurrentPickUpItem(resourceItem);
        _thrower.SetCurrentDropItem(resourceItem);
        ChangedTargetResourceItem?.Invoke();
    }

    public void SetFlag(Flag flag)
    {
        if (HasRestState() == false)
        {
            throw new InvalidOperationException($"Невозможно установит флаг, в состоянии отлично от {nameof(IRestState)}!");
        }

        Flag = flag;
        _mover.SetTarget(Flag);
        ChangedFlag?.Invoke();
    }

    protected override void HandleExitState(IReadOnlyState state)
    {
        switch (state)
        {
            case StateFarming:
                _mover.SetTarget(CurrentResourceItem.PointPickUp);
                break;
            case StatePickUp:
                _mover.SetTarget(Settlement);
                break;
        }
    }
}