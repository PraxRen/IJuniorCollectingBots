using System;

public class TransitionEmployeeChangedTargetItem : Transition
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
        _employee.ChangedTarget += SetNeedTransit;        
    }

    protected override void DeactivateAddon() 
    {
        _employee.ChangedTarget -= SetNeedTransit;
    }
}