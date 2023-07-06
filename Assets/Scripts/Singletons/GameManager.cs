using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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
        switch (sceneTransitionType) {
            case SceneTransitionType.Level when SceneData.IsGameplayScene(sceneName) && entryName != null:
                StartLevel(entryName);
                break;
            case SceneTransitionType.MainMenu when SceneData.IsGameplayScene(sceneName):
                StartLevel();
                break;
        }
        
        LevelStarted?.Invoke();
        
        yield return fader.FadeOut();
    }

    /// <summary>
    ///     Load the player at a save spot.
    /// </summary>
    /// <param name="saveScene">The saved scene to load.</param>
    public void LoadSaveSpot(string saveScene) {
        ChangeScene(saveScene, SceneTransitionType.MainMenu);
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

    /// <summary>
    ///     Start a gameplay level from a scene transition trigger.
    /// </summary>
    /// <param name="entryName">The name of the scene transition trigger to enter from.</param>
    private void StartLevel(string entryName) {
        var sceneTransitionTrigger = FindObjectsOfType<SceneTransitionTrigger>()
            .FirstOrDefault(trigger => trigger.name == entryName);
        if (!sceneTransitionTrigger) {
            Debug.LogError(
                $"No scene transition trigger found in scene {SceneManager.GetActiveScene().name} with entry name {entryName}.");
            return;
        }
        
        var triggerCollider = sceneTransitionTrigger.GetComponent<Collider2D>();
        var triggerTransform = sceneTransitionTrigger.transform;
        var triggerScale = triggerTransform.localScale;
        var triggerWidth = triggerCollider.bounds.size.x;
        var triggerPosition = triggerTransform.position;
        var targetX = triggerPosition.x + triggerWidth * triggerScale.x;
        triggerCollider.enabled = false;
        Player.transform.position = triggerPosition;
        var playerInputHandler = Player.GetComponent<PlayerInputHandler>();
        playerInputHandler.Disable();
        var playerScale = Player.transform.localScale;
        playerScale = new Vector3(Mathf.Sign(triggerScale.x) * playerScale.x, playerScale.y, playerScale.z);
        Player.transform.localScale = playerScale;
        var playerRunner = Player.GetComponent<Runner>();
        playerRunner.RunTo(targetX);

        void RunFinishedHandler(Runner runner) {
            runner.StopRun();
            playerInputHandler.Enable();
            triggerCollider.enabled = true;
            playerRunner.AutoRunFinished -= RunFinishedHandler;
        }
        
        playerRunner.AutoRunFinished += RunFinishedHandler;
    }

    /// <summary>
    ///     Start a gameplay level from a save spot.
    /// </summary>
    private void StartLevel() {
        var saveSpot = FindObjectOfType<SaveSpot>();
        if (!saveSpot) {
            Debug.LogError($"No save spot found in scene {SceneManager.GetActiveScene().name}.");
            return;
        }

        Player.transform.position = saveSpot.transform.position;
    }
}