using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Burnable : MonoBehaviour {
    [SerializeField] private float burnTime = 3;
    private ParticleSystem burnParticles;
    
    private float burnTimer;
    
    private void Awake() {
        burnParticles = GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (!burnParticles.isEmitting) return;
        burnTimer += Time.deltaTime;
        if (burnTimer >= burnTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger enter: " + other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Flame")) {
            Burn();
        }
    }

    private void Burn() {
        burnParticles.Play();
    }
}
