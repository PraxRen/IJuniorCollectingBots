using System;

public class StateFarming : State
{
    private Farming _farming;

    protected override void InitializeAfterAddon()
    {
        if (AICharacter.TryGetComponent(out _farming) == false)
        {
            throw new ArgumentNullException(nameof(_farming));
        }
    }

    protected override void ExitAfterAddon()
    {
        _farming.Cancel();
    }

    protected override void StartHandle() 
    {
        _farming.Run();
    }

    protected override bool CanHandle() => false;

    protected override void Work() { }
}