using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Huggable))]
public class GirlQuestNPC : QuestNPC {
    protected override void CheckQuestComplete() {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        if (inventory.GetItem(3) != null && GetComponent<Huggable>().Hugged) {
            inventory.ClearItem(3);
            GetComponent<Animator>().Play("Happy");
            CompleteQuest();
        }
    }
}
