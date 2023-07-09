using UnityEngine;

[RequireComponent(typeof(AudioPlayerPool))]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager> {
    private AudioPlayerPool audioPlayerPool;
    [SerializeField] private AudioSource MenuMusic;
    [SerializeField] private AudioSource HardMusic;
    [SerializeField] private AudioSource SoftMusic;

    public enum Music{ Menu, Hard, Soft}

    protected override void OnAwake() {
        audioPlayerPool = GetComponent<AudioPlayerPool>();
    }
    private void Start()
    {
        PlayMusic(Music.Menu);
    }

    public void PlayMusic(Music clip) {
        PauseMusic();
        switch (clip)
        {
            case Music.Menu:
                MenuMusic.Play();
                break;
            case Music.Hard:
                HardMusic.Play();
                break;
            case Music.Soft:
                SoftMusic.Play();
                break;
            default:
                break;
        }
    }
    
    private void PauseMusic(){
        MenuMusic.Pause();
        HardMusic.Pause();
        SoftMusic.Pause();
    }

    public void SpawnAndPlay(AudioClip clip, Vector3 spawnPosition, float pitchMin = 1, float pitchMax = 1, float minVolume = 1, float maxVolume = 1) {
        var audioPlayer = audioPlayerPool.Spawn();
        audioPlayer.transform.position = spawnPosition;
        audioPlayer.Clip = clip;
        var audioSource = audioPlayer.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(minVolume, maxVolume);
    }

    public void DespawnPlayer(PlayAndDespawn audioPlayer) {
        audioPlayerPool.Despawn(audioPlayer);
    }
}