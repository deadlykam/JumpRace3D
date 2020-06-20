# JumpRace3D

### Introduction of the game:
This is a mobile game where the player races with other AI by jump on floating stages.

## Gameplay:
The main objective is to get to the final floating stage which is green in colour and looks different from other stages. On the way there will be other AI racers that will try to reach the final stage before the player. The AI racers can be killed by colliding with them or by breaking some stages. The player dies when falling into the water or colliding with the big floating windmill fan obstacle. There is a booster pick up which helps the player to travel further. Once the player reachs the final stage a new random stage will be generated.

## Table of Contents:
- [Prerequisites](#prerequisites)
- [Controls](#controls)
- [Stage](#stage)
- [Versioning](#versioning)
- [Authors](#authors)

## Prerequisites
#### Unity Game Engine
Unity version of the build is **2019.3.15f1**. Install Unity game engine from version **2019.3.15f1** or above. Install [Unity Hub (recommended)](https://store.unity.com/download?ref=personal) and add Unity version **2019.3.15f1**. Once installed then open the latest stable build.
***
## Controls
1. Unity Editor: Press and hold the left mouse button to move the player forward. Press and hold the left mouse button and move the mouse in x-axis to rotate the player.

2. Android: Press and hold the screen to move the player forward. Swipe left or right to rotate the player left or right respectively.
***
## Stage
The **[StageGenerator](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Obstacles/StageGenerator.cs)** is responsible for generating a stage. Every stage is randomly generated so no two stages are alike. Everytime before a stage starts it generates a new random stage. Below are previews showing how the stages are being generated.

![](https://imgur.com/COkOkbu.gif)

![](https://imgur.com/XqJwFUF.gif)
***
## UML
Sharing some UML diagram to show the hierarchy of some classes and system and a brief description.

1. Character UML: This structure shows how the game character's functionalities are handled.
<img src="https://imgur.com/4byzjvQ.png" alt="Character UML">

   - **[BasicCharacter](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Characters/BasicCharacter.cs):** Handles the movement, rotation, jumping, falling and auto rotation functions of a character.
   - **[BasicAnimation](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Characters/BasicAnimation.cs):** Handles all the animation functions of a character. Also enables/disables the ragdool.
   - **[Player](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Characters/Player/Player.cs):** Makes the player character move forward or rotate by taking input from the user.
   - **[Enemy](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Characters/NPC/Enemy.cs):** Makes the enemy character move towards the next stage and also rotates the enemy to face the next stage. Colliding with the player kills the enemy character.

2. Platform UML: This structure shows how the game's platform system funtions.
<img src="https://imgur.com/kkjLamS.png" alt="Platform UML">

   - **[BasicStage](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Obstacles/BasicStage.cs):** This class sets the type of particle effect to show for the platform when the methods **void StageAction()** or **void StageAction(bool)** are called.
   - **[BouncyStageLong](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Obstacles/BouncyStageLong.cs):** This class starts the hide countdown process of the platform. Once the countdown process is done the platform is hidden and disabled from the game world. The method **void StageAction()** starts the countdown process and calls the parent's virtual method as well.
   - **[BouncyStage](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Obstacles/BouncyStage.cs):** This class handles the animation of the platform. It also shows/hides the booster pickup of a platform and stores the 3D text when the platform is activated. The method **void StageAction()** plays the animation of the platform if the property **bool \_hasAnimation** is true and calls the parent's virtual method as well.
   - **[BouncyStageMove](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Obstacles/BouncyStageMove.cs):** This class moves the platform sideways within a limit. The method **void StageAction()** calls the parent's virtual method.
   - **[BouncyStageBreakable](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/Obstacles/BouncyStageBreakable.cs):** This class starts the breaking up process of a platform. The method **void StageAction()** instantly starts the breaking process of the platform and calls the parent's virtual method. The method **void StageAction(bool)** checks if the platform's break up process should be activated or not depending on the **bool** value and also calls the parent's virtual method.
   
3. Menu UML: This structure shows how the game's menu system functions.
<img src="https://imgur.com/8adDLE5.png" alt="Platform UML">

   - **[BasicUI](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/UIs/BasicUI.cs):** This class shows the menu by calling the method **void ShowUI()** and hides the menu by calling the method **void HideUI()**. It shows/hides menus by activating/deactivating the Canvases stored in the field **Canvas[] \_canvases**.
   - **[EndScreenUI](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/UIs/EndScreenUI.cs):** This class shows the end menu which is the final result of the race. The menu is shown by calling the parent's virtual method **void ShowUI()** and hidden by calling the method **void HideUI()** which resets the end menu and calls parent's virtual method.
   - **[MoveUI](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/UIs/MoveUI.cs):** This class slides in/out a menu. The menu is slid in when the method **void ShowUI()** is called, this also calls the parent's virtual method. The menu is hidden when the method **void HideUI()** is called but the parent's virtual method is not called from here. This is done so that once the slide out effect is done only then will the menu be hidden.
   - **[PlayerUI](https://github.com/deadlykam/JumpRace3D/blob/features-readme/Assets/JumpRace3D/Scripts/UIs/PlayerUI.cs):** This class shows the current race position of the player and other racers. It also shows how far the player is in the race. The parent's virtual method **void ShowUI()** shows the player menu and the parent's virtual method **void HideUI()** hides the player menu.
***
## Versioning
The project uses [Semantic Versioning](https://semver.org/). Available versions can be seen in [tags on this repository](https://github.com/deadlykam/LastTry/releases).
***
## Authors
- Syed Shaiyan Kamran Waliullah \- [DeadlyKam](https://github.com/deadlykam)
