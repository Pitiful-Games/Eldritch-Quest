using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    [SerializeField] private Image itemImage;

    private Item item;

    public Item Item {
        get => item;
        private set {
            item = value;
            itemImage.sprite = item ? item.sprite : null;
        }
    }

    public void SetItem(Item newItem) {
        Item = newItem;
    }

    public void ClearItem() {
        Item = null;
    }
}
