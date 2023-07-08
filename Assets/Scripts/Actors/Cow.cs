using PathCreation;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Facer))]
[RequireComponent(typeof(PathFollower))]
public class Cow : MonoBehaviour {
    [SerializeField] private DetectRange detectRange;
    [SerializeField] private PathCreator initialPath;
    [SerializeField] private PathCreator remainingPath;

    [SerializeField] private LayerMask mask;

    private Animator animator;
    private Facer facer;
    private PathFollower pathFollower;
    private Vector3 lastPos;
    private Collider2D col;
    [SerializeField] private cowPickup pickup;

    private static int spookedParameter = Animator.StringToHash("Spooked");

    private bool isSpooked;
    private int lastDir;

    private void Awake() {
        animator = GetComponent<Animator>();
        facer = GetComponent<Facer>();
        pathFollower = GetComponent<PathFollower>();
        col = GetComponent<Collider2D>();
        detectRange.Detected += OnDetect;
        pathFollower.PathEnded += OnPathEnd;
    }

    private void Update() {
        var direction = (transform.position - lastPos).normalized;
        facer.FaceDirection(direction.x);
        Debug.DrawRay(transform.position, direction);
        if (Physics2D.Raycast(transform.position, direction, 0.7f, mask) && pathFollower.pathCreator == remainingPath)
        {
            pathFollower.travelSpeed = 0;
            lastDir *= -1;
        }
        lastPos = transform.position;
    }

    private void Start()
    {
        pathFollower.pathCreator = initialPath;
        isSpooked = true;
        lastDir = -1;
        pickup.gameObject.SetActive(false);
    }

    private void OnPathEnd(PathFollower follower) {
        animator.SetBool(spookedParameter, false);
        pathFollower.travelSpeed = 0;
        if (pathFollower.pathCreator != remainingPath)
        {
            pathFollower.ChangePath(remainingPath);
        }
        else{
            transform.position = lastPos;
            detectRange.Detected -= OnDetect;
            col.enabled = false;
            pickup.gameObject.SetActive(true);
        }
    }

    private void OnDetect(Collider2D other) {
        if (pathFollower.travelSpeed == 0)
        {
            if (isSpooked) {
                animator.SetBool(spookedParameter, true);
                pathFollower.travelSpeed = pathFollower.defaultSpeed * 2;
                isSpooked = false;
            }
            else
            {
                pathFollower.travelSpeed = pathFollower.defaultSpeed * lastDir;
            }
        }
    }
}
