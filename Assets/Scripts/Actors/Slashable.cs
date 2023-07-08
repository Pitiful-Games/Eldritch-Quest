using UnityEngine;

public class Slashable : MonoBehaviour {
    [SerializeField] private GameObject unslashedChild;
    [SerializeField] private GameObject slashedChild;
    private Collider2D col;

    private void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Slasher")) {
            Slash();
        }
    }
    
    public virtual void Slash() {
        unslashedChild.SetActive(false);
        slashedChild.SetActive(true);

        col.enabled = false;
    }
}
