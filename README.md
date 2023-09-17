# Mudskippy's Jukebox

This program is intended to be used with a specific VRChat avatar on PC. This program not only plays music for other users in VRChat, but if a playlist is loaded users can actually navigate through the playlist. The users can also pause/resume the current track using the middle lever of the avatar.

This has been tested on Windows 10 and 11. Also been tested with Google Chrome and Microsoft Edge. Anything else may have varying results.

## Installation Instructions
1: You'll need something that forwards audio output to microphone input. VB-Audio does this pretty well, but theoretically anything that does this should work. https://vb-audio.com/Cable/VirtualCables.htm

2: The fact you're reading this implies you have a browser that supports JavaScript, so you're good on that.

3: Probably goes without saying, but make sure to extract the files if you download this as a zip folder.

## Setup Instructions
1: Run the executable file (If you got from Releases) or compile the source code yourself (If you just cloned the repository). Make sure you allow Firewall access in the popup if it shows up. This is needed for Open Sound Control to work.

2: Open the HTML file. The webpage should connect to the bridge. If you opened the HTML file before the executable, you may need to click the re-connect button.

3: Open VRChat, if you haven't already.

4: Equip the "Jukebox OSC (Minecraft)" avatar (Name may be subject to change). This can be done anytime, if you feel like it. If you can't find the avatar, here's a link: https://vrchat.com/home/avatar/avtr_bb03466d-31c8-456e-933a-387f5b0515c2

5: Change your VRChat settings so the microphone input is set to the virtual microphone (VB-Audio's, if you used that). Use the volume mixer to set your browser's audio output to the associated virtual speaker. Audio played from your browser should now be outputted to VRChat!

6: ENABLE OSC IN VRCHAT IF IT'S NOT ALREADY! This can be done in the action (wheel) menu by going to Settings>OSC>Enable. If it's already enabled, you can skip this step.

7: Make sure your Avatar Interactions are set to everyone so they can pull the levers.

8: When all the previous steps are done. Press the 'Automatic' button in the webpage to start accepting inputs from other users!

## What effect does the avatar have on the webpage?
In automatic mode, the special avatar can have several effects on the webpage. The Next and Back levers of the avatar only affect playlists. As expected, they will play the Next or Previous video in the playlist, respectively. The Play/Pause lever will pause a playing video and play a paused video. The top of the avatar has a slot for discs. It's compatible with discs held by the "0valler (Minecraft)" avatar. https://vrchat.com/home/avatar/avtr_15149189-454c-45cd-b443-29e3e581b947 . When a disc is inserted, it will play a video corresponding to the disc (normally, the music of that disc).

## Operating the webpage
The HTML is effectively the GUI of the program. The input text box expects for Youtube URLs to be inputted. This will change what video is being played. It accepts normal links, shortened links, and even playlist links. However, it won't work with YouTube Music links. Due to embed limitations, certain videos aren't compatible.

The webpage has 2 modes: Manual and Automatic. Manual is for when you don't want the Levers/Discs to have an effect on the video. This is intended for if you want to manually play videos on the youtube site that don't work on the webpage. Turning off Avatar Interactions basically does the same thing, but Manual mode also hides the levers. Automatic is for when you want Levers/Discs to have an effect.

Remember: Even in Automatic mode, you can still interact with the embed video itself for direct control. Really helpful if you want to get to a particular video in a playlist.

## What's with the chatbox?
The program utilizes VRChat's chatbox to get information across about the video. This includes the video's title and how far through the video is through playing. This information updates every 2 seconds. This also lets players know when the jukebox is in Manual mode.

## Why is the chatbox sometimes not updating?
This is a consequence of VRCHat limiting how often you can update the chatbox. If messages are sent too frequently, it'll stop accepting for a bit.
