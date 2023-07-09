using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Grabbable : MonoBehaviour {
    [SerializeField] private float pullSpeed = 5;
    
    private Grabber grabber;

    public Grabber Grabber
    {
        get => grabber;
        set
        {
            grabber = value;
            if (!grabber) body.velocity = Vector2.zero;
        }
    }
    
    private Rigidbody2D body;
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Grabber){
            Grabber.Release();
            float x = Mathf.Round(transform.position.x + 0.5f) -0.5f;
            float y = Mathf.Round(transform.position.y + 0.5f) -0.5f;
            transform.position = new Vector3(x, y, 0);
        }
    }

    private void Update() {
        if (!Grabber) return;
        var distance = Grabber.transform.position - transform.position;
        if (distance.magnitude < 1) return;

        if(Mathf.Abs(distance.x) > Mathf.Abs(distance.y)){
            distance = new Vector2(distance.x, 0).normalized;
        }
        else{
            distance = new Vector2(0, distance.y).normalized;
        }

        body.velocity = distance * pullSpeed;
    }
}
