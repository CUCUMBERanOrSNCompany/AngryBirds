using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    /// <summary>
    /// Number of points in the aiming line
    /// </summary>
    private const int _aimingLinePointsCount = 24;

    public static Trajectory Instance { get; private set; } = null; 

    private void Start()
    {
        Instance = this;
    }

    public void CalculateTrajectoryPoints(Vector3 ballPos , Vector3 forceApplied, float timeStep)
    {
        PathPoints.Instance.Clear();

        float currentTime = timeStep;

        Vector2 currentPosition;

        for(int i = 0; i < _aimingLinePointsCount; i++)
        {
            currentPosition.x = (ballPos.x + forceApplied.x * currentTime);

            currentPosition.y = (ballPos.y + forceApplied.y * currentTime)
                - (Physics2D.gravity.magnitude * currentTime * currentTime)
                / 2f;

            PathPoints.Instance.CreateCurrentPathPoint(currentPosition,
                    isAiming: true);

            currentTime += timeStep;
        }
    }
}
