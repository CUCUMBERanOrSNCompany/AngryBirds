using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// Base class for enemies
/// </summary>
public abstract class EnemiesBase : MonoBehaviour, IDisposable
{
    /// <summary>
    /// Implementing IDisposable
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    /// Reactive property that reports the current state of the pig
    /// </summary>
    public ReactiveProperty<bool> IsAlive { get; private set; }
        = new ReactiveProperty<bool>(true);
}
