using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class MenuButton : MonoBehaviour, IPointerClickHandler {
    [SerializeField] private AudioClip clickSound;

    public void OnPointerClick(PointerEventData eventData) {
        AudioManager.Instance.SpawnAndPlay(clickSound, transform.position);
    }
}
