# Mumbo Jumbo Adventure — Game Design Final Project

**Mumbo Jumbo Adventure** is a small **local multiplayer puzzle-adventure game** created as a final project for the **Game Design Course**.  
This is **not a commercial release** and currently includes **only 2 playable levels**, with **no main menu** or additional systems yet.

The goal is simple: **work together, solve puzzles, avoid getting frustrated, and reach the trophy together!**

👉 **Build Download (GAME):**  
https://drive.google.com/drive/folders/1GNmPBEo7xdbjhXQQ3KRe98cAm3PTydH6?usp=sharing  
*(Download the entire folder and run **MumboJumboAdventure.exe** to play!)*

---

## 🎮 Game Description

Mumbo Jumbo Adventure is a **2-player cooperative game** played on the **same computer**.   
Both players must collaborate, platform, think, argue, and maybe scream a little while trying to reach the trophy at the end of each level.

The game focuses on:

- Team-based puzzle solving  
- Energy & health resource management  
- Platforming challenges  
- Co-op mechanics that can get chaotic (and fun!)  

Only by working together will you survive.

---

## ⚙️ Core Mechanics

- Each player starts with **100 Energy**.  
- Both players **share 6 Health**.  
- Use boxes, levers, teamwork, and problem-solving to progress.  
- Some obstacles require **precise timing and cooperation**.  
- If **both players run out of Energy**, you **die** and respawn at the **start of the level**.  
- **Health carries over** between levels.  
- **Energy resets** to 100 when entering a new level.

---

### 🔋 Energy Costs

| Action                         | Cost                 |
|--------------------------------|----------------------|
| Jump                           | 3 Energy             |
| Interact (normal lever)        | 10 Energy            |
| Interact (2-keys lever)        | 20 Energy + both keys|

---

### 🍉 Energy Pickups

| Item             | Energy Gain |
|------------------|-------------|
| Small Watermelon | +25 Energy  |
| Large Watermelon | +50 Energy  |

---

## 🔖 Checkpoints

- **Starter Point**  
  - Restores Energy **up to 100** (cannot exceed).

- **Checkpoint Flag**  
  - Sets respawn point with **full Health**,  
    but **does NOT restore Energy**.

---

## 🕹️ Controls

### **Player 1**
- **A / D** – Move left / right  
- **W** – Jump  
- **E** – Interact  
  - If near another Player → **transfer 20 Energy** (cannot exceed 100)

### **Player 2**
- **← / →** – Move left / right  
- **↑** – Jump  
- **Right Shift** – Interact  
  - If near another Player → **transfer 20 Energy** (cannot exceed 100)
---

## 🧠 Tricks & Advanced Moves

- Players can **stand on top of each other**.  
- A player can **drop a box on the other player’s head** to bounce it into place.  
- **Team Double Jump**:
  1. Top player jumps  
  2. Bottom player jumps  
  3. Top player jumps again instantly  

This allows reaching higher platforms through teamwork.

---

## 📚 Credits & Notes

This project is created **for educational purposes only**.

- All **graphics / pixel art assets** belong to the creators of **Pixel Adventure**.  
- Initial **basic character controller scripts** were obtained from their respective owners.  
  - Source code: https://github.com/btuhany/PixelAdventure-Unity2D  

- **Level designs** for the scenes **MyLevel1** and **MyLevel2** (used in this game) were created by me.  
- Other scenes such as *Level1*, *Level2*, and *Tutorial* are from the downloaded source project and **not used** in the game.  
- All gameplay ideas, concepts, and design decisions were made by our **Game Design Project team**.

---

Thank you for trying the game — hope you enjoy it, even with only 2 levels! 🎉
