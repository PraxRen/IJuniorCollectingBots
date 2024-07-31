using UnityEngine;

public class Farming : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ActivatorTool _activatorTool;
    [SerializeField] private Employee _employee;
    [SerializeField] private Mover _mover;


    public void Run()
    {
        _mover.LookAtTargetItem(_employee.TargetItem.Position);
        _activatorTool.Activate(_employee.TargetItem);
        _animator.SetBool(CharacterAnimatorData.Params.IsFarm, true);
    }

    public void Cancel()
    {
        _activatorTool.Deactivate(_employee.TargetItem);
        _animator.SetBool(CharacterAnimatorData.Params.IsFarm, false);
    }
}
