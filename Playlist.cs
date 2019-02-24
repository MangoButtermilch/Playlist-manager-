using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Playlist {

    #region Variables
    public string name;
    [HideInInspector] public float length;
    public bool saveSongPosition; //if false, then the playlist will start at index 0 again
    [HideInInspector] public bool started;
    [HideInInspector] public bool paused;
    [HideInInspector] public bool finished;
    public int currentSong; //Make this static if you want to keep it between scenes
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

    #region Song control functions
    //Pauses a song
    public void PauseSong(Song song) {
        song.paused = true;
        //PlaylistManager.audioSource.Pause();
    }
    //Unpauses a song
    public void UnPauseSong(Song song) {
        song.paused = false;
        //PlaylistManager.audioSource.UnPause();
    }
    //Loads and plays the next song
    public void NextSong(Song song) {
        //initalize the song to reset the length and all variables
        InitializeSong(song);
        //stop current song
        StopSong(song);

        //increase current song if variable is in range
        if (currentSong < songs.Count - 1) {
            currentSong++;
        } else {
            //if we are out of the range, start at 0 again to restart the playlist
            currentSong = 0;
        }
      
        //Play the next song after currentSong has been increased
        PlaySong(songs[currentSong]);
    }
    //Loads and plays the previous song
    public void PreviousSong(Song song) {
        //initalize the song to reset the length and all variables
        InitializeSong(song);

        //stop current song
        StopSong(song);

        //can't have less than 0 songs
        if (currentSong > 0) {
            currentSong--;
        } else {
            //if we are at 0, go to the last index to start at the end of the playlist
            currentSong = songs.Count - 1;
        }
        //play the song
        PlaySong(songs[currentSong]);
    }
    //Plays a single song
    public void PlaySong(Song song) {
        //Load song at first
        LoadSong(song);
        //if loaded
        if (song.loaded) {
            //song has started and is not finished
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
        //PlaylistManager.audioSource.Stop();
        //song has to be marked as finished, so the length counter will also stop
        song.finished = true;
        //unpause it so we can choose the next and previous again
        UnPauseSong(song);
    }
    //Loads a single song
    private void LoadSong(Song song) {
        if (song.audioClip != null) {
            //load it
            song.audioClip.LoadAudioData();
            if (song.audioClip.isReadyToPlay) {
                //Loaded
                song.loaded = true;
            }
        } else {
            Debug.LogError("Audio clip is null!");
        }
    }
    #endregion

    #region Playlist and song initialization
    //Initalizes this playlist
    public void InitializePlaylist() {
        //Amount of songs
        length = songs.Count;
        started = false;
        paused = false;
        finished = false;
        //you can remove this line if your want
        //to save the current song index
        //you can even make it static so you can switch the scene
        //and come back. So you still have the same song index
        if (!saveSongPosition)
            currentSong = 0;
        //name will be set in the inspector
        //Initialize every song for the playlist
        InitializeSongs();
    } 
    //Initializes a single song
    public void InitializeSong(Song song) {
        //Always reset the length, so we always start at the full clip length if we choose the next or the previous song
        song.length = songs[currentSong].audioClip.length;
        song.loaded = false;
        song.started = false;
        song.finished = false;
        //song.ID already initialized
    }
    //Initializes every song
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
    #endregion
}
