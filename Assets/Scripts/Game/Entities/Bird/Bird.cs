using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Defines the Bird entity
/// </summary>
[RequireComponent(typeof(SFXManager))]
public class Bird : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Indicates if the object is collided.
    /// </summary>
    public bool IsCollided;

    /// <summary>
    /// Reference to the SFX Manager component
    /// </summary>
    private SFXManager _sfxManager = null;

    /// <summary>
    /// Making references.
    /// </summary>
    private void Start()
    {
        _sfxManager = GetComponent<SFXManager>();
    }

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
        if (collision.gameObject.tag != "Ground") _sfxManager.PlaySFXNoOverride(SoundsEnum.Explode);

        IsCollided = true;

        Invoke("Dispose", 1);
    }

    /// <summary>
    /// Disposing the gameobject
    /// </summary>
    public void Dispose()
    {
        //gameObject.GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject);
    }
}
