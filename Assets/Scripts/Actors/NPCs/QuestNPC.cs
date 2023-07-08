using System.Collections;
using UnityEngine;

public abstract class QuestNPC : NPC {
    [SerializeField] private Quest quest;
    [SerializeField] private Dialogue beforeStartQuestDialogue;
    [SerializeField] private Dialogue afterStartQuestDialogue;
    [SerializeField] private Dialogue completedQuestDialogue;

    private void Start() {
        dialogue = beforeStartQuestDialogue;
    }

    public override void Interact() {
        if (dialogue == afterStartQuestDialogue) {
            CheckQuestComplete();
        }
        
        base.Interact();

        StartCoroutine(WaitForDialogueEnd());
    }

    private IEnumerator WaitForDialogueEnd() {
        var dialogueManager = UIManager.Instance.GetUI<DialogueManager>();
        yield return new WaitWhile(() => dialogueManager.gameObject.activeSelf);
        
        if (dialogue == beforeStartQuestDialogue) {
            StartQuest();
        }
    }

    private void StartQuest() {
        var questLog = UIManager.Instance.GetUI<QuestLog>();
        questLog.StartQuest(quest);
        dialogue = afterStartQuestDialogue;
    }

    protected virtual void CheckQuestComplete() { }

    protected void CompleteQuest() {
        var questLog = UIManager.Instance.GetUI<QuestLog>();
        questLog.CompleteQuest(quest);
        dialogue = completedQuestDialogue;
    }
}
