using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Playlist {

    #region Variables
    public string name;
    [HideInInspector] public float length;
    public bool saveSongPosition; 
    [HideInInspector] public bool started;
    [HideInInspector] public bool paused;
    [HideInInspector] public bool finished;
    //Make this static if you want to keep it between scenes
    public int currentSong;

    public bool playRandomSong;
    //plays a random song if the previous song function is called
    public bool playRandomSongPrev;
    [HideInInspector] public int ID;
    public List<Song> songs = new List<Song>();
    #endregion

    #region Constructor
    public Playlist(List<Song> _songs, int _ID, int _currentSong, string _name, bool _started, bool _paused, bool _finished, bool _saveSongPos) {
        songs = _songs;
        started = _started;
        paused = _paused;
        finished = _finished;
        currentSong = _currentSong;
        name = _name;
        ID = _ID;
        saveSongPosition = _saveSongPos;
    }
    #endregion

    #region Playlist and song initialization

    //Initializes a single song
    public void InitializeSong(Song song) {
        //Always reset the length, so we always start at the full clip length if we choose the next or the previous song
        song.length = songs[currentSong].audioClip.length;
        song.loaded = false;
        song.started = false;
        song.finished = false;
        //song.ID already initialized in function below
    }
    //Initializes every song and gives it an ID
    private void InitializeSongs() {
        for (int i = 0; i < songs.Count; i++) {
            songs[i].length = songs[i].audioClip.length;
            songs[i].loaded = false;
            songs[i].started = false;
            songs[i].paused = false;
            songs[i].finished = false;
            songs[i].ID = i;
        }
    }
    //Initalizes this playlist
    public void InitializePlaylist() {
        //Amount of songs
        length = songs.Count;
        started = false;
        paused = false;
        finished = false;
        //If false, the playlist will always start at index 0
        if (!saveSongPosition)
            currentSong = 0;
        //Name is set in the inspector
        //Now initialize every song for the playlist
        InitializeSongs();
    }
    
    #endregion

    #region Song controls

    //Pauses a song
    public void PauseSong(Song song) {
        song.paused = true;
    }
    
    //Unpauses a song
    public void UnPauseSong(Song song) {
        song.paused = false;
    }

    //Plays a single song
    public void PlaySong(Song song) {
        //Load song at first
        LoadSong(song);
        //if loaded
        if (song.loaded) {
            //song has started
            song.finished = false;
            song.started = true;
            //Assign audio clip to source
            PlaylistManager.audioSource.clip = song.audioClip;
            PlaylistManager.audioSource.Play();
        } else {
            Debug.LogError("Audio clip could not be found!");
        }
    }
   
    //Stops a single song
    public void StopSong(Song song) {
        PlaylistManager.audioSource.Stop();
        //song has to be marked as finished, so the length counter will also stop
        song.finished = true;
        //unpause it so we can choose the next and previous again
        UnPauseSong(song);
    }

    //Loads and plays the next song
    public void NextSong(Song song) {
        //initalize the song to reset the length and all variables
        InitializeSong(song);
        //stop current song
        StopSong(song);

        //increase current song if variable is in range of the playlist size
        if (!playRandomSong) {
            if (currentSong < songs.Count - 1) {
                currentSong++;
            } else {
                //if we are out of the range, start at 0 again to restart the playlist
                currentSong = 0;
            }
        } else {
            //we don't want the same song again
            float oldCurrentSong = currentSong;
            //we want a random song!
            while (currentSong == oldCurrentSong) {
                currentSong = Random.Range(0, songs.Count);
            }
        }
        //play the next song after currentSong has been increased
        PlaySong(songs[currentSong]);
    }

    //Loads and plays the previous song
    public void PreviousSong(Song song) {
        //initalize the song to reset the length and all variables
        InitializeSong(song);
        //stop current song
        StopSong(song);
        if (!playRandomSongPrev) {
            //We can't have less than 0 songs
            if (currentSong > 0) {
                currentSong--;
            } else {
                //if we are at 0, go to the last index to start at the end of the playlist
                currentSong = songs.Count - 1;
            }
        }else {
            //we don't want the same song again
            float oldCurrentSong = currentSong;
            //we want a random song!
            while (currentSong == oldCurrentSong) {
                currentSong = Random.Range(0, songs.Count);
            }
        }
        //play the song
        PlaySong(songs[currentSong]);
    }

    //Loads a single song
    private void LoadSong(Song song) {
        if (song.audioClip != null) {
            //load it
            song.audioClip.LoadAudioData();
            if (song.audioClip.loadState == AudioDataLoadState.Loaded) {
                //Loaded
                song.loaded = true;
            } else {
                Debug.LogError("Audio clip could not be loaded!");
            }
        } else {
            Debug.LogError("Audio clip is not assigned!");
        }
    }
    #endregion

  
}
