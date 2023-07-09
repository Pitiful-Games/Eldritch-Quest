using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObjects/Quest")]
public class Quest : ScriptableObject {
    public int id;
    public string text;
    public bool completed;

    public void Complete() {
        completed = true;
    }
}
