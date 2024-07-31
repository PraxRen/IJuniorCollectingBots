using System;

public class TransitionListenerThrower : Transition
{
    private Thrower _thrower;

    protected override void InitializeAddon()
    {
        if (AICharacter.TryGetComponent(out _thrower) == false)
        {
            throw new ArgumentNullException(nameof(_thrower));
        }
    }

    protected override void ActivateAddon()
    {
        if (_thrower.IsDropped)
        {
            OnDropped();
            return;
        }

        _thrower.Dropped += OnDropped;
    }

    protected override void DeactivateAddon()
    {
        _thrower.Dropped -= OnDropped;
    }

    private void OnDropped()
    {
        SetNeedTransit();
    }
}
