using UnityEngine;

public class CatQuestNPC : QuestNPC {
    protected override void CheckQuestComplete() {
        Debug.Log("Check quest complete");
        var inventory = UIManager.Instance.GetUI<Inventory>();
        if (inventory.GetItem(0) != null) {
            Debug.Log("COMPLETE QUEST");
            inventory.ClearItem(0);
            CompleteQuest();
        }
    }
}
