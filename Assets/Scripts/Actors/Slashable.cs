using UnityEngine;

public class Slashable : MonoBehaviour {
    [SerializeField] private GameObject unslashedChild;
    [SerializeField] private GameObject slashedChild;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Slasher")) {
            Slash();
        }
    }
    
    public virtual void Slash() {
        unslashedChild.SetActive(false);
        slashedChild.SetActive(true);
    }
}
