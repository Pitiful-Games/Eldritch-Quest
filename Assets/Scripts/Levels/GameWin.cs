using UnityEngine;

public class GameWin : MonoBehaviour {
    private void Awake() {
        AudioManager.Instance.PauseMusic();
        UIManager.Instance.DestroyUI<QuestLog>();
    }
}
