#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;
using TMPro;

public class TransparentScreenshot : MonoBehaviour
{
    public string[] Texts = new string[]{ };
    public Camera captureCamera;  // Assign the camera in the Inspector
    public LayerMask captureLayer; // Assign the specific layer
    public int imageWidth = 1024;
    public int imageHeight = 1024;
    public string savePath = "Assets/Screenshot.png";
    public TextMeshProUGUI _textElement;

    public void CaptureScreenshots()
    {
        foreach (string str in Texts)
        {
            _textElement.text = str;
            CaptureScreenshot();
        }
    }
    
    public void CaptureScreenshot()
    {
        if (captureCamera == null)
        {
            Debug.LogError("Capture camera is not assigned!");
            return;
        }

        // Set the culling mask to only render the selected layer
        captureCamera.cullingMask = captureLayer;

        // Create a RenderTexture
        RenderTexture rt = new RenderTexture(imageWidth, imageHeight, 24, RenderTextureFormat.ARGB32);
        captureCamera.targetTexture = rt;

        // Set camera to clear with transparency
        captureCamera.clearFlags = CameraClearFlags.SolidColor;
        captureCamera.backgroundColor = new Color(0, 0, 0, 0);

        // Render the camera
        captureCamera.Render();

        // Read pixels from RenderTexture
        RenderTexture.active = rt;
        Texture2D screenshot = new Texture2D(imageWidth, imageHeight, TextureFormat.ARGB32, false);
        screenshot.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        screenshot.Apply();

        // Reset camera target texture
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        // Encode texture to PNG and save
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(savePath, _textElement.text.Replace("\n", "")) + ".png", bytes);
        Debug.Log("Screenshot saved to: " + savePath);

        // Refresh the Asset database in the Editor
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        // Cleanup
        DestroyImmediate(screenshot);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TransparentScreenshot))]
public class TransparentScreenshotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TransparentScreenshot script = (TransparentScreenshot)target;
        if (GUILayout.Button("Capture Screenshot"))
        {
            script.CaptureScreenshots();
        }
    }
}
#endif
