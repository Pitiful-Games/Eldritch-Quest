using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Singleton that manages the game state.
/// </summary>
public class GameManager : Singleton<GameManager> {
    /// <summary>
    ///     The player prefab.
    /// </summary>
    [SerializeField] private Player playerPrefab;

    public delegate void OnLevelStart();

    public event OnLevelStart LevelStarted;

    public Player Player { get; private set; }

    private void Start() {
        Player = Instantiate(playerPrefab);
        Player.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Change scenes with a fade transition.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    /// <param name="sceneTransitionType">The type of scene transition when changing scenes.</param>
    /// <param name="entryName">The name of the scene transition trigger to enter from after the scene changes.</param>
    public void ChangeScene(string sceneName, SceneTransitionType sceneTransitionType = SceneTransitionType.Level,
        string entryName = null) {
        StartCoroutine(ChangeSceneRoutine(sceneName, sceneTransitionType, entryName));
    }

    /// <summary>
    ///     The routine that carries out the scene change sequence.
    /// </summary>
    /// <param name="sceneName">The name of the scene to change to.</param>
    /// <param name="sceneTransitionType">The type of scene transition when changing scenes.</param>
    /// <param name="entryName">The name of the scene transition trigger to load from.</param>
    /// <returns></returns>
    public IEnumerator ChangeSceneRoutine(string sceneName, SceneTransitionType sceneTransitionType, string entryName) {
        var fader = UIManager.Instance.GetUI<Fader>();
        yield return fader.FadeIn();
        SaveDataManager.Instance.SaveGame();
        yield return SceneManager.LoadSceneAsync(sceneName);
        SaveDataManager.Instance.LoadGame();
        Player.gameObject.SetActive(SceneData.IsGameplayScene(sceneName));
        
        LevelStarted?.Invoke();
        
        Debug.Log("FADING OUT");
        yield return fader.FadeOut();
        Debug.Log("FADED OUT");
    }

    /// <summary>
    ///     Load the player at a save spot.
    /// </summary>
    /// <param name="saveScene">The saved scene to load.</param>
    public void LoadSaveSpot(string saveScene, Vector2 savePosition) {
        ChangeScene(saveScene, SceneTransitionType.MainMenu);
        Player.transform.position = savePosition;
    }

    /// <summary>
    ///     Toggle whether the game is paused.
    /// </summary>
    public static void TogglePause() {
        if (Time.timeScale <= 0)
            ResumeGame();
        else
            PauseGame();
    }

    /// <summary>
    ///     Pause the game.
    /// </summary>
    public static void PauseGame() {
        Time.timeScale = 0;
    }

    /// <summary>
    ///     Resume the game.
    /// </summary>
    public static void ResumeGame() {
        Time.timeScale = 1;
    }

    /// <summary>
    ///     Quit the game.
    /// </summary>
    public static void QuitGame() {
        Application.Quit();
    }
}