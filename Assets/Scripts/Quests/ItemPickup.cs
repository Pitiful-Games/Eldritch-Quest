using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : MonoBehaviour {
    [SerializeField] protected Item item;

    private SpriteRenderer spriteRenderer;
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        spriteRenderer.sprite = item.sprite;
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
