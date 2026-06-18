using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateWhiteSquareTexture 
{
    // This adds a button to the top menu bar in Unity!
    [MenuItem("Tools/Create UI Square Sprite")]
    public static void CreateSprite()
    {
        // 1. Create a tiny 4x4 purely white texture in memory
        Texture2D tex = new Texture2D(4, 4);
        Color[] pixels = new Color[16];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();

        // 2. Save it as a PNG to our new Textures folder
        byte[] pngData = tex.EncodeToPNG();
        string path = "Assets/Textures/UISquare.png";
        File.WriteAllBytes(path, pngData);

        // 3. Tell Unity to refresh its files so it sees the new PNG
        AssetDatabase.Refresh();

        // 4. Force Unity to import this PNG specifically as a "Sprite (2D and UI)"
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            // Best settings for sharp UI edges
            importer.filterMode = FilterMode.Bilinear; 
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        Debug.Log("Successfully created a crisp white UI sprite at " + path);
    }
}