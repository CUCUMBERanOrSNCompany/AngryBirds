using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimingLine : MonoBehaviour
{
    public float Width = 0.2f;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // Initialize with no points
    }

    public void AimingLineCreator(IEnumerable<Vector3> points)
    {
        Vector3[] pointsArray = points.ToArray();

        lineRenderer.positionCount = pointsArray.Length * 2;

        for (int i = 0; i < pointsArray.Length; i++)
        {
            Vector3 currentPoint = pointsArray[i];
            Vector3 forwardDirection = (i < pointsArray.Length - 1) ? (pointsArray[i + 1] - currentPoint).normalized : Vector3.zero;
            Vector3 rightDirection = Vector3.Cross(Vector3.up, forwardDirection).normalized;

            Vector3 rightVertex = currentPoint + rightDirection * (Width * 0.5f);
            Vector3 leftVertex = currentPoint - rightDirection * (Width * 0.5f);

            lineRenderer.SetPosition(i * 2, rightVertex);
            lineRenderer.SetPosition(i * 2 + 1, leftVertex);
        }
    }
}
