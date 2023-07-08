public class CatQuestNPC : QuestNPC {
    protected override void CheckQuestComplete() {
        var inventory = UIManager.Instance.GetUI<Inventory>();
        if (inventory.GetItem(0) != null) {
            inventory.ClearItem(0);
            CompleteQuest();
        }
    }
}
