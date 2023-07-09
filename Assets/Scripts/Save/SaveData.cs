using System;
using UnityEngine;

/// <summary>
///     Data structure containing all objects with persistent data.
/// </summary>
[Serializable]
public class SaveData {

    /// <summary>
    ///     The scene of the save spot that the player last saved at.
    /// </summary>
    public string saveScene;

    public Vector2 savePosition;

    public SaveData() {
        saveScene = "Level1";
        savePosition = new Vector2(-2, 1);
    }
}