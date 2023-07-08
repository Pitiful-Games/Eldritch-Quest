using UnityEngine;

public class Grabber : MonoBehaviour {
    private Transform grabbed;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable")) {
            var otherTransform = other.transform;
            otherTransform.SetParent(transform);
            otherTransform.position = transform.position;
            grabbed = otherTransform;
        }
    }

    public void Release() {
        if (grabbed == null) return;
        grabbed.SetParent(null);
        grabbed.transform.localScale = Vector3.one;
        grabbed = null;
    }
}
