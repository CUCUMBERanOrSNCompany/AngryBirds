using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Defines the Bird entity
/// </summary>
public class Bird : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Indicates if the object is collided.
    /// </summary>
    public bool IsCollided;

    /// <summary>
    /// Observing the actual path of the object.
    /// </summary>
    public void Release()
    {
        PathPoints.Instance.Clear();
        StartCoroutine(CreatePathPoints());
    }

    /// <summary>
    /// Adding a point to the path every time interval.
    /// </summary>
    private IEnumerator CreatePathPoints()
    {
        while (true)
        {
            if (IsCollided) break;
            PathPoints.Instance.CreateCurrentPathPoint(transform.position);
            yield return new WaitForSeconds(PathPoints.Instance.TimeInterval);
        }
    }

    /// <summary>
    /// Reacting to a collision
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsCollided = true;

        if (collision.gameObject.tag == "Ground") Dispose();
    }

    /// <summary>
    /// Disposing the gameobject
    /// </summary>
    public void Dispose()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
