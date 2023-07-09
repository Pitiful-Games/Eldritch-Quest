using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
public class Burnable : MonoBehaviour {
    [SerializeField] private float burnTime = 3;
    [SerializeField] private GameObject unburntChild;
    [SerializeField] private GameObject burntChild;

    private AudioSource audioSource;
    private ParticleSystem burnParticles;
    private Collider2D col;
    
    private float burnTimer;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        burnParticles = GetComponent<ParticleSystem>();
        col = GetComponent<Collider2D>();
    }

    private void Update() {
        if (!burnParticles.isEmitting) return;
        burnTimer += Time.deltaTime;
        if (burnTimer >= burnTime) {
            burnParticles.Stop();
            if (unburntChild) unburntChild.SetActive(false);
            if (burntChild) burntChild.SetActive(true);
            audioSource.Stop();
            col.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Flame")) {
            Burn();
        }
    }

    private void Burn() {
        audioSource.Play();
        burnParticles.Play();
    }
}
