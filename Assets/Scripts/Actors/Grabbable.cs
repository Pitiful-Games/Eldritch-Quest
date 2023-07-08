using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Grabbable : MonoBehaviour {
    [SerializeField] private float pullSpeed = 5;
    
    private Grabber grabber;

    public Grabber Grabber {
        get => grabber;
        set {
            grabber = value;
            if (!grabber) body.velocity = Vector2.zero;
        }
    }

    private Rigidbody2D body;
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (!Grabber) return;
        var distance = Grabber.transform.position - transform.position;
        if (distance.magnitude < 1) return;
        body.velocity = distance * pullSpeed;
    }
}
