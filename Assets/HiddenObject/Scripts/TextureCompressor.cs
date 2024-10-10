using UnityEngine;
using System.Collections;

//Comressed texture must me *.bytes png/jpg file

public class TextureCompressor {

	public static string basicPath = @"CompressedTextures\";

    public static Texture Load (string path) {

        Texture2D result = new Texture2D (30, 30);
        byte [] textureFileBytes;

        textureFileBytes = (Resources.Load (basicPath + path) as TextAsset).bytes;

        result.LoadImage (textureFileBytes);
        return result;
    }

    public TextureCompressor (string _basicPath = @"CompressedTextures\") {

        basicPath = _basicPath;
    }
}
