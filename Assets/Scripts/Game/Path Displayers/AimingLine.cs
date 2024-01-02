using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Calculating the aiming line.
/// </summary>
public class AimingLine : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static AimingLine Instance { get; private set; }

    /// <summary>
    /// Number of points in the aiming line
    /// </summary>
    private const int _aimingLinePointsCount = 24;

    /// <summary>
    /// Setting up the Singleton
    /// </summary>
    private void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// Calculating the points forming the aiming line
    /// </summary>
    /// <param name="birdPosition">The bird position on the sling</param>
    /// <param name="velocity">The force applied to the sling</param>
    /// <param name="timeStep">The time interval between each two calculations</param>
    public void CalculateTrajectoryPoints(Vector3 birdPosition, Vector3 velocity, float timeStep)
    {
        PathPoints.Instance.Clear();

        float currentTime = timeStep;

        Vector2 currentPosition;

        for (int i = 0; i < _aimingLinePointsCount; i++)
        {
            // The horizontal position is calculated by the original
            // bird position plus the velocity multiply by the number of
            // time intervals we've already passed.
            currentPosition.x = (birdPosition.x + velocity.x * currentTime);

            // The vertical position is calculated in a similar fashion to the
            // horizontal position but we deduct the gravity multiply by
            // the number of time intervals we've already passed squared,
            // divided by 2.
            currentPosition.y = (birdPosition.y + velocity.y * currentTime)
                - (Physics2D.gravity.magnitude * currentTime * currentTime)
                / 2f;

            PathPoints.Instance.CreateCurrentPathPoint(currentPosition,
                    isAiming: true);

            currentTime += timeStep;
        }
    }
}
