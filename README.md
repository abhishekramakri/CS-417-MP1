# CS 417 MP1: Space Station Escape Room

**Contributors:** Abhishek Ramakrishnan, Aniketh

## Overview

A VR Escape Room set in a damaged space station. The player must restore three critical systems before oxygen runs out. Find the correct clue objects scattered around the station, place them into their matching sockets, and escape before the O2 timer hits zero.

**Theme:** Sci-Fi Space Station Airlock

**Scene:** `Assets/Scenes/Abhi_work_scene.unity`

## How to Play

1. Explore the station and find the three clue objects
2. Bring each clue to its matching socket:
   - **PowerCell** (GlassTank) → **ReactorSocket** (removes laser barrier)
   - **KeyCard** (Switch) → **DoorTerminalSocket** (removes sci-fi door)
   - **FuelCanister** (Metal Barrel) → **FuelPortSocket** (triggers win)
3. Restore all 3 systems before the O2 timer runs out
4. Watch out for red herrings — not every grabbable object is a real key

## Controls (Quest 3)

- **Left Thumbstick:** Move
- **Right Thumbstick:** Turn
- **Grip Button (middle finger):** Grab / Release objects
- Move grabbed objects near a socket to snap them in

---

## Implemented Features

### Required Features

**Grab Affordance Clues:** 3 clue objects (PowerCell, KeyCard, FuelCanister) each have XR Grab Interactable components. The player has Direct Interactors on both hands.

**Collider Environs:** Floor, walls, tables, and surfaces all have colliders. Physics objects land on surfaces without clipping through.

**Physics Objects:** All 3 clues have Rigidbodies and fall/tumble with gravity. Additionally, 6 tossable boxes in the medbay area have Rigidbodies and can be pushed around. The 3 red herring objects also have Rigidbodies.

**Unlocking Keys:** 3 socket interactors (ReactorSocket, DoorTerminalSocket, FuelPortSocket) each run a GateSocket script when the matching clue is placed inside. Sockets use XRI hover/select filters to only accept the correct clue.

**Escaping the Room:** All 3 gate sockets must be triggered before the win condition fires. The walkthrough video demonstrates all three clues placed in their corresponding sockets before the win triggers.

### Optional Features

**Secrets:** Placing the PowerCell in the ReactorSocket removes the laser barrier, revealing the path forward. Placing the KeyCard removes the sci-fi door.

**Doors and Keys:** The KeyCard socket triggers removal of the Future_Door_Final, opening a new zone for the player to navigate to.

**UI Scoreboard:** A world-space Canvas displays "Systems Restored: X / 3" and updates in real-time as gates are unlocked.

**Loss Timer:** An O2 countdown timer is displayed on screen ("O2 Remaining: MM:SS"). When it reaches 0, the game displays "O2 DEPLETED - GAME OVER".

**Restart Option:** A "Restart" UI button is available that reloads the scene from the beginning.

**Win Celebration:** Upon winning: (1) "ESCAPE SUCCESSFUL" WinCanvas appears, (2) a green WinLight point light turns on, (3) after a 2-second delay the player teleports to a bird's-eye ViewingPlane at Y=25 to view the entire station from above.

**Red Herrings:** 3 extra grabbable objects with Rigidbodies that are not clues: Computer - Red Herring, ScifiBox - Red Herring, Metal Barrel - Red Herring. The socket filter system rejects them automatically.

**Themed Setting:** Imported asset packs used for the environment: Low Poly Sci-Fi Station Pack (station walls, doors, furniture, beds, laptops, screens), Future Pad Door (sci-fi door), SpaceSkies Free (space skybox).

**Themed Props:** All 3 clue objects use imported sci-fi models from the SciFiProps asset pack: PowerCell = GlassTank model, KeyCard = Switch model, FuelCanister = Metal Barrel 2 model. Each has custom textures and materials applied.

---

## Project Structure

```
Assets/
  Scenes/
    Abhi_work_scene.unity    <- Main game scene
    Level3.unity             <- Original base scene
  Scripts/
    GateSocket.cs            <- Socket filtering, obstruction removal, win notification
    EscapeRoomManager.cs     <- Timer, scoreboard, win/loss logic, restart
  TeleportWin.cs             <- Teleports player to bird's-eye view on win
  SciFiProps/                <- Sci-fi prop models, materials, textures
  Low Poly Sci-Fi Station Pack/  <- Station environment assets
  Future Pad Door - FREE/    <- Sci-fi door prefab
  SpaceSkies Free/           <- Space skybox
```

## Scripts

### GateSocket.cs
Attached to each socket interactor. Uses XRI hover/select filters to only allow the correct clue object to enter the socket. When the correct clue is placed, it removes obstructions, reveals secrets, and notifies the EscapeRoomManager.

### EscapeRoomManager.cs
Central game manager. Tracks gate unlock progress, runs the O2 countdown timer, handles win/loss conditions, displays UI updates, and triggers the win celebration with teleport.

### TeleportWin.cs
Attached to the XR Origin. On win, teleports the player to position (0, 25, -35) for a bird's-eye view of the station.

## Build Instructions

1. Open the project in Unity 6
2. Open `Assets/Scenes/Abhi_work_scene.unity`
3. File → Build Settings → Android → Add Open Scenes → Build
4. The resulting APK can be sideloaded onto Quest 3
