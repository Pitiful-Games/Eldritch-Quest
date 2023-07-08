using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]
public class Flame : MonoBehaviour, ISpawnable {
    [SerializeField] private AudioClip flameClip;
    
    private Animator animator;
    private ParticleSystem particles;

    private float timer;
    
    private void Awake() {
        animator = GetComponent<Animator>();
        particles = GetComponent<ParticleSystem>();
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer >= particles.main.duration + particles.main.startLifetime.constantMax) {
            FindObjectOfType<FlameSpawner>().Despawn(this);
        }
    }

    public void OnCreate() { }

    public void OnSpawn() {
        timer = 0;
        
        animator.Play("Flame");
        AudioManager.Instance.SpawnAndPlay(flameClip, transform.position);
        particles.Play();
    }

    public void OnDespawn() {
        particles.Stop();
    }

    public void OnDelete() { }
}
