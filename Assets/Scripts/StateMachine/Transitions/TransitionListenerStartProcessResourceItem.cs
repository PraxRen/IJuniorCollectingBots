using System;

public class TransitionListenerStartProcessResourceItem : Transition
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
        if (_employee.CurrentResourceItem.IsStartedProcess)
        {
            OnStartProcess(_employee.CurrentResourceItem);
            return;
        }

        _employee.CurrentResourceItem.StartProcess += OnStartProcess;
    }

    protected override void DeactivateAddon()
    {
        _employee.CurrentResourceItem.StartProcess -= OnStartProcess;
    }

    private void OnStartProcess(ResourceItem item)
    {
        SetNeedTransit();
    }
}