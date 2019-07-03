using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaylistManager : MonoBehaviour {

    #region Variables
    //Make this static, if you want to keep the playlist position between scenes
    public int currentPlaylist;
    public bool randomPlaylist;
    //plays a random playlist if previous function is called
    public bool randomPlaylistPrev;
    public static AudioSource audioSource;
    public List<Playlist> playlists = new List<Playlist>();

    //For UI Manager, can be removed if you don't use the UI manager
    public static Playlist currentPl;
    #endregion

    private void Start() {
        //currentPl is used in the UI manager script
        currentPl = playlists[currentPlaylist];
        audioSource = GetComponent<AudioSource>();
        //Initialize all playlists
        InitializePlaylists();
        //Start first playlist
        StartPlaylist(playlists[currentPlaylist]);
    }

    private void Update() {
        //For UI Manager script
        currentPl = playlists[currentPlaylist];
        //Check the song length of the current play list
        CheckSongLength(playlists[currentPlaylist]);
    }

    #region Initialization (call in start method)
    //Initializes all playlists
    private void InitializePlaylists() {
        //Set current playlist to 0
        currentPlaylist = 0;
        for (int i = 0; i < playlists.Count; i++) {
            //different IDs for every playlist
            playlists[i].ID = i;
        }
        //Initialize each playlist
        foreach (Playlist pl in playlists) {
            pl.InitializePlaylist();
        }
    }
    #endregion

    #region Checks the song length (call in Update)
    // Call in Update. Checks the length of the current playing song of the current playlist
    private void CheckSongLength(Playlist playlist) {
        int currentSong = playlist.currentSong;

        //if the playlist has started and our current song/playlist is not paused
        if (playlist.started && !playlist.paused && !playlist.songs[currentSong].paused) {

            //count down the length of the audio clip
            playlist.songs[currentSong].length -= Time.deltaTime;
            //if less or equal to zero
            if (playlist.songs[currentSong].length <= 0f) {
                //search for a random song
                if (playlist.playRandomSong == true) {
                    currentSong = Random.Range(0, playlist.songs.Count);
                }
                //Song has ended, so start the next
                playlist.NextSong(playlist.songs[currentSong]);
            }
        }
    }

    #endregion

    #region Song and playlist controls (usable for example for UI buttons)

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
        //audio source will be controlled from the playlist 

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
        //audio source will be controlled from the playlist 
    }


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

        //search for a random song if enabled
        if (playlist.playRandomSong == true) {
            currentSong = Random.Range(0, playlist.songs.Count);
        }

        //next song from the current playlist from the current song
        playlist.PreviousSong(playlist.songs[currentSong]);

    }

    //Loads and starts the previous playlist. Song index can be saved if enabled inside the playlist
    public void PreviousPlaylist() {

        //stop current playlist
        StopPlaylist(playlists[currentPlaylist]);
        if (!randomPlaylistPrev) {
            //can't have less than 0 playlists
            if (currentPlaylist > 0) {
                //increase variable
                currentPlaylist--;
            } else {
                //if we are at 0, set it to the end of our range. So we start at the end of the playlist
                currentPlaylist = playlists.Count - 1;
            }
        } else {
            //we don't want the same playlist again
            float oldPlaylist = currentPlaylist;
            //we want a random playlist!
            while (currentPlaylist == oldPlaylist) {
                currentPlaylist = Random.Range(0, playlists.Count);
            }
        }
        //start with the next playlist after currentPlaylist has been increased
        StartPlaylist(playlists[currentPlaylist]);
    }

    //Loads and starts the next playlist. Song index can be saved if enabled inside the playlist
    public void NextPlaylist() {

        //stop current playlist
        StopPlaylist(playlists[currentPlaylist]);

        if (!randomPlaylist) {
            //increase currentPlaylist if we are in the range
            if (currentPlaylist < playlists.Count - 1) {
                //increase variable
                currentPlaylist++;
            } else {
                //if we are out of the range, we are at the end. So start it again with 0
                currentPlaylist = 0;
            }
        } else {
            //we don't want the same playlist again
            float oldPlaylist = currentPlaylist;
            //we want a random playlist!
            while (currentPlaylist == oldPlaylist) {
                currentPlaylist = Random.Range(0, playlists.Count);
            }
        }
        //start with the next playlist after currentPlaylist has been increased
        StartPlaylist(playlists[currentPlaylist]);
    }

    //Pauses the current playlist and its current song
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

    //Unpauses the current playlist and starts playing its current song
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

}
