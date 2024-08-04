using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEmployeeChangedFlag : Transition
{
    private Employee _employee;

    protected override void InitializeAddon()
    {
        if (AICharacter is not Employee employee)
        {
            throw new InvalidCastException(nameof(AICharacter));
        }

        _employee = employee;
    }

    protected override void ActivateAddon()
    {
        _employee.ChangedFlag += SetNeedTransit;
    }

    protected override void DeactivateAddon()
    {
        _employee.ChangedFlag -= SetNeedTransit;
    }
}
