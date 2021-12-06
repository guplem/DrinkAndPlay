Original Post: http://answers.unity.com/answers/1687958/view.html

0a. Download and install Texture Packer (https://www.codeandweb.com/texturepacker/download)
0b. Import Texture Packer into the project (https://assetstore.unity.com/packages/tools/sprite-management/texturepacker-importer-16641)

1. Put all .png files into TexturePacker (drag and drop to the software's GUI)

2. Set the "output Files" > "Framework" to -> JSON (Array)

3. If prompted for phaser framework, go for the option "keep JSON Array"

4. Click "Publish sprite sheet", it will export both the sprite atlas (.png) and the configuration file (.json).

5. Add both in your assets folder in Unity.

6. In Unity go to Window -> TextMeshPro -> Sprite Importer

7. If TexturePackerImporter was added successfully into your project, the Import Format should be Texture Packer (keep this). Select the .json file for Sprite Data Source and the .png file for the sprite atlas. After choosing these out of your assets folder, click Create Sprite Asset.

8. Keep the sprite atlas (.png) file selected (this is important, otherwise Unity throws a warning that you haven't chosen any files) and go to Assets -> Create -> TextMeshPro -> Sprite Asset.

9. Now this is important -> If you select your Sprite Asset, which you'd created in the previous step, you have to make sure, that the Unicode Value in Sprite Character Table (in the Inspector) is filled (e.g.: the unicode value for 1f4a9.png should be 0x1F4A9). Usually they are not filled automatically, which takes a bit time to enter them one by one.

10. In the TextMeshPro (text component in the object) you must set the created TMP_Sprite asset in the "Sprite Asset" field in the "Extra Settings" section so it is used by that component.

// Updating the "Face Info" of the "TMP_Sprite asset" might be necessary to match Font's size/properties. It can be done in real time