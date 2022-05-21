# Umqombothi

![In-game screenshot](https://www.dropbox.com/s/hwhdqnfoyongknw/mini2.jpg?dl=0&raw=1)

Watch the [video](https://youtu.be/aGKtCIhe5OU)!

In this paper, we describe an interactive embodied learningbased
simulation in Virtual Reality that was designed to disseminate
the Intangible Cultural Heritage behind “Umqombothi”,
a traditional South African beer, through teaching the
beer-making and chemistry behind it. The simulation was
designed in collaboration with South African researchers
from the University of Johannesburg. The simulation includes
an authentic beer-making environment based on a
South African township and an abstract chemistry environment
called the “microverse”. The simulation had a built in
logging system for triangulation of data, and was evaluated
in an explorative manner using grounded theory methods
for analysing the participant data. The paper serves to propose
a model for a VR simulation for safeguarding ICH. The
simulation proved insufficient for teaching the processes in
detail. Because of this, we present suggestions to improve
the simulation which are based on the data: improving the
balance between visual and audio aids, explaining the chemistry
more thoroughly and using more animated elements,
all of which are grounded in our data.

# Guide

*The following is a guide on how to set up our prototype game "Umqombothi". If you have any questions regarding anything in this document, please reach out to us as soon as possible!*

## Prerequisites

Please start by making sure you have the latest graphics drivers for your system. How to check and update this depends on the manufacturer of your computer:

- For Nvidia, refer to [this site](https://www.nvidia.com/en-in/drivers/nvidia-update/).
- For AMD, refer to [this site](https://www.amd.com/en/support).

Next, please download our game from our [releases page](https://github.com/turtle-masters/p7-prototype/releases) on the GitHub repo (you specifically need to download and extract the *“build\_1\_0\_1.zip”* file).

## Installing Oculus Link

To be able to run the prototype, you will first need to install [Oculus Link](https://www.oculus.com/setup/), enabling the connection between your computer and the virtual reality headset.

Note that you must download and install the version of the software that is intended for your specific headset. You can find software for the Oculus Quest 2 and Oculus Rift S [here](https://www.oculus.com/setup/).

For the software to work, you must sign in through a Facebook account. We cannot provide you with one, but you should be able to use any existing account.

Once you’re signed in, you just need to have the software open and plug in the headset with the Oculus Link Cable.

## Installing SteamVR

Our solution is built on [SteamVR](https://www.steamvr.com/en/), a virtual reality framework built on the OpenVR standard, made to support a very broad set of virtual reality equipment.

To get SteamVR, you must first download and install Steam, [this site](https://www.steamvr.com/en/) will guide you through the process. If you do not already have Steam installed, the site will redirect you to the [steam download page](https://store.steampowered.com/about/).

When you have Steam installed, you must create an account or log in to an existing one. We will not be able to provide you with an account, but it is free to create one.

Once you’re signed in to your Steam account on the client, please download SteamVR through [this link](https://www.steamvr.com/en/).

## Running the prototype for the first time

Once you have SteamVR installed, you...

1. Make sure your PC is plugged in while running the simulation for best performance.
1. start up the Oculus Rift S (or the VR headset you are using),
1. plug it into your computer with the Oculus Link Cable,
1. launch SteamVR,
1. unzip the compressed file “build\_1\_0\_2.zip”,
1. open the uncompressed folder of the same name,
1. double-click on the “Umqombothi” executable.

Our prototype should now launch through SteamVR and play through the Rift S. Please verify that this is the case, then exit SteamVR again.

# Testing the game in the Unity Editor
Some of the unit tests make use of an [external c# library](https://drive.google.com/file/d/1dNN436832phPiDvM_Iat0k2AhkWbgTdm/view?usp=sharing), which you must download and put into the `Assets` folder for the project to compile.

The game has a DebugPlayer, which you can trigger when running the game in the Unity editor and pressing one of the WASD movement keys. When triggered, you can move around the level with said keys and look around with the mouse. To interact with object, hold fown F and point on the screen where you want to simulate a VR grabbing action (there is a range limit to this). When playing through the game in this mode, Tasks related to moving objects around the scene are skipped.
