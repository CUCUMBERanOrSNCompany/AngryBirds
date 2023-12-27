using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Managing the scenes of the game
/// </summary>
public class SceneMover : MonoBehaviour
{
    /// <summary>
    /// Resetting the current scene
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Transferring to next scene
    /// </summary>
    public void NextScene()
    {

    }
}
