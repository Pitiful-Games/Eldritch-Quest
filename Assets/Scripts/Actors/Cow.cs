using PathCreation;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class Cow : MonoBehaviour {
    [SerializeField] private DetectRange detectRange;
    [SerializeField] private PathCreator initialPath;
    [SerializeField] private PathCreator remainingPath;
    
    private PathFollower pathFollower;
    private bool isSpooked;

    private void Awake() {
        pathFollower = GetComponent<PathFollower>();
        
        detectRange.Detected += OnDetect;
        pathFollower.PathEnded += OnPathEnd;
    }

    private void OnPathEnd(PathFollower follower) {
        if (follower == pathFollower && isSpooked) {
            isSpooked = false;
            pathFollower.travelSpeed /= 2;
        }
    }

    private void Start() {
        pathFollower.pathCreator = initialPath;
    }

    private void OnDetect(Collider2D other) {
        if (isSpooked) return;
        Spook();
    }


    private void Spook() {
        isSpooked = true;
        if (pathFollower.pathCreator != remainingPath) {
            pathFollower.ChangePath(remainingPath);
        }

        pathFollower.travelSpeed *= 2;
    }
}
