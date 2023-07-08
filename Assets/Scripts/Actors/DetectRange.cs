using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DetectRange : MonoBehaviour {
    public delegate void OnDetect(Collider2D other);

    public event OnDetect Detected;
    
    private void OnTriggerEnter2D(Collider2D other) {
        //TODO: Player or grapple
        if (other.CompareTag("Player")) {
            Detected?.Invoke(other);
        }
    }
}
