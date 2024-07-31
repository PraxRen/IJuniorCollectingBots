using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _speed;

    public IMoverTarget Target { get; private set; }

    private void OnValidate()
    {
        if (_navMeshAgent == null)
            return;

        _navMeshAgent.speed = _speed;
    }

    private void Update()
    {
        UpdateAnimator();
    }

    public void MoveToPosition(Vector3 position)
    {
        _navMeshAgent.destination = position;
    }

    public void MoveToTarget()
    {
        _navMeshAgent.destination = Target.Position;
    }

    public void Stop()
    {
        if (_navMeshAgent.enabled == false)
        {
            return;
        }

        if(_navMeshAgent.isOnNavMesh == false)
        {
            return;
        }

        if (_navMeshAgent.isStopped)
        {
            return;
        }

        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        _navMeshAgent.isStopped = false;
    }

    public void SetTarget(IMoverTarget target)
    {
        Target = target;
    }

    public void ClearTarget()
    {
        Target = null;
    }

    public void LookAtTargetItem(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        transform.forward = direction;
    }

    private void UpdateAnimator()
    {
        float currentSpeed = _navMeshAgent.velocity.magnitude;
        _animator.SetFloat(CharacterAnimatorData.Params.Speed, currentSpeed);
    }
}
