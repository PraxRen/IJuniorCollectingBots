using System;

public class StateDrop : State
{
    private Thrower _thrower;

    protected override void InitializeAfterAddon()
    {
        if (AICharacter.TryGetComponent(out _thrower) == false)
        {
            throw new ArgumentNullException(nameof(_thrower));
        }
    }

    protected override void StartHandle()
    {
        _thrower.Drop();
    }

    protected override void Work() { }

    protected override bool CanHandle() => false;
}