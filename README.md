# panopticon-prototype

Unity 6 gameplay prototype that simulates a multiple camera placement setup.

Features:
* 1st person controls (Unity's Template used)
* Cameras can be placed on the ground (first a ghost is spawned, and then it is finalized or cancelled)
* HUD has a preview during ghost placement
* Camera can be interacted with (primary) to adjust pitch & yaw. It also has preview during this
* Cameras have an interaction interface. They can be turned on&off from this interface
* Cameras have batteries, which drains at a certain rate. If there is no battery installed or it is depleted, camera turns off and cannot be turned on
* Cameras that are off are not displayed in the monitors (This should be updated to be a black screen instead of a skip so the player knows it's out of battery)
* There can be multiple monitors, each displaying cameras separately

What to do next:
* Multiple monitors with separate controls
* Player should be more than a capsule
* Camera upgrades & maintenance