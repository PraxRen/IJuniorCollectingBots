using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State : MonoBehaviour, IReadOnlyState
{
    [Range(0, 5)][SerializeField] private float _timeWaitHandle;
    [SerializeField] private List<Transition> _transitions;

    private WaitForSeconds _waitForSeconds;
    private Coroutine _jobHandle;
    private Coroutine _jobWaitStatusEnteredForTransit;

    public event Action<StatusState> ChangedStatus;
    public event Action<State> GetedNextState;

    public StatusState Status { get; private set; }
    protected AICharacter AICharacter { get; private set; }
    protected IReadOnlyCollection<Transition> Transitions => _transitions;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_timeWaitHandle);
    }

    public void Initialize(AICharacter aiCharacter)
    {
        if (Status != StatusState.None)
            throw new InvalidOperationException($"—осто€ние \"{GetType().Name}\" уже инициализировано!");

        InitializeBeforeAddon(aiCharacter);
        AICharacter = aiCharacter;

        foreach (var transition in _transitions)
        {
            transition.Initialize(aiCharacter, this);
        }

        InitializeAfterAddon();
        UpdateStatus(StatusState.Initialized);
    }

    public void Enter()
    {
        if (Status != StatusState.Initialized && Status != StatusState.Exited)
            throw new InvalidOperationException($"Ќевозможно изменить статус состо€ни€ \"{GetType().Name}\" на \"{StatusState.Entered}\"!");

        EnterBeforeAddon();

        foreach (var transition in _transitions)
        {
            transition.ChangedStatus += OnChangedTransitionStatus;
            transition.Activate();
        }

        EnterAfterAddon();
        UpdateStatus(StatusState.Entered);
    }

    public void RunHandle()
    {
        if (_jobHandle != null)
            throw new InvalidOperationException($"ќбработка состо€ни€ \"{GetType().Name}\" уже запущена!");

        if (Status != StatusState.Entered)
            throw new InvalidOperationException($"Ќевозможно запустить обработку состо€ни€ \"{GetType().Name}\"!");

        _jobHandle = StartCoroutine(Handle());
    }

    public void Exit()
    {
        if ((int)Status < (int)StatusState.Entered)
            throw new InvalidOperationException($"Ќевозможно изменить статус состо€ни€ \"{GetType().Name}\" на \"{StatusState.Exited}\"!");

        ExitBeforeAddon();

        foreach (var transition in _transitions)
        {
            transition.ChangedStatus -= OnChangedTransitionStatus;
            transition.Deactivate();
        }

        StopHandle();
        ExitAfterAddon();
        UpdateStatus(StatusState.Exited);
    }

    protected abstract void Work();

    protected virtual void InitializeBeforeAddon(AICharacter aiCharacter) { }

    protected virtual void InitializeAfterAddon() { }

    protected virtual void EnterBeforeAddon() { }

    protected virtual void EnterAfterAddon() { }

    protected virtual void ExitBeforeAddon() { }

    protected virtual void ExitAfterAddon() { }

    protected virtual bool CanHandle() => true;

    protected virtual void StartHandle() { }

    private void StopHandle()
    {
        if (_jobHandle == null)
            return;

        StopCoroutine(_jobHandle);
        CompleteHandle();
    }

    private void CompleteHandle()
    {
        _jobHandle = null;

        if ((int)Status < (int)StatusState.Completed)
        {
            UpdateStatus(StatusState.Completed);
        }
    }

    private IEnumerator Handle()
    {
        StartHandle();

        while (Status == StatusState.Entered && CanHandle())
        {
            Work();
            yield return _waitForSeconds;
        }

        CompleteHandle();
    }

    private void UpdateStatus(StatusState state)
    {
        if (Status == state)
        {
            return;
        }

        Status = state;
        ChangedStatus?.Invoke(Status);
    }

    private void OnChangedTransitionStatus(Transition transition, StatusTransition statusTransition)
    {
        if (statusTransition != StatusTransition.NeedTransit)
            return;

        if (Status != StatusState.Entered && Status != StatusState.Completed)
        {
            if (_jobWaitStatusEnteredForTransit != null)
                return;

            _jobWaitStatusEnteredForTransit = StartCoroutine(WaitStatusEnteredForTransit(transition));

            return;
        }

        GetedNextState?.Invoke(transition.TargetState);
    }

    private IEnumerator WaitStatusEnteredForTransit(Transition transition)
    {
        while (Status != StatusState.Entered && Status != StatusState.Completed)
        {
            yield return null;
        }

        GetedNextState?.Invoke(transition.TargetState);
        _jobWaitStatusEnteredForTransit = null;
    }
}