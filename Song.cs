
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

}
