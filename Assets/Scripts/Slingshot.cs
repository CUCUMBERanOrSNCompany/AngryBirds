using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] LineRenderers;
    public Transform[] StripPositions;
    public Transform Center;
    public Transform IdlePosition;
    public Vector3 CurrentPosition;
    public float MaxLengthOfSling;
    public float BottomBoundary;

    private bool _isMouseDown;
    public GameObject BirdPrefab;
    public float BirdPositionOffset;
    private Rigidbody2D _bird;
    private Collider2D _birdCollider;
    public float Force;

    private LineRenderer _aimingLineRenderer;  // A single LineRenderer for aiming line
    private List<Vector3> _aimingLinePoints;    // List to store aiming line points
    private const int AimingLinePointsCount = 50;

    [SerializeField] private PathPoints _pathPoints = null;

    void Start()
    {
        LineRenderers[0].positionCount = 2;
        LineRenderers[1].positionCount = 2;
        LineRenderers[0].SetPosition(0, StripPositions[0].position);
        LineRenderers[1].SetPosition(0, StripPositions[1].position);

        InitializeAimingLineRenderer();

        CreateBird();
    }

    void InitializeAimingLineRenderer()
    {
        GameObject aimingLineObject = new GameObject("AimingLine");
        _aimingLineRenderer = aimingLineObject.AddComponent<LineRenderer>();
        _aimingLineRenderer.positionCount = AimingLinePointsCount;
        _aimingLineRenderer.enabled = false;
        _aimingLinePoints = new List<Vector3>();
    }



    void CreateBird()
    {
        _bird = Instantiate(BirdPrefab).GetComponent<Rigidbody2D>();
        _birdCollider = _bird.GetComponent<Collider2D>();
        _birdCollider.enabled = false;

        _bird.isKinematic = true;

        ResetStrips();
    }

    void UpdateAimingLine()
    {
        if (_bird != null)
        {
            _aimingLinePoints = CalculateTrajectoryPoints(_bird.position, _bird.velocity, 0.1f, AimingLinePointsCount);
            _aimingLineRenderer.SetPositions(_aimingLinePoints.ToArray());

            if (_pathPoints != null)
            {
                // Create path points using the pathPoints reference
                foreach (Vector3 point in _aimingLinePoints)
                {
                    _pathPoints.CreateCurrentPathPoint(point);
                }
            }
        }
    }


    void Update()
    {
        if (_isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            CurrentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            CurrentPosition = Center.position + Vector3.ClampMagnitude(CurrentPosition - Center.position, MaxLengthOfSling);

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

    private void OnMouseDown()
    {
        _isMouseDown = true;
        _aimingLineRenderer.enabled = true;
    }

    private void OnMouseUp()
    {
        _isMouseDown = false;
        Shoot();
        UpdateAimingLine();
        CurrentPosition = IdlePosition.position;
        _aimingLineRenderer.enabled = false;
    }

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

    void ResetStrips()
    {
        CurrentPosition = IdlePosition.position;
        SetStrips(CurrentPosition);
    }

    void SetStrips(Vector3 position)
    {
        LineRenderers[0].SetPosition(1, position);
        LineRenderers[1].SetPosition(1, position);

        if (_bird)
        {
            Vector3 dir = position - Center.position;
            _bird.transform.position = position + dir.normalized * BirdPositionOffset;
            _bird.transform.right = -dir.normalized;
        }
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, BottomBoundary, 1000);
        return vector;
    }

    List<Vector3> CalculateTrajectoryPoints(Vector3 startPosition, Vector3 initialVelocity, float timeStep, int numPoints)
    {
        List<Vector3> points = new List<Vector3>();

        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = initialVelocity;

        _pathPoints.Clear();

        for (int i = 0; i < numPoints; i++)
        {
            currentPosition += currentVelocity * timeStep;
            currentVelocity += Physics.gravity * timeStep;

            points.Add(currentPosition);
        }

        return points;
    }

    

}
