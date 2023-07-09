using System.Collections;
using UnityEngine;

public abstract class QuestNPC : NPC {
    [SerializeField] private Quest quest;
    [SerializeField] private Dialogue beforeStartQuestDialogue;
    [SerializeField] private Dialogue afterStartQuestDialogue;
    [SerializeField] private Dialogue completedQuestDialogue;
    [SerializeField] private Dialogue finalDialogue;
    [SerializeField] private AudioClip dialogueSound;

    private void Start() {
        dialogue = beforeStartQuestDialogue;
    }

    public override void Interact() {
        if (dialogue == afterStartQuestDialogue) {
            CheckQuestComplete();
        }

        AudioManager.Instance.SpawnAndPlay(dialogueSound, transform.position, 0.85f, 1.15f);
        
        base.Interact();

        StartCoroutine(WaitForDialogueEnd());
    }

    private IEnumerator WaitForDialogueEnd() {
        var dialogueManager = UIManager.Instance.GetUI<DialogueManager>();
        yield return new WaitWhile(() => dialogueManager.gameObject.activeSelf);
        
        if (dialogue == beforeStartQuestDialogue) {
            StartQuest();
        } else if (dialogue == completedQuestDialogue) {
            dialogue = finalDialogue;
        }
    }

    private void StartQuest() {
        var questLog = UIManager.Instance.GetUI<QuestLog>();
        questLog.StartQuest(quest);
        dialogue = afterStartQuestDialogue;
        AudioManager.Instance.PlayMusic(AudioManager.Music.Hard);
    }

    protected virtual void CheckQuestComplete() { }

    protected void CompleteQuest() {
        var questLog = UIManager.Instance.GetUI<QuestLog>();
        questLog.CompleteQuest(quest);
        dialogue = completedQuestDialogue;
        AudioManager.Instance.PlayMusic(AudioManager.Music.Soft);
    }
}
