using System;
using UnityEngine;

public class StateMoveToTarget : State
{
    private Mover _mover;
    private Vector3 _lastTargetPosition;

    protected override void InitializeAfterAddon()
    {
        if (AICharacter.TryGetComponent(out _mover) == false)
        {
            throw new ArgumentNullException(nameof(Mover));
        }
    }

    protected override void EnterBeforeAddon()
    {
        if (_mover.Target == null)
        {
            throw new ArgumentNullException(nameof(_mover.Target));
        }
    }

    protected override void ExitAfterAddon()
    {
        _mover.Stop();
    }

    protected override void Work() 
    {
        if (_lastTargetPosition == _mover.Target.Position)
            return;

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        _lastTargetPosition = _mover.Target.Position;
        _mover.MoveToTarget();
    }
}
