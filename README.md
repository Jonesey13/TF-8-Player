# 8 Player TowerFall Mod 

A Towerfall mod that uses a modified version of the [Bartizan](https://github.com/Kha/Bartizan) modding framework and [Mono.Cecil](https://github.com/jbevain/cecil). Original copyright belongs to Mark Thorson.

![](https://github.com/Jonesey13/TF-8-Player/blob/master/Demo.gif)

# Features
* 8 player versus mode
* 3v3 in team deathmatch

# Installing the mod (for non-developers on Windows)
IMPORTANT: Please backup your entire towerfall folder before following the steps below:
* Copy the contents of the Pre-Built folder into the root of the Towerfall folder. This includes replacing the savefile (not necessary if you already have 8 characters unlocked in-game)
* Merge the Content folder of this Repo with the Content folder of Towerfall (this is to install the patched levels and atlas file)
* (Optional, Darkworld only) Do the same for the DarkWorldContent folder
* Run Patcher.exe in your TowerFall folder. There should be no errors and the TowerFall folder should now have a new file called TowerFall8Player.exe
* (Steam Version Only) Rename TowerFall.exe to TowerFallOrig.exe and TowerFall8Player.exe to TowerFall.exe
* Enjoy!

# Building the mod (for developers, using Visual Studio)
The solution has two build configurations: MakeReferenceImage and Create8PlayerMod. MakeReferenceImage is used to setup the solution and should only need to be run once (following the steps below). Create8PlayerMod should only be run when the references for the Mod project are setup correctly.

First we need to get MakeReferenceImage working:
* First add Mono.Cecil as a reference for Patcher using "Manage NuGet Packages". Select version number 9.4.0 of Mono.Cecil (this is very important!)
* Copy Towerfall.exe into the root of the solution
* Run the build configuration named MakeReferenceImage, this should build TowerFallReference.exe

Now to setup the references for the Mod project:
* Add references to the XNA framework (or FNA if you prefer) for the Microsoft.Xna.Framework, Microsoft.Xna.Framework.Game, Microsoft.Xna.Framework.Graphics and Microsoft.Xna.Framework.Net modules. The dll's can typically be found under C:\Program Files\Microsoft XNA\XNA Game Studio\v4.0\References\
* Copy over SharpDX.dll and SharpDX.DirectInput.dll from the TowerFall folder and adds them as references
* Finally add a reference to TowerFallReference.exe 

If everything is setup correctly, you should be able to just run Create8PlayerMod and this will build both Mod.dll and Patcher.exe

# Known Issues / Limitations
* The Amaranth and Cataclysm are not currently patched (Only one level half-works to prevent the game from crashing).
* You cannot have 4 players on a team. Don't even try! The game will crash. You'll have to patch your own levels to make this work!
* The text and awards are too large on the results screen. (this is tricky to fix because of the limitations of the modding framework / Mono.Cecil but I aim to fix it asap)
* Same goes for the big color rectangles that appear when selecting characters
* Some of the patched starting positions are a little broken but I've tried my hardest to space people out. It's the best I could do!
* The code was cobbled together in a short period of time just to get things working. You're probably going to be rather confused by it! For the brave who want to understand what on earth the code is doing, I recommend decompiling TowerFall.exe using something like .Net Reflector, Telerik's JustDecompile or ILSpy. You should also check out the Bartizan github page as well. Also feel free to contact me with any questions and I'll try my best to answer them.

# Thanks
* Matt Thorson and everybody else who contributed to making such an awesome game!
* Everybody who worked on the Bartizan project which allowed the mod to be open-sourceable
* All those who worked on Mono.Cecil
* Alec Gibson for suggesting that someone should make this mod in the first place
* Everybody at Softwire and RCVGS who tested the earlier versions of the mod
