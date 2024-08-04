using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private float _maxNavMeshProjectionDistance;
    [SerializeField] private LayerMask _layerMaskSettlement;
    [SerializeField] private LayerMask _layerMaskGround;

    private Camera _cameraMain;
    private Flag _currentFlag;

    private void Awake()
    {
        _cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.ActivetedCursor += OnActivetedCursor;
    }

    private void OnDisable()
    {
        _inputReader.ActivetedCursor -= OnActivetedCursor;
    }

    private void OnActivetedCursor()
    {
        if (_currentFlag == null)
        {
            FindFlag();

            if (_currentFlag?.State == StateFlag.Selected)
            {
                StartCoroutine(UpdateFlag());
            }
        }
        else
        {
            if (_currentFlag.State != StateFlag.Selected)
                return;

            if (_currentFlag.CanBuilding == false)
                return;

            _currentFlag.Assign();
        }
    }

    private IEnumerator UpdateFlag()
    {
        while (_currentFlag.State == StateFlag.Selected) 
        {
            SetFlagPosition();

            yield return null;
        }

        _currentFlag = null;
    }

    private void FindFlag()
    {
        if (TryGetRaycastHit(_layerMaskSettlement, out RaycastHit hit) == false)
            return;

        if (hit.transform.TryGetComponent(out Settlement settlement) == false)
            return;

        Flag flag = settlement.Flag;

        if (flag.State == StateFlag.Free || flag.State == StateFlag.Assigned)
        {
            _currentFlag = flag;
            _currentFlag.Select();
        }
    }

    private void SetFlagPosition()
    {
        if (TryGetRaycastHit(_layerMaskGround, out RaycastHit hit) == false)
            return;

        if (TryGetPositionNavMesh(hit, out Vector3 position) == false)
            return;

        _currentFlag.transform.position = position;
    }

    private bool TryGetRaycastHit(LayerMask layerMask, out RaycastHit hit)
    {
        Ray mouseRay = _cameraMain.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(mouseRay, out hit, Mathf.Infinity, layerMask);
    }

    private bool TryGetPositionNavMesh(RaycastHit hit, out Vector3 position)
    {
        position = Vector3.zero;
        bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, _maxNavMeshProjectionDistance, NavMesh.AllAreas);

        if (!hasCastToNavMesh)
            return false;

        position = navMeshHit.position;
        return hasCastToNavMesh;
    }
}