using UnityEngine;
using System.Collections;

public class PlaylistSoundManager : MonoBehaviour {

    public AudioClip[] Clips;
    private int currentId;
    private static PlaylistSoundManager _instance = null;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        PlayRandomSound();
	}
	
	// Update is called once per frame
	void Update () {
        if (!audio.isPlaying)
            PlayRandomSound();
	}

    void PlayRandomSound()
    {
        currentId = Random.Range(0, Clips.Length);
        audio.clip = Clips[currentId];
        audio.Play();
    }
}
