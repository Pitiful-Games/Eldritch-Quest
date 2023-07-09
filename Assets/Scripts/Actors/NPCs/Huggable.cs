using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Huggable : MonoBehaviour {
    public bool Hugged { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabber")) {
            Hug();
        }
    }

    private void Hug() {
        Hugged = true;
    }
}
