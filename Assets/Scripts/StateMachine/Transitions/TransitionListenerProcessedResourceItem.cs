using System;

public class TransitionListenerProcessedResourceItem : Transition
{
    private Employee _employee;

    protected override void InitializeAddon()
    {
        if (AICharacter is not Employee employee)
        {
            throw new InvalidCastException(nameof(employee));
        }

        _employee = employee;
    }

    protected override void ActivateAddon()
    {
        if (_employee.TargetItem.IsProcessed)
        {
            OnProcessed(_employee.TargetItem);
            return;
        }

        _employee.TargetItem.Processed += OnProcessed;
    }

    protected override void DeactivateAddon()
    {
        _employee.TargetItem.Processed -= OnProcessed;
    }

    private void OnProcessed(ResourceItem item)
    {
        SetNeedTransit();
    }
}