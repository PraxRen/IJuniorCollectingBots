using System;

public class TransitionEmployeeChangedSettlement : Transition
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
        _employee.ChangedSettlement += SetNeedTransit;
    }

    protected override void DeactivateAddon()
    {
        _employee.ChangedSettlement -= SetNeedTransit;
    }
}
