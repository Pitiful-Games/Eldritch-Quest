using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CatQuestNPC : QuestNPC {
    protected override void CheckQuestComplete() {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        if (inventory.GetItem(0) != null) {
            inventory.ClearItem(0);
            transform.Find("Cat").gameObject.SetActive(true);
            GetComponent<Animator>().Play("Relieved");
            CompleteQuest();
        }
    }
}
