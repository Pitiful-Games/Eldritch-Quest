using UnityEngine;

public class RaspberryBush : Slashable {
    [SerializeField] private ItemPickup raspberryPickup;
    
    public static int NumRaspberryBushes { get; private set; }

    private void Awake() {
        NumRaspberryBushes = FindObjectsOfType<RaspberryBush>().Length;
    }

    public override void Slash() {
        base.Slash();

        NumRaspberryBushes--;
        if (NumRaspberryBushes <= 0) {
            Instantiate(raspberryPickup, transform.position, Quaternion.identity);
        }
    }
}
