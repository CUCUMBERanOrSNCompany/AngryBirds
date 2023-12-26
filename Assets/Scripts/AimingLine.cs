using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Responsible for displaying the aiming line.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AimingLine : MonoBehaviour
{
    /// <summary>
    /// Width dictating the spacing between the meshes.
    /// </summary>
    public float Width = 0.2f;

    /// <summary>
    /// The mesh object
    /// </summary>
    private Mesh _mesh;

    /// <summary>
    /// Initializing the mesh
    /// </summary>
    private void Start()
    {
        _mesh = new Mesh();
		
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    /// <summary>
    /// Setting up a new aiming line 
    /// </summary>
    /// <param name="point"></param>
    public void AimingLineCreator(IEnumerable<Vector3> point)
    {
        var pointsArray = point as Vector3[] 
                        ?? point.ToArray();
            
        _mesh.Clear();

        var vertices = new Vector3[pointsArray.Length * 2];

        var uvs = new Vector2[pointsArray.Length * 2];

        int[] triangles;

        if (pointsArray.Length > 1)
        {
            triangles = new int[(pointsArray.Length - 1) * 2 * 3 * 2];
        }
        else
        {
            return;
        }

   
        for (var i = 0; i < pointsArray.Length; i++)
        {
            var currentPoint = pointsArray[i];
                
            var previousPoint = i > 0 
                ? pointsArray[i - 1] 
                : currentPoint;
                
            var nextPoint = i < pointsArray.Length - 1 
                ? pointsArray[i + 1] 
                : currentPoint;

            var forwarightDirectionDirection = (nextPoint - previousPoint).normalized;

            var rightDirection = (Vector3.Cross(Vector3.up, forwarightDirectionDirection)).normalized;

            var rightVertex = currentPoint + rightDirection * (Width * 0.5f);

            var leftVertex = currentPoint - rightDirection * (Width * 0.5f);

            vertices[i * 2] = rightVertex;

            vertices[i * 2 + 1] = leftVertex;

            uvs[i * 2] = new Vector2(1f, (float)i / pointsArray.Length);

            uvs[i * 2 + 1] = new Vector2(0f, (float)i / pointsArray.Length);

            if (i >= pointsArray.Length - 1) break;

            triangles = TrianglesConstructor(i, triangles);
        }

        MeshSetter(vertices, uvs, triangles);
    }

    /// <summary>
    /// Constructs a triangles array used by the Mesh
    /// </summary>
    /// <param name="i">Index provided</param>
    /// <param name="triangles">A triangles array</param>
    /// <returns>Modified triangles array</returns>
    private int[] TrianglesConstructor(int i, int[] triangles)
    {
        Debug.Log($"{i}");
        int baseIndex = i * 6;

        if (baseIndex + 5 < triangles.Length)
        {
            triangles[baseIndex] = i * 2;
            triangles[baseIndex + 1] = i * 2 + 1;
            triangles[baseIndex + 2] = i * 2 + 2;
            triangles[baseIndex + 3] = i * 2 + 2;
            triangles[baseIndex + 4] = i * 2 + 1;
            triangles[baseIndex + 5] = i * 2 + 3;
        }
        else
        {
            Debug.LogError("Index out of bounds in TrianglesConstructor");
        }

        return triangles;
    }

    /// <summary>
    /// Setting up and defining _mesh.
    /// </summary>
    /// <param name="vertices">The vertices array defining the mesh</param>
    /// <param name="uvs">The uvs array defining the mesh</param>
    /// <param name="triangles">The triangles array defining the mesh</param>
    private void MeshSetter(Vector3[] vertices, Vector2[] uvs, int[] triangles)
    {
        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
    }

}
