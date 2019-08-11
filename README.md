# ServiceModePlugin
This project is a plugin for SimFeedback [https://github.com/SimFeedback/SimFeedback-AC-Servo](https://github.com/SimFeedback/SimFeedback-AC-Servo) which lets you control the SFX 100 with the press of a button.
Available controls are:
- Min/max height
- Max left/max right
- Max front/max back

You can select a button for every function on your controller in the GUI.
Pressing the button once, will move the SFX 100 to the max position. Pressing it again will center the Rig to default position on that axis.

## Disclaimer:
Take caution while using this plugin:
- Do not use this plugin at max speed and/or acceleration.
- If you are planning to work under your rig, always put a support under the rig to prevent it from falling down.

## Installation Instructions:

### Step 1:
Download (or clone) this project.

### Step 2:
Go into the folder 'Service Mode Plugin Release' and copy the 3 folders into the root of your SimFeedback installation.

### Step 3:
Run remove_blocking.bat in the SimFeedback root folder.

### Step 4:
Launch SimFeedback and activate the plugin's profile.

### Step 5:
Hit the start button. Go to the popup window (use ALT+TAB), select your controller from the dropdown menu and configure the buttons.

## Videos": 
[https://youtu.be/IU6mgVw7TTc](https://youtu.be/IU6mgVw7TTc)

[https://youtu.be/o5k5Xlbr3Z4](https://youtu.be/o5k5Xlbr3Z4)


## Building Guide:
Only needed if you would like to make changes to the code.
### Step 1:
Clone (or download) SimFeedbacks github repository and open the .sln file in Visual Studio.

### Step 2:
Create a new project in the solution (by right clicking on the solution in the tab 'Solution Explorer'), with the template: 'C# .NET Framework'. Don't choose '.NET Core' or 'VB'.

### Step 3:
Create a folder in the project called "mc". Then put the source code files from this repository in that folder.

### Step 4:
Add three NuGet packages: SharpDX, SharpDX.DirectInput and WindowsInput.

### Step 5:
Build your project.

### Step 6:
Follow the installation instructions. Use the DLL built by you instead of the DLL in 'Service Mode Plugin Release/provider'
