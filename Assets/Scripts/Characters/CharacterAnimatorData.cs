using UnityEngine;

public class CharacterAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int IsFarm = Animator.StringToHash("Farm");
        public static readonly int IsPickUp = Animator.StringToHash("PickUp");
    }
}