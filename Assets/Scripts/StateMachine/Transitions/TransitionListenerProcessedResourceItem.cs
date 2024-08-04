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
        if (_employee.CurrentResourceItem.IsProcessed)
        {
            OnProcessed(_employee.CurrentResourceItem);
            return;
        }

        _employee.CurrentResourceItem.Processed += OnProcessed;
    }

    protected override void DeactivateAddon()
    {
        _employee.CurrentResourceItem.Processed -= OnProcessed;
    }

    private void OnProcessed(ResourceItem item)
    {
        SetNeedTransit();
    }
}