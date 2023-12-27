using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigsStateList : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites = null;

    public Dictionary<HealthEnum, Sprite> PigsStates { get; private set; }
        = new Dictionary<HealthEnum, Sprite>();

    /// <summary>
    /// Singleton
    /// </summary>
    public static PigsStateList Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        if (_sprites.Count != 2)
        {
            throw new Exception($"_sprites expect EXACTLY two sprites. " +
                $"You provided: {_sprites.Count}");
        }

        PigsStates[HealthEnum.Healthy] = _sprites[0];

        PigsStates[HealthEnum.Injured] = _sprites[1];
    }
}
