using System;
using UnityEngine;

public class TransitionReachPointPickUp : TransitionReachMoverTarget
{
    private Employee _employee;

    protected override void InitializeAddon()
    {
        base.InitializeAddon();

        if (AICharacter is not Employee employee)
        {
            throw new InvalidCastException(nameof(employee));
        }

        _employee = employee;
    }

    protected override bool CanTransit()
    {
        bool result = Mover.Target == _employee.CurrentResourceItem.PointPickUp;
        return result;
    }
}
