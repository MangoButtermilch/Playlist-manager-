
using UnityEngine;

[System.Serializable]
public class Song {

    public AudioClip audioClip;
    [HideInInspector] public float length;
    [HideInInspector] public bool loaded;
    [HideInInspector] public bool started;
    [HideInInspector] public bool paused;
    [HideInInspector] public bool finished;
    [HideInInspector] public int ID;

    //Constructor
    public Song(AudioClip _audioClip, int _ID, float _length, bool _started, bool _paused, bool _finished) {
        audioClip = _audioClip;
        //length = _length;
        length = _audioClip.length;
        started = _started;
        paused = _paused;
        finished = _finished;
        _ID = ID;
    }
}
