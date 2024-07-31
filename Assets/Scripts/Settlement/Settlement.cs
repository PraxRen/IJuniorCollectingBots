using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Settlement : MonoBehaviour, ISettlement
{
    [SerializeField] private List<Employee> _employees;
    [SerializeField] private ScannerResourceItem _scannerPickUpItem;
    [SerializeField] private Transform _pointForDropResource;

    private Dictionary<ResourceItem, Employee> _occupiedItems =  new Dictionary<ResourceItem, Employee>();

    public Vector3 Position => _pointForDropResource.position;

    private void Awake()
    {
        foreach (var employee in _employees)
        {
            employee.Initialize(this);
        }
    }

    private void Update()
    {
        UpdateFreeEmployee();
    }

    private void UpdateFreeEmployee()
    {
        var items = _scannerPickUpItem.ResourceItems.OrderBy(item => (item.Position - transform.position).sqrMagnitude).ToArray();

        foreach (var item in items) 
        {
            if (_occupiedItems.ContainsKey(item))
            {
                continue;
            }

            foreach (Employee employee in _employees)
            {
                if (employee.HasRestState() == false)
                {
                    continue;
                }

                employee.SetTargetItem(item);
                _occupiedItems.Add(item, employee);
                break;
            }
        }
    }
}