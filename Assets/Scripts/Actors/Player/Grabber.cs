using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {
    [SerializeField] private RandomAudioClipSelector grabSelector;
    [SerializeField] private Animator parentAnimator;

    private AnimatorStateInfo animatorState => parentAnimator.GetCurrentAnimatorStateInfo(0);

    private List<Grabbable> grabbed = new();
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Grabbable>(out var grabbable)) {
            Grab(grabbable);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            Retract();
        }
    }

    public void Grab(Grabbable grabbable) {
        grabbable.Grabber = this;
        grabbed.Add(grabbable);
        Retract();
    }

    public void Reach() {
        grabSelector.PlayRandomClip(transform.position);
        var duration = animatorState.length / 2;
        transform.localScale = new Vector3(1 / 3.0f, 1, 1);
        iTween.ScaleTo(gameObject, iTween.Hash(
            "name", "Reach",
            "x", 2,
            "time", duration,
            "oncomplete", "Retract"
        ));
    }

    public void Release() {
        if (grabbed.Count <= 0) return;
        foreach (var grabbable in grabbed) grabbable.Grabber = null;
        grabbed.Clear();
    }

    public void Retract() {
        iTween.StopByName("Reach");
        var duration = animatorState.length - parentAnimator.GetCurrentTime();
        iTween.ScaleTo(gameObject, iTween.Hash(
            "x", 1 / 3.0f,
            "time", duration,
            "oncomplete", "OnRetractComplete"
        ));
    }

    private void OnRetractComplete() {
        Release();
        gameObject.SetActive(false);
    }
}
