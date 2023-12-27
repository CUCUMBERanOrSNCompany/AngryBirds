using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managing pools for the paths points
/// </summary>
public class PointsPool : MonoBehaviour
{
    /// <summary>
    /// Template of object that build a shooting path
    /// </summary>
    [SerializeField] private GameObject[] _pathTemplates;

    /// <summary>
    /// Template of object that build an aiming line
    /// </summary>
    [SerializeField] private GameObject[] _aimingTemplates;

    /// <summary>
    /// Pool for the aiming points
    /// </summary>
    private List<GameObject> _aimingPointsPool = new List<GameObject>();

    /// <summary>
    /// Current index of the aiming points
    /// </summary>
    private int _aimingPointsIndex = 0;

    /// <summary>
    /// Pool for the path points
    /// </summary>
    private List<GameObject> _pathPointsPool = new List<GameObject>();

    /// <summary>
    /// Current index of the path points
    /// </summary>
    private int _pathPointsIndex = 0;

    /// <summary>
    /// Clearing the path
    /// </summary>
    public void ClearPathPoints()
    {
        if (_pathPointsPool.Count == 0) return;

        for(int i = _pathPointsPool.Count - 1; i >= 0; i--)
        {
            _pathPointsPool[i].SetActive(false);
        }

        _pathPointsIndex = 0;
    }

    /// <summary>
    /// Clearing the path
    /// </summary>
    public void ClearAimingPoints()
    {
        if (_aimingPointsPool.Count == 0) return;
        
        for (int i = _aimingPointsPool.Count - 1; i >= 0; i--)
        {
            _aimingPointsPool[i].SetActive(false);
        }

        _aimingPointsIndex = 0;
    }

    /// <summary>
    /// Adding a point to the screen from the pool
    /// </summary>
    /// <param name="position">The desired position of the point</param>
    /// <returns>Reference to the point</returns>
    public GameObject AddPathPoint(Vector3 position)
    {
        if(_pathPointsPool.Count > _pathPointsIndex)
        {
            _pathPointsPool[_pathPointsIndex].transform.position = position;
            _pathPointsPool[_pathPointsIndex].SetActive(true);
            _pathPointsIndex += 1;
            return _pathPointsPool[_pathPointsIndex - 1];
        }

        else
        {
            GameObject point;

            point = Instantiate(_pathTemplates[_pathPointsIndex % _pathTemplates.Length],
                position, Quaternion.identity, transform);

            point.SetActive(true);

            _pathPointsPool.Add(point);

            _pathPointsIndex += 1;

            return _pathPointsPool[_pathPointsIndex - 1];
        }
    }

    /// <summary>
    /// Adding a point to the screen from the pool
    /// </summary>
    /// <param name="position">The desired position of the point</param>
    /// <returns>Reference to the point</returns>
    public GameObject AddAimingPoint(Vector3 position)
    {
        if (_aimingPointsPool.Count > _aimingPointsIndex)
        {
            _aimingPointsPool[_aimingPointsIndex].transform.position = position;
            _aimingPointsPool[_aimingPointsIndex].SetActive(true);
            _aimingPointsIndex += 1;
            return _aimingPointsPool[_aimingPointsIndex - 1];
        }

        else
        {
            GameObject point;

            point = Instantiate(_aimingTemplates[_aimingPointsIndex % _aimingTemplates.Length],
                position, Quaternion.identity, transform);

            point.SetActive(true);

            _aimingPointsPool.Add(point);

            _aimingPointsIndex += 1;

            return _aimingPointsPool[_aimingPointsIndex - 1];
        }
    }


}
