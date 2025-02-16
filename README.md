# Intro
**EasyTask Outlook Add-in** extends the standard task functionality in Outlook (2007–2021) by allowing you to specify due dates and categories directly within the task name. This streamlines task management by enabling natural language input to set due dates.

>**Note: The add-in is not compatible with the latest web-based Outlook, where tasks have been replaced by integration with Microsoft To Do.** 

If you're interested in the code, feel free to explore it. Check out the `OutlookControlsExtensions` class to see how to extend the standard Outlook edit box with additional features like a filtered drop-down. To enable the drop-down functionality, make sure to activate the corresponding view in Outlook and restart the application.

![Untitled](https://github.com/user-attachments/assets/19bbaf48-3aa2-4174-bc92-a5fb608284c4)

# Demo
![Demo gif](./EasyTaskDemo.gif)

# Source code
## Dependencies
- [WiX Toolkit v3](https://docs.firegiant.com/wix/wix3/): Used for building the installer.
- [WiX toolset extension for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=WixToolset.WixToolsetVisualStudio2022Extension): Recommended to simplify the compilation process.
## How to compile
1. Open the solution file: `EasyTask/EasyTaskAddin.sln`
2. Restore NuGet packages.
3. Compile the solution
4. Run the installer or debug the add-in (MS Outlook 2021 or earlier is required).
