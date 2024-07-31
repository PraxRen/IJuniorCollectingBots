using System;
using UnityEngine;

public class StatePickUp : State
{
    private Picker _picker;

    protected override void InitializeAfterAddon()
    {
        if (AICharacter.TryGetComponent(out _picker) == false)
        {
            throw new ArgumentNullException(nameof(_picker));
        }
    }

    protected override void StartHandle()
    {
        _picker.Pickup();
    }

    protected override void Work() { }

    protected override bool CanHandle() => false;
}