using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Manages dialogue from NPCs.
/// </summary>
public class DialogueManager : BaseUI {
    /// <summary>
    ///     The displayed dialogue typewriter.
    /// </summary>
    [SerializeField] private Typewriter typer;

    private Dialogue currentDialogue;
    private int currentPage;

    /// <summary>
    ///     The currently displayed, translated dialogue text.
    /// </summary>
    private string CurrentText => currentDialogue.pages[currentPage];

    /// <inheritdoc />
    public override void Open() {
        base.Open();

        var player = FindObjectOfType<Player>();
        if (player) player.enabled = false;

        if (UIManager.Instance) UIManager.Instance.Actions.Submit.performed += OnSubmit;
    }

    /// <inheritdoc />
    public override void Close() {
        currentDialogue = null;

        if (UIManager.Instance) UIManager.Instance.Actions.Submit.performed -= OnSubmit;

        Debug.Log("Dialogue enable");
        var player = FindObjectOfType<Player>();
        if (player) player.enabled = true;

        if (UIManager.Instance.GetUI<QuestLog>().AllQuestsComplete) {
            GameManager.Instance.ChangeScene("GameWin");
        }
        
        base.Close();
    }

    /// <summary>
    ///     Start a dialogue.
    /// </summary>
    /// <param name="dialogue">The dialogue data object.</param>
    public void StartDialogue(Dialogue dialogue) {
        currentDialogue = dialogue;
        currentPage = 0;
        typer.Type(CurrentText);
    }

    /// <summary>
    ///     Advance the dialogue to the next page.
    /// </summary>
    private void NextPage() {
        currentPage++;
        if (currentPage >= currentDialogue.pages.Length) {
            Close();
            return;
        }

        typer.Type(CurrentText);
    }

    /// <summary>
    ///     Callback to skip the typewriting effect with dialogue.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OnSubmit(InputAction.CallbackContext context) {
        if (!context.ReadValueAsButton()) return;
        if (typer.IsPrinting)
            typer.Skip();
        else
            NextPage();
    }
}