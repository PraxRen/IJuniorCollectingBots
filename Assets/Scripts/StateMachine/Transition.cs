using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public event Action<Transition, StatusTransition> ChangedStatus;

    public StatusTransition Status { get; private set; }
    public State CurrentState { get; private set; }
    public State TargetState => _targetState;
    protected AICharacter AICharacter { get; private set; }

    public void Initialize(AICharacter aiCharacter, State currentState)
    {
        if (Status != StatusTransition.None)
        {
            throw new InvalidOperationException($"������ ������������� \"{nameof(Transition)}\"! \"{GetType().Name}\" � \"{nameof(State)} -{CurrentState.GetType().Name}\" ��� ���������������.");
        }

        if (aiCharacter == null)
        {
            throw new ArgumentNullException(nameof(aiCharacter));
        }

        if (currentState == null)
        {
            throw new ArgumentNullException(nameof(currentState));
        }

        AICharacter = aiCharacter;
        CurrentState = currentState;
        InitializeAddon();
        UpdateStatusTransition(StatusTransition.Initialized);
    }

    public void Activate()
    {
        if ((int)Status < (int)StatusTransition.Initialized)
        {
            string message = $"������ ��������� \"{nameof(Transition)}\"! \"{GetType().Name}\" �� ���������������.";
            throw new InvalidOperationException(message);
        }

        ActivateAddon();
        UpdateStatusTransition(StatusTransition.Activated);
    }

    public void Deactivate()
    {
        if ((int)Status < (int)StatusTransition.Activated)
        {
            string message = $"������ ����������� \"{nameof(Transition)}\"! \"{GetType().Name}\" �� �������.";
            throw new InvalidOperationException(message);
        }

        DeactivateAddon();
        UpdateStatusTransition(StatusTransition.Deactivated);
    }

    protected void SetNeedTransit() => UpdateStatusTransition(StatusTransition.NeedTransit);

    protected virtual void InitializeAddon() { }

    protected virtual void ActivateAddon() { }

    protected virtual void DeactivateAddon() { }

    private void UpdateStatusTransition(StatusTransition status)
    {
        if (status == Status)
            return;

        Status = status;
        ChangedStatus?.Invoke(this, Status);
    }
}