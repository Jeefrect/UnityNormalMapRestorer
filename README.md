# UnityNormalMapRestorer
Restoring normal maps after optimization in Unity.
Unity uses texture optimization by moving channels. This small program restores the normal map.

Drag&Drop textures or a folder onto the exe file. The program will find textures with "nm," "norm," or "normal" in their names, fix them, and create a new folder replicating the original folder structure with the corrected textures.
![Esesy](https://github.com/user-attachments/assets/e86e0c19-1390-4754-8496-8fde39f112d5)

Used: [SixLabors](https://github.com/SixLabors/ImageSharp)
