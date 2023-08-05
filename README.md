# Mudskippy's Jukebox

This program is intended to be used with a specific VRChat avatar on PC. This program not only plays music for other users in VRChat, but if a playlist is loaded users can actually navigate through the playlist. The users can also pause/resume the current track using the middle lever of the avatar.

This has only been tested on Windows 11 and Google Chrome. Results may vary on different setups.

## Installation Instructions
1: You'll need something that forwards audio output to microphone input. VB-Audio does this pretty well, but theoretically anything that does this should work. https://vb-audio.com/Cable/VirtualCables.htm

2: You'll need Node.js to run the NodeBridge.js script. https://nodejs.org/en/download

3: The fact you're reading this implies you have a Browser that supports JavaScript, so you're good on that. Google Chrome will work. I really shouldn't have to tell you how to get Google Chrome.

4: Probably goes without saying, but make sure to extract the files if you download this as a zip folder.

5: While in the folder containing the code, you may need to run 'npm install' to get the dependencies. The only direct dependency the script has is the "osc" module, but that may have it's own dependencies.

## Setup Instructions
1: You may either attempt to use the scuffed .sh file, or just manually run NodeBridge.js using "node NodeBridge.js" in a command prompt.

2: Open the HTML file. This needs to be done AFTER the NodeBridge.js has been opened. Socket connections will screw up if it's done the other way. If you ran the scuffed batch file, both steps 1 and 2 should be done automatically.

3: Open VRChat, if you haven't already.

4: Equip the "Jukebox OSC (Minecraft)" avatar (Name may be subject to change). This can be done anytime, if you feel like it. If you can't find the avatar, here's a link: https://vrchat.com/home/avatar/avtr_bb03466d-31c8-456e-933a-387f5b0515c2

5: Change your VRChat settings so the microphone input is set to VB-Audio's virtual mic. Use the volume mixer to set Google Chrome's audio output to VB-Audio's virtual speaker. Audio played from your browser should now be outputted to VRChat!

6: ENABLE OSC IN VRCHAT! This can be done in the action (wheel) menu by going to Settings>OSC>Enable. If it's already enabled, you can skip this step.

7: Make sure your Avatar Interactions are set to everyone so they can pull the levers.

8: When all the previous steps are done. Press the 'Automatic' button in the webpage to start accepting inputs from other users!

## What effect does the avatar have on the webpage?
In automatic mode, the special avatar can have several effects on the webpage. The Next and Back levers of the avatar only affect playlists. As expected, they will play the Next or Previous video in the playlist, respectively. The Play/Pause lever will pause a playing video and play a paused video. The top of the avatar has a slot for discs. It's compatible with discs held by the "0valler (Minecraft)" avatar. https://vrchat.com/home/avatar/avtr_15149189-454c-45cd-b443-29e3e581b947 . When a disc is inserted, it will play a video corresponding to the disc (normally, the music of that disc).

## Operating the webpage
The HTML is effectively the GUI of the program. The input text box expects for Youtube URLs to be inputted. This will change what video is being played. It accepts normal links, shortened links, and even playlist links. However, it won't work with YouTube Music links. Due to embed limitations, certain videos aren't compatible. Blame YouTube for this obstacle.

The webpage has 2 modes: Manual and Automatic. Manual is for when you don't want the Levers/Discs to have an effect on the video. This is intended for if you want to manually play videos on the youtube site that don't work on the webpage. Turning off Avatar Interactions basically does the same thing. Automatic is for when you want Levers/Discs to have an effect.

Remember: Even in Automatic mode, you can still interact with the embed video itself for direct control. Really helpful if you want to get to a particular video in a playlist.

## What's with the chatbox?
The program utilizes VRChat's chatbox to get information across about the video. This includes the video's title and how far through the video is through playing. This information updates every 2 seconds. This also lets players know when the jukebox is in Manual mode. 
