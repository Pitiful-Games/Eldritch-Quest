using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Controller for a player.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Facer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Runner))]
public class Player : MonoBehaviour, ISpawnable {
    #region Exposed Values

    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private Grabber grabber;
    
    #endregion

    /// <summary>
    ///     The input manager for this specific player instance.
    /// </summary>
    public PlayerInputHandler InputHandler { get; private set; }
    
    #region Components

    private Animator animator;
    private Rigidbody2D body;
    private Facer facer;
    private Runner runner;

    private int moveParameter = Animator.StringToHash("Move");
    private int slashParameter = Animator.StringToHash("Slash");
    private int grabParameter = Animator.StringToHash("Grab");
    private int flameParameter = Animator.StringToHash("Flame");
    private int verticalParameter = Animator.StringToHash("Vertical");

    #endregion

    #region Tracked Values

    private float coyoteTimer;
    private Vector2 inputVector;

    #endregion
    
    #region Unity Functions

    private void Awake() {
        GetComponents();
        AssignPlayer();
    }

    private void Update() {
        facer.CheckFlip();
    }

    private void OnEnable() {
        EnableAllInputs();
    }

    private void OnDisable() {
        DisableAllInputs();
    }

    #endregion
    
    /// <summary>
    ///     Enable all player inputs.
    /// </summary>
    public void EnableAllInputs() {
        EnableBaseInputs();
    }

    private void EnableMovement() {
        InputHandler.Move.performed += OnMoveStart;
        InputHandler.Move.canceled += OnMoveStop;
    }
    
    private void DisableMovement() {
        StopMovement();
        InputHandler.Move.performed -= OnMoveStart;
        InputHandler.Move.canceled -= OnMoveStop;
    }
    
    /// <summary>
    ///     Enable only the player's base inputs.
    /// </summary>
    private void EnableBaseInputs() {
        EnableMovement();
        InputHandler.Slash.InputAction.performed += OnSlash;
        InputHandler.Grab.performed += OnGrab;
        InputHandler.Flame.performed += OnFlame;
        InputHandler.Inventory.performed += OnInventoryOpen;
    }

    private void OnInventoryOpen(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton()) {
            UIManager.Instance.OpenUI<Inventory>();
        }
    }

    private void OnFlame(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !animator.GetBool(slashParameter) && !animator.GetBool(grabParameter) && !animator.GetBool(flameParameter)) {
            Flame();
        }
    }

    private void OnGrab(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !animator.GetBool(slashParameter) && !animator.GetBool(grabParameter) && !animator.GetBool(flameParameter)) {
            Grab();
        }
    }

    private void OnSlash(InputAction.CallbackContext context) {
        if (context.ReadValueAsButton() && !animator.GetBool(slashParameter) && !animator.GetBool(grabParameter) && !animator.GetBool(flameParameter)) {
            Slash();
        }
    }

    public void ResetActions() {
        animator.SetBool(slashParameter, false);
        animator.SetBool(grabParameter, false);
        animator.SetBool(flameParameter, false);
        grabber.Release();
        grabber.gameObject.SetActive(false);
        flameParticles.Stop();
        EnableMovement();
    }

    private void Slash() {
        DisableMovement();
        animator.SetBool(slashParameter, true);
    }

    private void Grab() {
        DisableMovement();
        animator.SetBool(grabParameter, true);
    }
    
    private void Flame() {
        DisableMovement();
        animator.SetBool(flameParameter, true);
        flameParticles.Play();
    }

    /// <summary>
    ///     Disable all player inputs.
    /// </summary>
    public void DisableAllInputs() {
        DisableBaseInputs();
    }

    /// <summary>
    ///     Disable only the player's base inputs.
    /// </summary>
    private void DisableBaseInputs() {
        DisableMovement();
        InputHandler.Slash.InputAction.performed -= OnSlash;
        InputHandler.Grab.performed -= OnGrab;
        InputHandler.Flame.performed -= OnFlame;
        InputHandler.Inventory.performed -= OnInventoryOpen;
    }

    /// <summary>
    ///     Get all components on the player.
    /// </summary>
    private void GetComponents() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        facer = GetComponent<Facer>();
        runner = GetComponent<Runner>();
        InputHandler = GetComponent<PlayerInputHandler>();
    }

    ///     Stop all movement of the player.
    /// <summary>
    /// </summary>
    public void StopMovement() {
        animator.SetBool(moveParameter, false);
        runner.StopRun();
    }

    /// <inheritdoc />
    public void OnCreate() { }

    /// <inheritdoc />
    public void OnSpawn() {
        
    }

    /// <inheritdoc />
    public void OnDespawn() { }

    /// <inheritdoc />
    public void OnDelete() { }

    /// <summary>
    ///     Callback for when the player starts moving.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnMoveStart(InputAction.CallbackContext context) {
        inputVector = context.ReadValue<Vector2>();
        animator.SetFloat(verticalParameter, inputVector.y);
        animator.SetBool(moveParameter, inputVector.magnitude > 0.1f);
        runner.Run(inputVector);
    }

    /// <summary>
    ///     Callback for when the player stops moving.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnMoveStop(InputAction.CallbackContext context) {
        animator.SetBool(moveParameter, false);
        runner.StopRun();
    }

    /// <summary>
    ///     Assign the player as the camera controller's current target.
    /// </summary>
    private void AssignPlayer() {
        FindObjectOfType<CameraController>(true).Target = transform;
        DontDestroyOnLoad(this);
    }
}