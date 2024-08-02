using UnityEngine;

public class TextureUtils
{
    public static Texture2D Resize(Texture2D source, int newX, int newY)
    {
        RenderTexture cachedRt = RenderTexture.active;

        RenderTexture rt = new RenderTexture(newX, newY, 24);
        Texture2D result = new Texture2D(newX, newY, source.format, source.mipmapCount > 0);
        rt.filterMode = source.filterMode;
        result.wrapMode = source.wrapMode;

        Graphics.Blit(source, rt);
        result.ReadPixels(Rect.MinMaxRect(0f, 0f, newX, newY), 0, 0);  // Reads from current RT
        result.Apply();

        RenderTexture.active = cachedRt;
        rt.Release();

        return result;
    }
}