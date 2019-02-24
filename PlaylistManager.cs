using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class PlaylistManager : MonoBehaviour {

    #region Variables
    //can be changed to static int, if you want to keep your playlist between scenes
    public int currentPlaylist;
    public static AudioSource audioSource;
    public List<Playlist> playlists = new List<Playlist>();
    //For UI Manager, can be removed if you don't need this
    public static Playlist currentPl;
    #endregion

    
    private void Start() {
        //currentPl is used in the UI manager
        currentPl = playlists[currentPlaylist];
        audioSource = GetComponent<AudioSource>();
        //Initialize all playlists
        InitializePlaylists();
        //Start first playlist
        StartPlaylist(playlists[currentPlaylist]);
    }

    private void Update() {
        //For UI Manager
        currentPl = playlists[currentPlaylist];
        //Check the song length of the current play list
        CheckPlaylistSongLength(playlists[currentPlaylist]);
    }

    #region Functions song and playlist controls (Usable for example for UI buttons)
    //Load and play the next song
    public void NextSong() {
        //unpause the playlist
        UnPausePlaylist();

        //get the current play list
        Playlist playlist = playlists[currentPlaylist];
        //get the current playing song
        int currentSong = playlist.currentSong;


        //initalize the song to reset the length and all variables
        playlist.InitializeSong(playlist.songs[currentSong]);

        //next song from the current playlist from the current song
        playlist.NextSong(playlist.songs[currentSong]);

    }
    //Load and play the previous song
    public void PreviousSong() {
        //unpause the playlist
        UnPausePlaylist();

        //get the current play list
        Playlist playlist = playlists[currentPlaylist];
        //get the current playing song
        int currentSong = playlist.currentSong;

        //initalize the song to reset the length and all variables
        playlist.InitializeSong(playlist.songs[currentSong]);

        //next song from the current playlist from the current song
        playlist.PreviousSong(playlist.songs[currentSong]);

    }
    //Loads and starts the previous playlist. Song index can be saved if enabled inside the playlist
    public void PreviousPlaylist() {

        //stop current playlist
        StopPlaylist(playlists[currentPlaylist]);

        //can't have less than 0 playlists
        if (currentPlaylist > 0) {
            //increase variable
            currentPlaylist--;
        } else {
            //if we are at 0, set it to the end of our range. So we start at the end of the playlist
            currentPlaylist = playlists.Count - 1;
        }
        //start with the next playlist after currentPlaylist has been increased
        StartPlaylist(playlists[currentPlaylist]);
    }
    //Loads and starts the next playlist. Song index can be saved if enabled inside the playlist
    public void NextPlaylist() {

        //stop current playlist
        StopPlaylist(playlists[currentPlaylist]);

        //increase currentPlaylist if we are in the range
        if (currentPlaylist < playlists.Count - 1) {
            //increase variable
            currentPlaylist++;
        } else {
            //if we are out of the range, we are at the end. So start it again with 0
            currentPlaylist = 0;
        }
        //start with the next playlist after currentPlaylist has been increased
        StartPlaylist(playlists[currentPlaylist]);
    }
    //Pauses the current playlist and its current song. 
    public void PausePlaylist() {
        Playlist playlist = playlists[currentPlaylist];
        int currentSong = playlist.currentSong;

        //pause the current song of the playlist
        playlist.PauseSong(playlist.songs[currentSong]);
        //mark as paused
        playlist.paused = true;

        //pause the audio source
        audioSource.Pause();
    }
    ///Unpauses the current playlist and its current song. 
    public void UnPausePlaylist() {
        Playlist playlist = playlists[currentPlaylist];
        int currentSong = playlist.currentSong;
        //unpause the current song of the playlist
        playlist.UnPauseSong(playlist.songs[currentSong]);
        //unmark as paused
        playlist.paused = false;

        //pause the audio source
        audioSource.UnPause();
    }
    #endregion

    #region Functions for start and update method
    // Call in Update. Checks the length of the current playing song of the current playlist
    private void CheckPlaylistSongLength(Playlist playlist) {
        int currentSong = playlist.currentSong;

        //if the playlist has started and our current song/playlist is not paused
        if (playlist.started && !playlist.paused && !playlist.songs[currentSong].paused) {

            //Count down the length of the audio clip
            playlist.songs[currentSong].length -= Time.deltaTime;
            //if less or equal to zero
            if (playlist.songs[currentSong].length <= 0f) {
                //Song has ended, so start the next
                playlist.NextSong(playlist.songs[currentSong]);
            }
        }
    }
    //Initializes all playlists
    private void InitializePlaylists() {
        //Set current playlist to 0
        currentPlaylist = 0;
        for (int i = 0; i < playlists.Count; i++) {
            //Define different IDs for every playlist
            playlists[i].ID = i;
        }
        foreach (Playlist pl in playlists) {
            pl.InitializePlaylist();
        }
    }
    #endregion



    //Starts and plays a playlist and its current song
    private void StartPlaylist(Playlist playlist) {
        //initialize playlist
        playlist.InitializePlaylist();
        //get the current song
        int currentSong = playlist.currentSong;
        //Playlist started
        playlist.started = true;
        //Play the current song inside the playlist
        playlist.PlaySong(playlist.songs[currentSong]);

    }
    //Stops a playlist and its songs
    private void StopPlaylist(Playlist playlist) {
        playlist.started = false;
        playlist.paused = false;
        //set playlist to finished
        playlist.finished = true;

        //go through all songs
        for (int i = 0; i < playlist.songs.Count; i++) {
            //stop all songs
            playlist.StopSong(playlist.songs[i]);
        }
        //stop audioSource
        audioSource.Stop();
    }
  

}
