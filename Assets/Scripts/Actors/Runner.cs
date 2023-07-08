using System.Collections;
using UnityEngine;

/// <summary>
///     Manages horizontal movement for the actor.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Facer))]
public class Runner : MonoBehaviour {
    public delegate void OnAutoRunFinish(Runner runner);

    /// <summary>
    ///     The speed at which the actor runs.
    /// </summary>
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private RandomAudioClipSelector footstepSelector;
    [SerializeField] private float footstepInterval;

    private Rigidbody2D body;
    private Facer facer;

    private float footstepTimer;
    
    public bool IsRunning { get; private set; }

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        facer = GetComponent<Facer>();
    }

    private void Update() {
        if (!IsRunning) return;

        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepInterval) {
            footstepTimer = 0;
            footstepSelector.PlayRandomClip(transform.position);
        }
    }   

    /// <summary>
    ///     Raised when the actor has finished running to a horizontal position.
    /// </summary>
    public event OnAutoRunFinish AutoRunFinished;

    /// <summary>
    ///     Perform horizontal movement.
    /// </summary>
    /// <param name="vector">The direction that the actor runs in.</param>
    public void Run(Vector2 vector) {
        IsRunning = true;
        body.velocity = vector * runSpeed;
        var scaleX = transform.localScale.x;
        if ((scaleX < 0 && vector.x > 0) || (scaleX > 0 && vector.x < 0)) facer.Flip();
    }

    /// <summary>
    ///     Stop running.
    /// </summary>
    public void StopRun() {
        IsRunning = false;
        body.velocity = Vector2.zero;
    }

    /// <summary>
    ///     Run to a target x position.
    /// </summary>
    /// <param name="targetX">The x position to run to.</param>
    public void RunTo(Vector3 target) {
        IEnumerator DoRunTo() {
            Run(target.normalized);

            yield return new WaitUntil(() => (transform.position - target).magnitude <= 0.1f);

            AutoRunFinished?.Invoke(this);
        }

        StartCoroutine(DoRunTo());
    }
}