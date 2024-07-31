using System;
using UnityEngine;

public interface IPickUpItem : IMoverTarget
{
    public TypeTool TypeTool { get; }
    public IMoverTarget PointPickUp { get; }

    void PickUp(Transform transform);
} 
