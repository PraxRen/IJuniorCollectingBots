using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ResourceItem : Item, IPickUpItem, IDropItem
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private PointPickUp _pointPickUp;
    [SerializeField] private TypeTool _typeTool;
    [SerializeField] private int _countAttemptsFarm;
    [SerializeField] private float _timeProcess;

    private WaitForSeconds _waitTimeProcess;

    public event Action<ResourceItem> StartProcess;
    public event Action<ResourceItem> Processed;
    public event Action<ResourceItem> Destroyed;

    public bool IsStartedProcess { get; private set; }
    public bool IsProcessed { get; private set; }
    public Vector3 Position => transform.position;
    public IMoverTarget PointPickUp => _pointPickUp;
    public TypeTool TypeTool => _typeTool;

    private void Awake()
    {
        _waitTimeProcess = new WaitForSeconds(_timeProcess);
    }

    public void Farm()
    {
        if (IsProcessed) 
            return;

        _animator.SetTrigger(ItemAnimatorData.Params.Hit);

        _countAttemptsFarm--;

        if (_countAttemptsFarm <= 0)
        {
            Process();
        }
    }

    public void PickUp(Transform container)
    {
        _rigidbody.isKinematic = true;
        transform.parent = container;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        _collider.enabled = false;
    }

    public void Drop()
    {
        DisableKinematic();
        StartCoroutine(WaitEnableKinematic(null));
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void Process()
    {
        DisableKinematic();
        StartCoroutine(WaitEnableKinematic(() => 
        {
            IsProcessed = true;
            Processed?.Invoke(this);
        }));

        IsStartedProcess = true;
        StartProcess?.Invoke(this);
    }

    private void DisableKinematic()
    {
        transform.parent = null;
        _animator.enabled = false;
        _navMeshObstacle.enabled = false;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(transform.forward, ForceMode.Impulse);
    }

    private IEnumerator WaitEnableKinematic(Action afterEnableKinematic)
    {
        yield return _waitTimeProcess;
        _rigidbody.isKinematic = true;
        afterEnableKinematic?.Invoke();
    }
}