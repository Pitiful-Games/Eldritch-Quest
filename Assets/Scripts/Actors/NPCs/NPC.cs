using UnityEngine;

public class NPC : Interactable {
    /// <summary>
    ///     The dialogue object used when interacting with this NPC.
    /// </summary>
    [SerializeField] protected Dialogue dialogue;

    /// <inheritdoc />
    public override void Interact() {
        if (UIManager.Instance == null) return;
        CanInteract = false;
        var dialogueManager = UIManager.Instance.OpenUI<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);
    }
}