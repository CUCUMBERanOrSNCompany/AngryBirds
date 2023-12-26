using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;

    public float maxLength;

    public float bottomBoundary;

    bool isMouseDown;

    public GameObject birdPrefab;

    public float birdPositionOffset;

    Rigidbody2D bird;

    Collider2D birdCollider;

    public float force;

    [SerializeField] private AimingLine _aimingLine = null;

    void Start()
    {
        _aimingLine = GetComponent<AimingLine>();

        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateBird();
    }

    void CreateBird()
    {
        bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        birdCollider = bird.GetComponent<Collider2D>();
        birdCollider.enabled = false;

        bird.isKinematic = true;

        ResetStrips();
    }

    void Update()
    {
        if (isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);

            SetStrips(currentPosition);

            if (birdCollider)
            {
                birdCollider.enabled = true;

                // Calculate trajectory points based on the current slingshot parameters
                var trajectoryPoints = CalculateTrajectoryPoints();

                // Update the aiming line during the aiming phase
                _aimingLine.AimingLineCreator(trajectoryPoints);

            }
        }
        else
        {
            _aimingLine.AimingLineCreator(new Vector3[0]);
            ResetStrips();
        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        Shoot();
        currentPosition = idlePosition.position;

        // Clear the AimingLine after shooting
        _aimingLine.AimingLineCreator(new Vector3[0]);
    }

    void Shoot()
    {
        bird.isKinematic = false;
        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        bird.velocity = birdForce;

        bird.GetComponent<Bird>().Release();

        bird = null;
        birdCollider = null;
        Invoke("CreateBird", 2);
    }

    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }

    List<Vector3> CalculateTrajectoryPoints()
    {
        List<Vector3> points = new List<Vector3>();

        // Simulate the trajectory based on the current slingshot parameters
        // You may need to adjust this based on your specific game mechanics

        // Sample points for demonstration; replace this with your trajectory calculation logic
        for (float t = 0; t <= 1f; t += 0.1f)
        {
            //Vector3 point = /* Your trajectory calculation here based on t */;
            //points.Add(point);
        }

        return points;
    }
}
