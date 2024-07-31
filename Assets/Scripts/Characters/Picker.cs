using System;
using UnityEngine;

public class Picker : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _container;
    [SerializeField] private Mover _mover;

    private IPickUpItem _pickUpItem;

    public event Action Picked;

    public bool IsPicked { get; private set; }

    public void SetCurrentPickUpItem(IPickUpItem pickUpItem)
    {
        _pickUpItem = pickUpItem;
        IsPicked = false;
    }

    public void Pickup()
    {
        _mover.LookAtTargetItem(_pickUpItem.PointPickUp.Position);
        int indexLayer = _animator.GetLayerIndex(TypeAnimationLayer.Picker.ToString());
        _animator.SetLayerWeight(indexLayer, 1f);
        _animator.SetTrigger(CharacterAnimatorData.Params.IsPickUp);
    }

    //AnimationEvent
    private void OnCompleteAnimationPickup()
    {
        _pickUpItem.PickUp(_container);
        IsPicked = true;
        Picked?.Invoke();
    }
}
