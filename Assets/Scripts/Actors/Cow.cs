using PathCreation;
using UnityEngine;

[RequireComponent(typeof(PathFollower))]
public class Cow : ItemPickup {
    [SerializeField] private DetectRange detectRange;
    [SerializeField] private PathCreator initialPath;
    [SerializeField] private PathCreator remainingPath;
    
    private PathFollower pathFollower;
    private bool isSpooked;
    private bool endReached;
    private int lastDir;

    private void Awake() {
        pathFollower = GetComponent<PathFollower>();
        
        detectRange.Detected += OnDetect;
        pathFollower.PathEnded += OnPathEnd;
    }

    private void Start()
    {
        pathFollower.pathCreator = initialPath;
        endReached = false;
        isSpooked = true;
        lastDir = 1;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (endReached) base.OnTriggerEnter2D(collision);
    }

    protected override void PickUp()
    {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        inventory.AddItem(item);
    }

    private void OnPathEnd(PathFollower follower) {
        if (pathFollower.pathCreator != remainingPath)
        {
            pathFollower.ChangePath(remainingPath);
            pathFollower.travelSpeed = 0;
        }
        else{
            pathFollower.travelSpeed = 0;
            detectRange.Detected -= OnDetect;
            endReached = true;
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
                lastDir *= -1;
            }
        }
    }
}
