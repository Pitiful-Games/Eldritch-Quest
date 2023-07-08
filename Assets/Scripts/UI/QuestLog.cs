using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestLog : BaseUI {
    [SerializeField] private TextMeshProUGUI questPrefab;
    private List<Quest> quests = new();

    public void StartQuest(Quest quest) {
        var questTmp = Instantiate(questPrefab.gameObject, transform).GetComponent<TextMeshProUGUI>();
        questTmp.text = quest.text;
        quests.Add(quest);
    }
    
    public void CompleteQuest(Quest quest) {
        var questId = quest.id;
        var questTmp = transform.GetChild(questId).GetComponent<TextMeshProUGUI>();
        questTmp.fontStyle = FontStyles.Strikethrough;
        questTmp.color = Color.gray;
        FindObjectOfType<Player>().AddQuestAddition(quest.id);
        quests.Remove(quest);
    }
}
