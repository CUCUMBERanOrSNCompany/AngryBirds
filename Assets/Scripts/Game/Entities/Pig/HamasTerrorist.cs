using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class HamasTerrorist : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Counting the number of times the pig collided
    /// </summary>
    protected int _collisionCounter = 0;

    /// <summary>
    /// Reactive property that reports the current state of the pig
    /// </summary>
    public ReactiveProperty<bool> IsAlive { get; private set; }
        = new ReactiveProperty<bool>(true);

    /// <summary>
    /// Triggers whenever a collision detected
    /// </summary>
    /// <param name="collision">object we collided with</param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hi");
        if (IsAlive.Value)
        {
            _collisionCounter += 1;

            // It is possible for the pig not to hit the ground,
            // and we still need to classify the hit
            if (_collisionCounter > 1) Dispose();
        }
    }

    /// <summary>
    /// Killing the object
    /// </summary>
    public virtual void Dispose()
    {
        IsAlive.Value = false;

        gameObject.SetActive(false);
    }
}
