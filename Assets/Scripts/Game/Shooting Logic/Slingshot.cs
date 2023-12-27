using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for the slingshot logic.
/// </summary>
public class Slingshot : MonoBehaviour
{
    #region fields

    /// <summary>
    /// Array of line renderers
    /// </summary>
    public LineRenderer[] LineRenderers;

    /// <summary>
    /// The Sling's strips positions
    /// </summary>
    public Transform[] StripPositions;

    /// <summary>
    /// The center position of the sling
    /// </summary>
    public Transform Center;

    /// <summary>
    /// Idle position of the strips
    /// </summary>
    public Transform IdlePosition;

    /// <summary>
    /// The current position of the strips
    /// </summary>
    public Vector3 CurrentPosition;

    /// <summary>
    /// The maximum length of the sling's strips
    /// </summary>
    public float MaxLengthOfSling;

    /// <summary>
    /// Helping us in ensuring the strips aren't going through the ground
    /// </summary>
    public float BottomBoundary;

    /// <summary>
    /// Indicating if the mouse is clicked
    /// </summary>
    private bool _isMouseDown;

    /// <summary>
    /// Prefab of the bird
    /// </summary>
    public GameObject BirdPrefab;

    /// <summary>
    /// The offset of the bird to the sling.
    /// </summary>
    public float BirdPositionOffset;

    /// <summary>
    /// Rigidbody component of the bird.
    /// </summary>
    private Rigidbody2D _bird;

    /// <summary>
    /// Collider component of the bird
    /// </summary>
    private Collider2D _birdCollider;

    /// <summary>
    /// Current force applied.
    /// </summary>
    public float Force;

    /// <summary>
    /// Aiming line
    /// </summary>
    private LineRenderer _aimingLineRenderer;

    /// <summary>
    /// List to store aiming line points
    /// </summary>
    private List<Vector3> _aimingLinePoints;

    /// <summary>
    /// Number of points in the aiming line
    /// </summary>
    private const int AimingLinePointsCount = 24;

    /// <summary>
    /// Reference to the PathPoints class
    /// </summary>
    //[SerializeField] private PathPoints _pathPoints = null;
    #endregion

    #region Initializers
    /// <summary>
    /// Initialization Tasks.
    /// </summary>
    void Start()
    {
        InitializeStripLinesRenderers();

        InitializeAimingLineRenderer();

        CreateBird();
    }

    /// <summary>
    /// Initializing the sling's strip lines
    /// </summary>
    private void InitializeStripLinesRenderers()
    {
        LineRenderers[0].positionCount = 2;
        LineRenderers[1].positionCount = 2;
        LineRenderers[0].SetPosition(0, StripPositions[0].position);
        LineRenderers[1].SetPosition(0, StripPositions[1].position);
    }

    /// <summary>
    /// Initializing the Aiming line
    /// </summary>
    void InitializeAimingLineRenderer()
    {
        GameObject aimingLineObject = new GameObject("AimingLine");
        _aimingLineRenderer = aimingLineObject.AddComponent<LineRenderer>();
        _aimingLineRenderer.positionCount = AimingLinePointsCount;
        _aimingLineRenderer.enabled = false;
        _aimingLinePoints = new List<Vector3>();
    }

    /// <summary>
    /// Creating the bird object
    /// </summary>
    void CreateBird()
    {
        _bird = Instantiate(BirdPrefab).GetComponent<Rigidbody2D>();
        _birdCollider = _bird.GetComponent<Collider2D>();
        _birdCollider.enabled = false;

        _bird.isKinematic = true;

        ResetStrips();
    }
    #endregion

    /// <summary>
    /// The monobehavior logic.
    /// </summary>
    void Update()
    {
        if (_isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            CurrentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            CurrentPosition = Center.position
                + Vector3.ClampMagnitude(CurrentPosition - Center.position,
                MaxLengthOfSling);

            CurrentPosition = ClampBoundary(CurrentPosition);

            SetStrips(CurrentPosition);

            if (_birdCollider)
            {
                _birdCollider.enabled = true;
                UpdateAimingLine();
            }
        }
        else
        {
            ResetStrips();
            _aimingLineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Defining behavior when the mouse is clicked
    /// </summary>
    private void OnMouseDown()
    {
        _isMouseDown = true;
        _aimingLineRenderer.enabled = true;
    }

    /// <summary>
    /// Defining behavior when the mouse is unclicked
    /// </summary>
    private void OnMouseUp()
    {
        _isMouseDown = false;
        Shoot();
        UpdateAimingLine();
        CurrentPosition = IdlePosition.position;
        _aimingLineRenderer.enabled = false;
    }

    /// <summary>
    /// The shooting logic
    /// </summary>
    void Shoot()
    {
        _bird.isKinematic = false;
        Vector3 birdForce = (CurrentPosition - Center.position) * Force * -1;
        _bird.velocity = birdForce;

        _bird.GetComponent<Bird>().Release();

        _bird = null;
        _birdCollider = null;
        Invoke("CreateBird", 2);
    }

    /// <summary>
    /// Resetting the sling's strips position to idle.
    /// </summary>
    void ResetStrips()
    {
        CurrentPosition = IdlePosition.position;
        SetStrips(CurrentPosition);
    }

    /// <summary>
    /// Changing the sling's strip position
    /// </summary>
    /// <param name="position">The new position of the strips</param>
    void SetStrips(Vector3 position)
    {
        LineRenderers[0].SetPosition(1, position);
        LineRenderers[1].SetPosition(1, position);

        if (_bird)
        {
            Vector3 dir = position - Center.position;
            _bird.transform.position = position + dir.normalized
                * BirdPositionOffset;

            // Calculate the angle of the slingshot
            float slingshotAngle = Mathf.Atan2(dir.y, dir.x);

            // Set the launch direction for the bird based on the slingshot angle
            _bird.transform.right = Quaternion.Euler(0, 0,
                Mathf.Rad2Deg * slingshotAngle) * Vector3.right;
        }
    }

    /// <summary>
    /// The boundary of the clamp
    /// </summary>
    /// <param name="vector">Unprocessed boundary vector</param>
    /// <returns>The input vector with y element clamped</returns>
    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, BottomBoundary, 1000);
        return vector;
    }

    /// <summary>
    /// Updating the aiming line
    /// </summary>
    void UpdateAimingLine()
    {
        if (_bird != null)
        {
            float slingshotAngle = Mathf.Atan2(CurrentPosition.x - Center.position.x,
                CurrentPosition.y - Center.position.y);

            _aimingLinePoints = CalculateTrajectoryPoints(_bird.position,
                _bird.velocity, PathPoints.Instance.TimeInterval, slingshotAngle);

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
                    isAiming:true);
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
    /// Calculating the points forming the aiming line
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

        for (int i = 0; i < AimingLinePointsCount; i++)
        {
            currentPosition += currentVelocity * timeStep;
            currentVelocity += Physics.gravity * timeStep;

            points.Add(currentPosition);

            // Create path points using the pathPoints reference
            PathPoints.Instance.CreateCurrentPathPoint(currentPosition, isAiming:true);
        }

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
