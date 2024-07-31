using System;
using System.Linq;
using UnityEngine;

public class ActivatorTool : MonoBehaviour
{
    [SerializeField] LocationTool[] _locations;

    private LocationTool _currentLocationTool;

    public void Activate(ResourceItem item)
    {
        LocationTool location = _locations.FirstOrDefault(location => location.Type == item.TypeTool);

        if (location == null) 
        {
            throw new ArgumentNullException(nameof(location));
        }

        _currentLocationTool = location;
        _currentLocationTool.Tool.SetTarget(item);
        _currentLocationTool.gameObject.SetActive(true);
    }

    public void Deactivate(ResourceItem item) 
    {
        LocationTool location = _locations.FirstOrDefault(location => location.Type == item.TypeTool);

        if (location == null)
        {
            throw new ArgumentNullException(nameof(location));
        }

        _currentLocationTool.Tool.DisableCollider();
        _currentLocationTool.Tool.ClearTarget();
        _currentLocationTool.gameObject.SetActive(false);
    }

    private void OnStartAnimation()
    {
        _currentLocationTool.Tool.EnableCollider();
    }

    private void OnStopAnimation()
    {
        _currentLocationTool.Tool.DisableCollider();
    }
}