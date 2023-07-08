using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Burnable : MonoBehaviour {
    [SerializeField] private float burnTime = 3;
    [SerializeField] private GameObject unburntChild;
    [SerializeField] private GameObject burntChild;
    private ParticleSystem burnParticles;
    private Collider2D col;
    
    private float burnTimer;
    
    private void Awake() {
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
            col.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Flame")) {
            Burn();
        }
    }

    private void Burn() {
        burnParticles.Play();
    }
}
