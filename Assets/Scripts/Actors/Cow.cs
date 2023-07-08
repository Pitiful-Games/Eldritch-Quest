using PathCreation;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class Cow : MonoBehaviour {
    [SerializeField] private DetectRange detectRange;
    [SerializeField] private PathCreator initialPath;
    [SerializeField] private PathCreator remainingPath;

    [SerializeField] private LayerMask mask;

    private PathFollower pathFollower;
    private Vector3 lastPos;
    private Collider2D col;
    [SerializeField] private cowPickup pickup;

    private bool isSpooked;
    private int lastDir;

    private void Awake() {
        pathFollower = GetComponent<PathFollower>();
        col = GetComponent<Collider2D>();
        detectRange.Detected += OnDetect;
        pathFollower.PathEnded += OnPathEnd;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, (transform.position - lastPos).normalized);
        if (Physics2D.Raycast(transform.position, (transform.position - lastPos).normalized, 0.7f, mask) && pathFollower.pathCreator == remainingPath)
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
        if (pathFollower.pathCreator != remainingPath)
        {
            pathFollower.ChangePath(remainingPath);
            pathFollower.travelSpeed = 0;
        }
        else{
            transform.position = lastPos;
            pathFollower.travelSpeed = 0;
            detectRange.Detected -= OnDetect;
            col.enabled = false;
            pickup.gameObject.SetActive(true);
        }
    }

    private void OnDetect(Collider2D other) {
        if (pathFollower.travelSpeed == 0)
        {
            if (isSpooked)
            {
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
