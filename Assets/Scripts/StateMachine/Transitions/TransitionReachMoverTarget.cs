using System;
using System.Collections;
using UnityEngine;

public class TransitionReachMoverTarget : Transition
{
    [SerializeField] private float _distance;

    protected Mover Mover {  get; private set; }
    private Coroutine _jobUpdateDistance;
    private float _sqrDistance;

    protected override void InitializeAddon()
    {
        if (AICharacter.TryGetComponent(out Mover mover) == false)
        {
            throw new ArgumentNullException(nameof(mover));
        }

        Mover = mover;
        _sqrDistance = _distance * _distance;
    }

    protected override void ActivateAddon()
    {
        if (Mover.Target == null)
        {
            throw new ArgumentNullException(nameof(Mover.Target));                
        }

        _jobUpdateDistance = StartCoroutine(UpdateDistance());
    }

    protected override void DeactivateAddon() 
    {
        if (_jobUpdateDistance != null)
        {
            StopCoroutine(_jobUpdateDistance);
            _jobUpdateDistance = null;
        }
    }

    private IEnumerator UpdateDistance()
    {
        while (Status != StatusTransition.NeedTransit) 
        {
            float currentDistanceSqr = (Mover.Target.Position - AICharacter.transform.position).sqrMagnitude;

            if (currentDistanceSqr < _sqrDistance && CanTransit())
            {
                SetNeedTransit();
            }
            
            yield return null;
        }

        _jobUpdateDistance = null;
    }

    protected virtual bool CanTransit() => true;
}