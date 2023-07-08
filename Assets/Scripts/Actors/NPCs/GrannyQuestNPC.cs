public class GrannyQuestNPC : QuestNPC {
    protected override void CheckQuestComplete() {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        if (inventory.GetItem(1) != null && inventory.GetItem(2) != null) {
            inventory.ClearItem(1);
            inventory.ClearItem(2);
            CompleteQuest();
        }
    }
}
