using System;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Mover _mover;

    private IDropItem _dropItem;

    public event Action Dropped;

    public bool IsDropped { get; private set; }

    public void SetCurrentDropItem(IDropItem dropItem)
    {
        _dropItem = dropItem;
        IsDropped = false;
    }

    public void Drop()
    {
        int indexLayer = _animator.GetLayerIndex(TypeAnimationLayer.Picker.ToString());
        _animator.SetLayerWeight(indexLayer, 0f);
        _dropItem.Drop();
        IsDropped = true;
        Dropped?.Invoke();
    }
}
