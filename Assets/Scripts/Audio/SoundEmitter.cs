using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour {
    [SerializeField] private RandomAudioClipSelector audioClipSelector;
    [SerializeField] private float minTimeBetweenSounds;
    [SerializeField] private float maxTimeBetweenSounds;

    private AudioSource audioSource;
    
    private float currentTimeBetweenSounds;
    private float timeSinceLastSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        currentTimeBetweenSounds = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }

    private void Update() {
        timeSinceLastSound += Time.deltaTime;
        if (timeSinceLastSound >= currentTimeBetweenSounds) {
            EmitSound();
        }
    }

    public void EmitSound() {
        timeSinceLastSound = 0;
        currentTimeBetweenSounds = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);

        audioSource.PlayOneShot(audioClipSelector.GetRandomClip());
    }
}
