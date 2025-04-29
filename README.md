# VR Medical Camp Simulation - Kevin Wu and Alex Atienza

This application simulates a **virtual medical camp environment**, allowing users to interact with and reposition various objects in 3D space using VR controllers.

Built for **Meta Quest 2**, this immersive experience supports object selection, manipulation (rotation/scale), and movement (teleportation and joystick travel).

## Features
- **Object Selection**  
  - **Right Hand (Raycast)** or **Left Hand (Virtual Hand)**
  - Press the **Grip Button once** to select an object

- **Manipulation**  
  - **Rotate**: With an object selected, press the **Trigger Button** on the opposite hand and rotate
  - **Scale**: Hold the **Trigger Buttons on both hands**, move hands closer/farther to scale

- **Travel / Movement**
  - **Joystick Movement**: Reorient and move around freely
  - **Teleportation**:  
    1. Point the ray at your **target floor position**  
    2. Press **Grip Button once** to set destination  
    3. Press again at a second point to set orientation

---

## Controls Summary

| Action               | Input                                                                 |
|----------------------|-----------------------------------------------------------------------|
| Select Object        | Grip Button (Right/Left hand)                                         |
| Rotate Object        | Trigger on opposite hand while selecting                              |
| Scale Object         | Trigger on both hands and adjust distance between them                |
| Joystick Movement    | Move/reorient using joystick                                          |
| Teleport             | Raycast → Grip once to set position → Grip again to set orientation  |

---

## Technologies Used

- Unity
- Meta Quest 2 SDK / Oculus Integration
- C#
- XR interaction toolkit

---

## Demo Video:
[![Video Title](https://img.youtube.com/vi/zJduxtnzq_0/0.jpg)](https://www.youtube.com/watch?v=zJduxtnzq_0)
