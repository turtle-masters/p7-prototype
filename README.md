# p7-prototype

NOTE: Some of the unit tests make use of an [external c# library](https://drive.google.com/file/d/1dNN436832phPiDvM_Iat0k2AhkWbgTdm/view?usp=sharing), which you must download and put into the `Assets` folder for the project to compile.

## testing the game

The game has a DebugPlayer, which you can trigger when running the game in the Unity editor and pressing one of the WASD movement keys. When triggered, you can move around the level with said keys and look around with the mouse. To interact with object, hold fown F and point on the screen where you want to simulate a VR grabbing action (there is a range limit to this). When playing through the game in this mode, Tasks related to moving objects around the scene are skipped.

IMPORTANT: You <b>must</b> have the `Room` scene open when launching the game for the Levels to work.
