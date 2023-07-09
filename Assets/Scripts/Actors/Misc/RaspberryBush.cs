using UnityEngine;

public class RaspberryBush : MonoBehaviour {
    [SerializeField] private ItemPickup raspberryPickup;
    
    public static int NumRaspberryBushes { get; set; }

    private void Awake() {
        NumRaspberryBushes++;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Slasher")) {
            NumRaspberryBushes--;
            if (NumRaspberryBushes <= 0) {
                Instantiate(raspberryPickup, transform.position, Quaternion.identity);
            }
        }
    }
}
