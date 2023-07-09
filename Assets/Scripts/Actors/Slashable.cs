using UnityEngine; 

[RequireComponent(typeof(AudioSource))]
public class Slashable : MonoBehaviour {
    [SerializeField] private GameObject unslashedChild;
    [SerializeField] private GameObject slashedChild;
    [SerializeField] private RandomAudioClipSelector slashSoundSelector;

    private AudioSource audioSource;
    private Collider2D col;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = slashSoundSelector.GetRandomClip();
    }

    private void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Slasher")) {
            Slash();
        }
    }
    
    public virtual void Slash() {
        unslashedChild.SetActive(false);
        slashedChild.SetActive(true);
        audioSource.Play();

        col.enabled = false;
    }
}
