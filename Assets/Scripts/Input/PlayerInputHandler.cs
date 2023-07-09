using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Singleton that manages global inputs.
/// </summary>
public class PlayerInputHandler : MonoBehaviour {
    /// <summary>
    ///     The duration that an input may be buffered for.
    /// </summary>
    [SerializeField] private float inputBufferTime = 0.15f;

    public BufferedInputAction Slash;

    [NonSerialized] public InputAction Grab;
    [NonSerialized] public InputAction Flame;

    [NonSerialized] public InputAction Inventory;
    
    /// <summary>
    ///     The input action for moving.
    /// </summary>
    [NonSerialized] public InputAction Move;

    [NonSerialized] public InputAction ResetPosition;

    public PlayerInputActions InputActions { get; private set; }

    /// <summary>
    ///     Whether the input manager is enabled.
    /// </summary>
    public bool IsEnabled => InputActions.asset.enabled;

    private void Awake() {
        SetupInputActions();
    }

    private void OnEnable() {
        Enable();
    }

    private void OnDisable() {
        Disable();
    }

    /// <summary>
    ///     Initialize input actions.
    /// </summary>
    private void SetupInputActions() {
        InputActions = new PlayerInputActions();

        Slash = new BufferedInputAction(InputActions.Player.Slash, inputBufferTime);
        Grab = InputActions.Player.Grab;
        Flame = InputActions.Player.Flame;

        Inventory = InputActions.Player.Inventory;

        Move = InputActions.Player.Move;

        ResetPosition = InputActions.Player.ResetPosition;
    }

    /// <summary>
    ///     Disable all input.
    /// </summary>
    public void Disable() {
        InputActions.asset.Disable();
    }

    /// <summary>
    ///     Enable all input.
    /// </summary>
    public void Enable() {
        InputActions.asset.Enable();
    }
}