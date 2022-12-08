# Label Timer Countdowns

Ever been frustrated that the customer countdowns were inexact? Me too, so I made this mod.

![Label display](https://i.imgur.com/4WdcOP6.png)  
![Full screen with label](https://i.imgur.com/41SGPLJ.png)

## Installation
1. [Download BepInEx](https://github.com/BepInEx/BepInEx/releases)
2. [Install BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)
3. Download this mod
4. Extract to your PlateUp! game directory


## Usage
The plugin automatically loads when you start the game, and it automatically updates with new timers as customers come in.

The window is draggable, and clicking the little button in the top left will minimize it.

### In Multiplayer
Currently this *kind of* works in multiplayer.  
Only the **host** will see the correct values. All other players will see nonsense numbers.

## Known Issues
Check the [Issues tab](https://github.com/Elec0/plateup-label-timer-countdown/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc) here on Github to see the currently known bugs and feature requests.

## Dev environment setup
I use Visual Studio 2022.

1. Clone this repo
2. Create a new folder, `lib/`
3. Copy the following DLLs from the PlateUp! install folder to the lib folder:
   - Kitchen.Common.dll
   - Kitchen.FranchiseBuilderMode.dll
   - Kitchen.FranchiseMode.dll
   - Kitchen.GameData.dll
   - Kitchen.Layouts.dll
   - Kitchen.Networking.dll
   - Kitchen.Persistence.dll
   - Kitchen.PostgameMode.dll
   - Kitchen.ResearchMode.dll
   - Kitchen.RestaurantMode.dll
   - Kitchen.TutorialMode.dll
   - KitchenMode.dll
   - Unity.Entities.dll
   - Unity.TextMeshPro.dll
   - UnityEngine.dll
   - UnityEngine.CoreModule.dll
   - UnityEngine.IMGUIModule.dll
   - UnityEngine.UI.dll
4. Open the project in Visual Studio, NuGet should download some plugins, it should then work