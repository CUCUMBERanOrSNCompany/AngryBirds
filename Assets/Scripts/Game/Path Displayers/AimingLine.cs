using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Calculating the aiming line.
/// </summary>
public class AimingLine : MonoBehaviour
{
    public static AimingLine Instance { get; private set; }

    /// <summary>
    /// Number of points in the aiming line
    /// </summary>
    private const int _aimingLinePointsCount = 24;

    /// <summary>
    /// Aiming line
    /// </summary>
    private LineRenderer _aimingLineRenderer;

    /// <summary>
    /// List to store aiming line points
    /// </summary>
    private List<Vector3> _aimingLinePoints;


    private void Start()
    {
        Instance = this;

        InitializeAimingLineRenderer();
    }

    /// <summary>
    /// Initializing the Aiming line
    /// </summary>
    void InitializeAimingLineRenderer()
    {
        GameObject aimingLineObject = new GameObject("AimingLine");
        _aimingLineRenderer = aimingLineObject.AddComponent<LineRenderer>();
        _aimingLineRenderer.positionCount = _aimingLinePointsCount;
        _aimingLineRenderer.enabled = false;
        _aimingLinePoints = new List<Vector3>();
    }

    public void ShowHideAimingLine(bool state)
    {
        _aimingLineRenderer.enabled = state;
    }

    /// <summary>
    /// Updating the aiming line
    /// </summary>
    public void UpdateAimingLine(Vector3 currentPosition, Transform center, Rigidbody2D bird)
    {
        if (bird != null)
        {
            float slingshotAngle = Mathf.Atan2(currentPosition.x - center.position.x,
                currentPosition.y - center.position.y);

            _aimingLinePoints = CalculateTrajectoryPoints(bird.position,
                bird.velocity, PathPoints.Instance.TimeInterval, slingshotAngle);

            // Clear the path points
            PathPoints.Instance.Clear();

            // Set the positions of the line renderer
            _aimingLineRenderer.positionCount = _aimingLinePoints.Count;

            // Set the positions based on the trajectory points in world space
            for (int i = 0; i < _aimingLinePoints.Count; i++)
            {
                _aimingLineRenderer.SetPosition(i, _aimingLinePoints[i]);

                // Create path points using the pathPoints reference
                PathPoints.Instance.CreateCurrentPathPoint(_aimingLinePoints[i],
                    isAiming: true);
            }

            // Ensure the LineRenderer is enabled
            _aimingLineRenderer.enabled = true;
        }
        else
        {
            // Disable the LineRenderer if there's no bird
            _aimingLineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Calculating the points forming the aiming line,
    /// assuming the aiming line is perfectly horizontal.
    /// </summary>
    /// <param name="startPosition">The start position of the bird</param>
    /// <param name="initialVelocity">The velocity we have from the
    /// current tension of the strip</param>
    /// <param name="timeStep">The time interval between each two points</param>
    /// <param name="slingshotAngle">The angle of the sling</param>
    /// <returns>A list of points forming the aiming line</returns>
    List<Vector3> CalculateTrajectoryPoints(Vector3 startPosition,
        Vector3 initialVelocity, float timeStep, float slingshotAngle)
    {
        List<Vector3> points = new List<Vector3>();

        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = initialVelocity;

        PathPoints.Instance.Clear();

        for (int i = 0; i < _aimingLinePointsCount; i++)
        {
            // Set the position of the current point.
            currentPosition += currentVelocity * timeStep;

            // Factoring into the current velocity
            // the earth-like physics for the next point.
            currentVelocity += Physics.gravity * timeStep;

            // Adding the current position to the output.
            points.Add(currentPosition);

            // Create path points using the pathPoints reference
            PathPoints.Instance.CreateCurrentPathPoint(currentPosition, isAiming: true);
        }

        return CalculateRotationOfTrajectoryPoints(points, startPosition, slingshotAngle);
    }

    /// <summary>
    /// Getting list of horizontal points, and calculate their rotation using
    /// Radians between every two points.
    /// </summary>
    /// <param name="points">List of points</param>
    /// <param name="startPosition">The position of the bird.</param>
    /// <param name="slingshotAngle">At what position the slingshot is pointing at?</param>
    /// <returns>The list of points with rotation applied</returns>
    private List<Vector3> CalculateRotationOfTrajectoryPoints(List<Vector3> points,
        Vector3 startPosition, float slingshotAngle)
    {
        // Calculate the average direction of the trajectory
        Vector3 averageDirection = Vector3.zero;

        for (int i = 1; i < points.Count; i++)
        {
            averageDirection += points[i] - points[i - 1];
        }

        averageDirection /= points.Count - 1;

        // Calculate the rotation angle based on the average direction
        float rotationAngle = Mathf.Atan2(averageDirection.y, averageDirection.x);

        // Rotate the trajectory points based on the rotation angle and the slingshot's angle
        for (int i = 0; i < points.Count; i++)
        {
            float angle = rotationAngle - slingshotAngle;
            points[i] = startPosition + new Vector3(Mathf.Cos(angle),
                Mathf.Sin(angle), 0) * (points[i] - startPosition).magnitude;
        }

        return points;
    }
}
