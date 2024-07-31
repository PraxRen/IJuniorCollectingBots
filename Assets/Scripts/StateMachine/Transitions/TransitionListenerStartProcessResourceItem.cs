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
        if (_employee.TargetItem.IsStartedProcess)
        {
            OnStartProcess(_employee.TargetItem);
            return;
        }

        _employee.TargetItem.StartProcess += OnStartProcess;
    }

    protected override void DeactivateAddon()
    {
        _employee.TargetItem.StartProcess -= OnStartProcess;
    }

    private void OnStartProcess(ResourceItem item)
    {
        SetNeedTransit();
    }
}