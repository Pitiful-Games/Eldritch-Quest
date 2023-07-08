using UnityEngine;

public class Grabber : MonoBehaviour {
    [SerializeField] private RandomAudioClipSelector grabSelector;
    [SerializeField] private Animator parentAnimator;

    private AnimatorStateInfo animatorState => parentAnimator.GetCurrentAnimatorStateInfo(0);
    
    private Transform grabbed;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable")) {
            Grab(other.transform);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
            Retract();
        }
    }

    public void Grab(Transform grabbedTransform) {
        grabbedTransform.SetParent(transform);
        grabbedTransform.position = transform.position;
        grabbed = grabbedTransform;
    }

    public void Reach() {
        grabSelector.PlayRandomClip(transform.position);
        var duration = animatorState.length / 2;
        Debug.Log("Reach duration: " + duration);
        iTween.ScaleTo(gameObject, iTween.Hash(
            "name", "Reach",
            "x", 2,
            "time", duration,
            "oncomplete", "Retract"
        ));
    }

    public void Release() {
        if (grabbed == null) return;
        grabbed.SetParent(null);
        grabbed.transform.localScale = Vector3.one;
        grabbed.transform.rotation = Quaternion.identity;
        grabbed = null;
    }

    public void Retract() {
        iTween.StopByName("Reach");
        var duration = animatorState.length - parentAnimator.GetCurrentTime();
        Debug.Log("Retract duration: " + duration);
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
