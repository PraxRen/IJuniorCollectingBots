using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Settlement : MonoBehaviour, ISettlement
{
    [SerializeField] private Employee _prefab;
    [SerializeField] private Transform _pointForSpawnNewEmployee;
    [SerializeField] private List<Employee> _employees;
    [SerializeField] private ScannerResourceItem _scannerPickUpItem;
    [SerializeField] private Transform _pointForDropResource;
    [SerializeField] private StorageSettlementResources _storageSettlementResources;
    [SerializeField] private int _countResourcesForCreateEmployee;
    [SerializeField] private int _countResourcesForExtensions;
    [SerializeField] private Flag _flag;

    private Dictionary<ResourceItem, Employee> _occupiedItems =  new Dictionary<ResourceItem, Employee>();
    private bool _isReservedEmployeeForFlag;

    public Vector3 Position => _pointForDropResource.position;
    public Flag Flag => _flag;

    private void Awake()
    {
        foreach (Employee employee in _employees)
        {
            employee.Initialize(this);
        }
    }

    private void Update()
    {
        HandleFlag();
        HandleScanner();
        Handle—olonization();
    }

    public void AddEmployee(Employee employee)
    {
        employee.Initialize(this);
        _employees.Add(employee);
    }

    public void RemoveEmployee(Employee employee)
    {
        if (_employees.Contains(employee) == false)
            return;

        _employees.Remove(employee);
    }

    private void HandleFlag()
    {
        if (_isReservedEmployeeForFlag)
        {
            return;
        }

        if (Flag.State != StateFlag.Assigned)
        {
            return;
        }

        if (_storageSettlementResources.Count < _countResourcesForExtensions)
        {
            return;
        }

        foreach (Employee employee in _employees)
        {
            if (employee.HasRestState() == false)
            {
                continue;
            }

            employee.SetFlag(Flag);
            Flag.ChangedState += OnChangedStateFlag;
            _storageSettlementResources.RemoveResources(_countResourcesForExtensions);
            _isReservedEmployeeForFlag = true;
            break;
        }
    }

    private void OnChangedStateFlag(StateFlag stateFlag)
    {
        if (stateFlag == StateFlag.Free)
        {
            Flag.ChangedState -= OnChangedStateFlag;
            _isReservedEmployeeForFlag = false;
        }
    }

    private void HandleScanner()
    {
        var items = _scannerPickUpItem.ResourceItems.OrderBy(item => (item.Position - transform.position).sqrMagnitude).ToArray();

        foreach (ResourceItem item in items) 
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

    private void Handle—olonization()
    {
        if (Flag.State == StateFlag.Assigned && _isReservedEmployeeForFlag == false)
        {
            return;
        }

        if (_storageSettlementResources.Count < _countResourcesForCreateEmployee)
        {
            return;
        }

        _storageSettlementResources.RemoveResources(_countResourcesForCreateEmployee);
        CreateEmployee();
    }

    private void CreateEmployee()
    {
        Employee newEmployee = Instantiate(_prefab, _pointForSpawnNewEmployee.position, _prefab.transform.rotation);
        AddEmployee(newEmployee);
    }
}