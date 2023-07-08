using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Controller for a player.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Facer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Runner))]
[RequireComponent(typeof(FlameSpawner))]
public class Player : MonoBehaviour, ISpawnable {
    #region Exposed Values

    [SerializeField] private Grabber grabber;
    [SerializeField] private GameObject flower;
    [SerializeField] private GameObject blush;
    [SerializeField] private Transform flamePointUp;
    [SerializeField] private Transform flamePointDown;
    [SerializeField] private Transform flamePointHorizontal;
    
    #endregion

    /// <summary>
    ///     The input manager for this specific player instance.
    /// </summary>
    public PlayerInputHandler InputHandler { get; private set; }
    
    #region Components

    private Animator animator;
    private Rigidbody2D body;
    private Facer facer;
    private FlameSpawner flameSpawner;
    private Runner runner;

    private int moveParameter = Animator.StringToHash("Move");
    private int slashParameter = Animator.StringToHash("Slash");
    private int grabParameter = Animator.StringToHash("Grab");
    private int flameParameter = Animator.StringToHash("Flame");
    private int verticalParameter = Animator.StringToHash("Vertical");

    #endregion

    #region Tracked Values

    private Vector2 lastVector;
    private Vector2 inputVector;
    private static readonly int AddColor = Shader.PropertyToID("_AddColor");

    #endregion
    
    #region Unity Functions

    private void Awake() {
        GetComponents();
        AssignPlayer();
    }

    private void Update() {
        Move();
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
        InputHandler.Move.Enable();
    }
    
    private void DisableMovement() {
        StopMovement();
        InputHandler.Move.Disable();
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
    }

    public void CreateFlame() {
        var flame = flameSpawner.Spawn();
        var parent = flamePointHorizontal;
        switch (animator.GetFloat(verticalParameter)) {
            case > 0.5f:
                parent = flamePointUp;
                break;
            case < -0.5f:
                parent = flamePointDown;
                break;
        }

        var flameTransform = flame.transform;
        flameTransform.SetParent(parent);
        flameTransform.localScale = Vector3.one;
        flameTransform.localPosition = Vector3.zero;
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

    public void Move() {
        lastVector = inputVector;
        inputVector = InputHandler.Move.ReadValue<Vector2>();

        if (inputVector.x != 0) {
            animator.SetFloat(verticalParameter, 0);
        } else if (inputVector.y != 0) {
            animator.SetFloat(verticalParameter, inputVector.y);
        }

        var moving = inputVector.magnitude > 0.1f;
        animator.SetBool(moveParameter, moving);
        if (moving) {
            runner.Run(inputVector);
        } else {
            runner.StopRun();
        }
    }

    /// <summary>
    ///     Get all components on the player.
    /// </summary>
    private void GetComponents() {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        facer = GetComponent<Facer>();
        flameSpawner = GetComponent<FlameSpawner>();
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
    public void OnSpawn() { }

    /// <inheritdoc />
    public void OnDespawn() { }

    /// <inheritdoc />
    public void OnDelete() { }

    public void AddQuestAddition(int questId) {
        switch (questId) {
            // Cat
            case 0:
                transform.Find("Sprite").GetComponent<SpriteRenderer>().material
                    .SetColor(AddColor, new Color(0.25f, 0.05f, 0.25f, 1));
                break;
            // Hug
            case 1:
                flower.SetActive(true);
                break;
            case 2:
                blush.SetActive(true);
                break;
        }
    }
    
    /// <summary>
    ///     Assign the player as the camera controller's current target.
    /// </summary>
    private void AssignPlayer() {
        FindObjectOfType<CameraController>(true).Target = transform;
        DontDestroyOnLoad(this);
    }
}