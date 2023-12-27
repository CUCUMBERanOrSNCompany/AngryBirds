using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for ending the game
/// </summary>
public class ExitTheGame : MonoBehaviour
{
    /// <summary>
    /// Closing the game.
    /// </summary>
    public void ApplicationQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
