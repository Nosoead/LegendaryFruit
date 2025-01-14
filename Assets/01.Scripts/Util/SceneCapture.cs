using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneCapture : MonoBehaviour
{
    public Camera captureCamera;
    public RawImage rawImage;
    public void CaptureScene()
    {
        Capture();
    }
    
    private void Capture()
    {
        captureCamera.gameObject.SetActive(true);
        Vector3 playerpos = GameManager.Instance.player.transform.position;
        captureCamera.transform.position = new Vector3(playerpos.x, playerpos.y, -10);
        
        captureCamera.orthographic = true;
        captureCamera.orthographicSize = 2f;
        captureCamera.clearFlags = CameraClearFlags.SolidColor;
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        //RenderTexture renderTexture = new RenderTexture(480,370,24);
        captureCamera.targetTexture = renderTexture;
        
        //yield return new WaitForEndOfFrame();
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        
        ShowCaptureImage(screenShot);
        
    }

    private void ShowCaptureImage(Texture2D screenShot)
    {
        rawImage.texture = screenShot;
        
        captureCamera.gameObject.SetActive(false);
        
    }
}
