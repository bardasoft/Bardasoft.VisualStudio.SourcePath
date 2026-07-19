# SourcePath

![SourcePath Logo](https://raw.githubusercontent.com/bardasoft/Bardasoft.VisualStudio.SourcePath/master/src/Bardasoft.VisualStudio.SourcePath/Resources/Icon128.png)

**Free and open-source extension for Visual Studio.**  
Shows the full source file path of the active document in a discreet footer at the bottom of the editor.

---

## Description

**SourcePath** helps you quickly identify the exact file you are editing in Visual Studio.

It is especially useful when working with large solutions, multiple projects, deep folder structures, generated files, or repeated file names such as `User.cs`, `Program.cs`, `Settings.cs`, `App.xaml`, or `UserService.cs`.

The extension adds a clean and discreet footer at the bottom of the editor and displays the full path of the active file.

---

## Features

- Shows the full path of the active file inside the editor.
- Adds a clean and discreet visual footer.
- Helps distinguish files with equal or similar names.
- Allows selecting and copying text directly from the footer.
- Allows copying the full file path.
- Allows copying only the file name.
- Allows copying the containing folder path.
- Allows opening the file location in Windows Explorer.
- Includes compact footer action buttons.
- Includes configurable display options.
- Supports light and dark Visual Studio themes.
- Free and open source.

---

## Preview

![SourcePath preview](https://raw.githubusercontent.com/bardasoft/Bardasoft.VisualStudio.SourcePath/master/src/Bardasoft.VisualStudio.SourcePath/Resources/marketplace-preview.png)

---

## Usage Example

![SourcePath footer example](https://raw.githubusercontent.com/bardasoft/Bardasoft.VisualStudio.SourcePath/master/src/Bardasoft.VisualStudio.SourcePath/Resources/footer-example.png)

---

## Change History

### Version 2.1.2

- Added localized UI text for tooltips, context menus, options page labels, display mode values, and diagnostic messages.
- Added support for English, Spanish, Italian, Portuguese, French, Russian, Chinese, Hindi, Japanese, and German.
- Added English fallback for unsupported Visual Studio UI languages.
- Added automated tests for localized text coverage and fallback behavior.
- Updated README documentation for the new localized UI release.

### Version 2.1.0

- Renamed the visible extension name to **SourcePath** for better marketplace visibility.
- Added a Visual Studio options page.
- Added configurable font family, font size, footer height, and horizontal padding.
- Added display modes: full path, file name, and solution-relative path.
- Added selectable footer text.
- Added compact footer action buttons for copying path, copying file name, copying folder path, and opening the file location.
- Improved clipboard and Windows Explorer actions so failures are handled safely.
- Improved button accessibility metadata.
- Added automated tests for path formatting behavior.
- Improved VSIX packaging to include the generated `.pkgdef` package asset.

### Initial Version

- Added the first editor footer that displays the full path of the active file.
- Added support for Visual Studio light and dark themes.
- Added right-click actions to copy file information.
- Added `Ctrl + Click` support to open the file location in Windows Explorer.
- Published as a free and open-source extension under the MIT License.

---

## Donations

This project is free and open source.  
If **SourcePath** is useful to you, you can support its development with an optional donation.

[![Donate with PayPal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate/?hosted_button_id=EM5DZL2RPA8SL)

---

## Author

**Carlos Bardales Castaneda**  
Guatemala, Central America  
[BardaSoft@gmail.com](mailto:Bardasoft@gmail.com)

Developed by **Carlos Bardales Castaneda** as part of the **Bardasoft** project.

This extension was created to improve the Visual Studio development experience, especially when working with large .NET solutions, multiple projects, generated files, and repeated file names.

---

## License

This project is distributed under the **MIT License**.

See the [LICENSE](https://github.com/bardasoft/Bardasoft.VisualStudio.SourcePath/blob/master/LICENSE) file for more information.
