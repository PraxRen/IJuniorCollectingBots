using System;

public class TransitionReachResourceItem : TransitionReachMoverTarget
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
        return Mover.Target == (IMoverTarget)_employee.TargetItem;
    }
}
