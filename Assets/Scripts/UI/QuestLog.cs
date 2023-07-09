using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestLog : BaseUI {
    [SerializeField] private TextMeshProUGUI questPrefab;
    private List<Quest> quests = new();

    public bool AllQuestsComplete => quests.Count(quest => quest.completed) >= 3;

    public void StartQuest(Quest quest) {
        var questTmp = Instantiate(questPrefab, transform);
        questTmp.text = quest.text;
        quests.Add(quest);
    }
    
    public void CompleteQuest(Quest quest) {
        var childIndex = quests.IndexOf(quest);
        var questTmp = transform.GetChild(childIndex).GetComponent<TextMeshProUGUI>();
        questTmp.fontStyle = FontStyles.Strikethrough;
        questTmp.color = Color.gray;
        quest.Complete();
        FindObjectOfType<Player>().AddQuestAddition(quest.id);
    }
}
