using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour {

    //You don't need this class.
    //The playlist manager can work without it.

    public TextMeshProUGUI songName;
    public TextMeshProUGUI playlistName;


    void Start() {
        if (PlaylistManager.currentPl != null) {
            songName.text = PlaylistManager.currentPl.songs[PlaylistManager.currentPl.currentSong].audioClip.name;
            playlistName.text = PlaylistManager.currentPl.name;
        }
    }

    void Update() {
        songName.text = PlaylistManager.currentPl.songs[PlaylistManager.currentPl.currentSong].audioClip.name;
        playlistName.text = PlaylistManager.currentPl.name;
    }
}
