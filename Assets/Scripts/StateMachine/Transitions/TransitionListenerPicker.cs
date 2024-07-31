using System;

public class TransitionListenerPicker : Transition
{
    private Picker _picker;

    protected override void InitializeAddon()
    {
        if (AICharacter.TryGetComponent(out _picker) == false)
        {
            throw new ArgumentNullException(nameof(_picker));
        }
    }

    protected override void ActivateAddon()
    {
        if(_picker.IsPicked)
        {
            OnPicked();
            return;
        }

        _picker.Picked += OnPicked;
    }

    protected override void DeactivateAddon()
    {
        _picker.Picked -= OnPicked;
    }

    private void OnPicked()
    {
        SetNeedTransit();
    }
}
