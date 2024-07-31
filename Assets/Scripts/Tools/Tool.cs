using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private ResourceItem _target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourceItem item) == false)
            return;

        if (_target != item) 
            return;

        item.Farm();
    }

    public void SetTarget(ResourceItem target)
    {
        _target = target;
    }

    public void ClearTarget()
    {
        _target = null;
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider() 
    {
        _collider.enabled = false;
    }
}