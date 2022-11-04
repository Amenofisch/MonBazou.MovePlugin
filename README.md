# MonBazou.MovePlugin
This tool allows you to quickly and automatically copy your plugins while developing to the BepInEx/plugins folder

## Setup
 1. Copy the binary "MovePlugin.exe" to your project's root folder, this is where the .sln file is located
 2. Compile your project (make sure it's either selected as a "Release" or as a "Debug" when compiling)
 3. Run the binary "MovePlugin.exe" once and enter the plugin's dll name with the extension
 4. Make any changes to your project
 5. After compiling just launch the binary once and it will automatically copy the dll to the plugins folder
 6. Enjoy :)
 
 ## Workflow
  1. Create new branch from main
  2. Commit and Push your changes to your new branch
  3. Create PR to main
  4. Wait for review from lead dev
  5. If the lead-dev reviewed your PR and accepted it, merge it.
     If it hasn't been accepted, then read the comments and fix your issues
  6. Done :)
  

