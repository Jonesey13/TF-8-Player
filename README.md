# 8 Player TowerFall Mod 

A Towerfall mod that uses a modified version of the [Bartizan](https://github.com/Kha/Bartizan) modding framework and [Mono.Cecil](https://github.com/jbevain/cecil). Original copyright belongs to Matt Thorson.

![](https://github.com/Jonesey13/TF-8-Player/blob/master/Demo.gif)

# Features
* 8 player versus mode
* 3v3 in team deathmatch

# Installing the mod (for non-developers on Windows)
IMPORTANT: Please backup your entire TowerFall folder before following the steps below:
* Copy the contents of the Pre-Built folder (or zip file if downloading the release version) into the root of the Towerfall folder, replacing all files when requested. 
* If you do not have the DarkWorld Expansion, delete the DarkWorldContent folder.
* If you are using the Humble version, replace Content/Atlas/menuAtlas.xml with Content/Atlas/menuAtlas-Humble-Version.xml.
* Run Patcher.exe in your TowerFall folder. There should be no errors and the TowerFall folder should now have a new file called TowerFall8Player.exe
* (Steam Version Only) Rename TowerFall.exe to TowerFallOrig.exe and TowerFall8Player.exe to TowerFall.exe
* Enjoy!

# Building the mod (for developers, using Visual Studio)
The solution has two build configurations: MakeReferenceImage and Create8PlayerMod. MakeReferenceImage is used to setup the solution and should only need to be run once (following the steps below). Create8PlayerMod should only be run when the references for the Mod project are setup correctly.

First we need to get MakeReferenceImage working:
* First add Mono.Cecil as a reference for Patcher using "Manage NuGet Packages".
* Copy Towerfall.exe into the root of the solution
* Run the build configuration named MakeReferenceImage, this should build TowerFallReference.exe

Now to setup the references for the Mod project:
* Add references to the XNA framework (or FNA if you prefer) for the Microsoft.Xna.Framework, Microsoft.Xna.Framework.Game, Microsoft.Xna.Framework.Graphics and Microsoft.Xna.Framework.Net modules. The dll's can typically be found under C:\Program Files\Microsoft XNA\XNA Game Studio\v4.0\References\
* Copy over SharpDX.dll and SharpDX.DirectInput.dll from the TowerFall folder and adds them as references
* Finally add a reference to TowerFallReference.exe 

If everything is setup correctly, you should be able to just run Create8PlayerMod and this will build both Mod.dll and Patcher.exe

# Known Issues / Limitations
* You cannot have 4 players on a team. Don't even try! The game will crash. You'll have to patch your own levels to make this work!
* Some of the patched starting positions are a little broken but I've tried my hardest to space people out. It's the best I could do!
* The code was cobbled together in a short period of time just to get things working. You're probably going to be rather confused by it! For the brave who want to understand what on earth the code is doing, I recommend decompiling TowerFall.exe using something like .Net Reflector, Telerik's JustDecompile or ILSpy. You should also check out the Bartizan github page as well. Also feel free to contact me with any questions and I'll try my best to answer them.

# Thanks
* Matt Thorson and everybody else who contributed to making such an awesome game!
* Everybody who worked on the Bartizan project which allowed the mod to be open-sourceable
* All those who worked on Mono.Cecil
* Alec Gibson for suggesting that someone should make this mod in the first place
* Everybody at Softwire and RCVGS who tested the earlier versions of the mod
