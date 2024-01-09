# AngryBirds

# Introduction:
This assignment is part of my job application for a Unity Developer position. I was assigned to build a Unity game replicating Angry Birds.

# Bootstrapping:
To demonstrate my ability to read and understand other's people code, I bootstrapped from the following project
[Angry Birds by ggghostmaker](https://github.com/ggghostmaker/angrybirds). He also published a [video](https://www.youtube.com/watch?v=r7UDs5kLoB4) tutorial where he live-coding the game, but for the sake of the assignment, I did not watched the video and integrated the project straight to a clean Unity project, covering the various required (and optional) features on top of it, while refactoring the code in the process.

# Flow:
Player is loading into a scene -> The player aims a bird towards the pigs -> If the pigs are being hitted they can potentially drop from their towers. Once they do they are being disposed -> Once the player killed all pigs, he gets an option to proceed to the next level or restart the current level. 

# UI:
1. Enables the player to mute/unmute the volume this setting is being saved to player prefs for consistancy between runs.
2. The player can quit the game at all times
3. The player can restart/move to the next level at all times.

# My take:
On top of the project I bootstrapped from, I did the follwoing:

1. Refactoring.
2. Integrating Reactive Code using UniRx.
3. Added an aiming line.
4. Created an Objects Pool for the Points.
5. Added a second level.
6. Added UI.
7. Integrated IDisposable into the Pig and Bird entities, for enhance performance.
8. Used Dirty Flag.
9. Doubled down on Singleton DP.

    

