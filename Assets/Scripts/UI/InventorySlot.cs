using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite emptySprite;

    private Item item;

    public Item Item {
        get => item;
        private set {
            item = value;
            itemImage.sprite = item ? item.sprite : emptySprite;
        }
    }

    public void SetItem(Item newItem) {
        Item = newItem;
    }

    public void ClearItem() {
        Item = null;
    }
}
