using UnityEngine;

public class PointPickUp : MonoBehaviour, IMoverTarget
{
    public Vector3 Position => transform.position;
}
