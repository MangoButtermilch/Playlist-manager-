#You don't need the UIManager.cs script, the code will work without it.

#The code is completly free to use.

#You can easily modify the code the way you want to.

#Installation:

1. Copy these scripts into your project.
2. Create an empty game object.
3. Attach the PlaylistManager.cs script to it.
4. Assign your playlists and songs.
5. Add the methods to buttons or something else.
5. Done.

#Documentation

- Variables
  - Current playlist: Shows which playlist is currently active
  - Random playlist: If activated, searches for a random playlist if NextPlaylist() is called
  - Random playlist prev: If activated, searches for a random playlist if PreviousPlaylist() is called
  
  - Save song position: Saves the song position if you switch to another playlist
  - Play random song: If activated, searches for a random song inside the current playlist if NextSong() is called
  - Play random song prev: If activated, searches for a random song inside the current playlist if PreviousSong() is called
  
- #methods needed for controlling the playlist their songs:
  - NextSong(): plays the next song
  - PreviousSong(): plays the previous song
  - PreviousPlaylist(): plays the previous playlist
  - NextPlaylist(): plays the next playlist
  - PausePlaylist(): pauses the current playlist
  - UnpausePlaylist(): unpauses the current playlist
