using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for displaying paths.
/// </summary>
public class PathPoints : MonoBehaviour
{
    /// <summary>
    /// Template of object that build a shooting path
    /// </summary>
    [SerializeField] private GameObject[] PathTemplates;

    /// <summary>
    /// Template of object that build an aiming line
    /// </summary>
    [SerializeField] private GameObject[] AimingPathTemplates;

    /// <summary>
    /// Singleton
    /// </summary>
    public static PathPoints Instance { get; private set; }

    /// <summary>
    /// The list of points that we registered
    /// </summary>
    private List<GameObject> _lastPoints;

    /// <summary>
    /// Dictating the time interval that we wait between getting each new point
    /// </summary>
    public float TimeInterval;

    /// <summary>
    /// Index to the visual template
    /// </summary>
    private int _lastIndex = 0;

    /// <summary>
    /// Instantiating the class and setting up the singleton
    /// </summary>
    void Start()
    {
        Instance = this;
        _lastPoints = new List<GameObject>();
    }

    /// <summary>
    /// Registering a new point to the path.
    /// </summary>
    /// <param name="position">The position of the point</param>
    /// <param name="isAiming">Is the point part of an aiming path or regular?</param>
    public void CreateCurrentPathPoint(Vector3 position, bool isAiming = false)
    {
        GameObject point;

        if(isAiming)
        {
            point = Instantiate(AimingPathTemplates[_lastIndex % AimingPathTemplates.Length],
                position, Quaternion.identity, transform);
        }
        else
        {
            point = Instantiate(PathTemplates[_lastIndex % PathTemplates.Length],
                position, Quaternion.identity, transform);
        }
        
        point.SetActive(true);
        _lastPoints.Add(point);

        _lastIndex++;
    }

    /// <summary>
    /// Clearing the points list.
    /// </summary>
    public void Clear()
    {
        //todo: Pooling on the points instead of destroying them.
        //Consider using IDisposable
        _lastPoints.ForEach((obj) => Destroy(obj));
        _lastPoints.Clear();
        _lastIndex = 0;
    }
}
