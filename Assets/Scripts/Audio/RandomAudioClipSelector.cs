using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Random Audio Clip Selector", menuName = "ScriptableObjects/Random Audio Clip Selector")]
public class RandomAudioClipSelector : ScriptableObject {
    [SerializeField] private AudioClip[] audioClips;
    
    public AudioClip GetRandomClip() {
        if (audioClips.Length <= 0) {
            throw new ArgumentOutOfRangeException(nameof(audioClips));
        }

        return audioClips[Random.Range(0, audioClips.Length)];
    }

    public void PlayRandomClip(Vector3 spawnPosition, float pitchMin = 0.85f, float pitchMax = 1.15f, float minVolume = 1, float maxVolume = 1) {
        var randomClip = GetRandomClip();
        AudioManager.Instance.SpawnAndPlay(randomClip, spawnPosition, pitchMin, pitchMax, minVolume, maxVolume);
    }
}
