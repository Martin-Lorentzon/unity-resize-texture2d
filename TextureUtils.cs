using UnityEngine;

public class TextureUtils
{
    /// <summary>
    /// Resizes a given Texture2D object to the specified dimensions and returns a new Texture2D object.
    /// </summary>
    /// <param name="source">The source Texture2D object to resize.</param>
    /// <param name="newX">The new width for the texture.</param>
    /// <param name="newY">The new height for the texture.</param>
    /// <returns>A new Texture2D object with the resized dimensions.</returns>
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

    /// <summary>
    /// Resizes a given Texture2D object to the nearest power of two plus one (as required by Terrain component) and returns a new Texture2D object.
    /// </summary>
    /// <param name="source">The source Texture2D object to resize.</param>
    /// <returns>A new Texture2D object with the resized dimensions.</returns>
    public static Texture2D FitTerrain(Texture2D source)
    {
        int oldResolution = source.width;

        int nextPowerOfTwo = 2;
        while (oldResolution >= nextPowerOfTwo + 1) nextPowerOfTwo *= 2;

        int newResolutionLower = nextPowerOfTwo / 2 + 1;
        int newResolutionUpper = nextPowerOfTwo + 1;

        int distanceToLower = Mathf.Abs(oldResolution - newResolutionLower);
        int distanceToUpper = Mathf.Abs(oldResolution - newResolutionUpper);

        if (distanceToLower < distanceToUpper) return Resize(source, newResolutionLower, newResolutionLower);
        else return Resize(source, newResolutionUpper, newResolutionUpper);
    }
}