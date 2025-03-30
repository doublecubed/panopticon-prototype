using UnityEngine;

public static class CameraUtility
{
    public const int DefaultWidth = 1920;
    public const int DefaultHeight = 1080;
    public const int DefaultDepth = 24;
    
    public static RenderTexture CreateRenderTexture()
    {
        return CreateRenderTexture(DefaultWidth, DefaultHeight, DefaultDepth);
    }

    public static RenderTexture CreateRenderTexture(int width, int height, int depth)
    {
        RenderTexture rt = new RenderTexture(width, height, depth);
        rt.name = "MonitorRT_" + System.Guid.NewGuid().ToString();
        rt.filterMode = FilterMode.Bilinear;
        rt.antiAliasing = 4;
        rt.Create();
        return rt;
    }
}
