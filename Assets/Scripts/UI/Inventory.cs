using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : BaseUI {
    [SerializeField] private int numSlots;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private float upTime;
    [SerializeField] private float fadeTime;
    
    private List<InventorySlot> slots = new();

    private float upTimer;
    private float fadeTimer;

    private void Awake() {
        for (var i = 0; i < numSlots; i++) {
            slots.Add(Instantiate(slotPrefab, transform));
        }
    }

    private void Update() {
        upTimer += Time.deltaTime;
        
        if (upTimer < upTime) return;
        
        fadeTimer += Time.deltaTime;
        
        foreach (var canvasRenderer in GetComponentsInChildren<CanvasRenderer>()) {
            canvasRenderer.SetColor(Color.Lerp(Color.white, Color.clear, fadeTimer / fadeTime));
        }

        if (fadeTimer >= fadeTime) {
            gameObject.SetActive(false);
        }
    }

    public override void Open() {
        base.Open();

        upTimer = 0;
        fadeTimer = 0;
        
        foreach (var canvasRenderer in GetComponentsInChildren<CanvasRenderer>()) {
            canvasRenderer.SetColor(Color.white);
        }
    }

    public bool AddItem(Item item) {
        var slotNum = slots.FindIndex(slot => slot.Item == null);
        if (slotNum >= 0) {
            SetItem(slotNum, item);
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public void SetItem(int slotNum, Item item) {
        CheckSlotNumber(slotNum);
        slots[slotNum].SetItem(item);
    }

    public void ClearItem(int itemId) {
        var slotNum = slots.FindIndex(slot => slot.Item != null && slot.Item.id == itemId);
        if (slotNum >= 0) {
            slots[slotNum].ClearItem();   
        }
    }

    public Item GetItem(int itemId) {
        return slots.Select(slot => slot.Item).FirstOrDefault(item => item != null && item.id == itemId);
    }

    private void CheckSlotNumber(int slotNum) {
        if (slotNum < 0 || slotNum >= numSlots) {
            throw new ArgumentOutOfRangeException(nameof(slotNum));
        }
    }
}
