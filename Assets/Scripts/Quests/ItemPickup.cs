using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : MonoBehaviour {
    [SerializeField] protected Item item;

    private SpriteRenderer renderer;
    
    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        renderer.sprite = item.sprite;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PickUp();
        }
    }

    protected virtual void PickUp() {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        if (inventory.AddItem(item)) {
            Destroy(gameObject);   
        }
    }
}
