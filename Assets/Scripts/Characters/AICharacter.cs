using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICharacter : Character
{
    [SerializeField] private List<State> _states;

    private State _defaultState;
    private State _currentState;

    public IReadOnlyState State => _currentState;

    private void OnEnable()
    {
        SetCurrentState(_currentState);
    }

    private void OnDisable()
    {
        CurrentStateExit();
    }

    private void Start()
    {
        StartCoroutine(InitializeStates());
    }

    private IEnumerator InitializeStates()
    {
        if (_states == null || _states.Count == 0)
            yield break;

        foreach (State state in _states)
        {
            state.Initialize(this);
            yield return null;
        }

        _defaultState = _states[0];
        SetCurrentState(_defaultState);
    }

    private void SetCurrentState(State startState)
    {
        if (startState == null)
        {
            return;
        }

        _currentState = startState;
        _currentState.GetedNextState += Transit;
        _currentState.Enter();
        HandleEnterState(_currentState);
        _currentState.RunHandle();
    }

    private void Transit(State nextState)
    {
        if (_states.Contains(nextState) == false)
            return;

        CurrentStateExit();
        SetCurrentState(nextState);
    }

    private void CurrentStateExit()
    {
        if (_currentState == null)
        {
            return;
        }

        _currentState.GetedNextState -= Transit;
        _currentState.Exit();
        HandleExitState(_currentState);
    }

    protected virtual void HandleEnterState(IReadOnlyState state) { }

    protected virtual void HandleExitState(IReadOnlyState state) { }
}