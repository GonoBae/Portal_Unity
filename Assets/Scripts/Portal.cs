using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Main Settings")]
    public Portal linkedPortal;
    public MeshRenderer screen;

    Camera playerCam;
    Camera portalCam;
    RenderTexture viewTexture;

    private void Awake() {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = false;
    }

    public void Render() {

        if(!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCam))
        {
            var testTexture = new Texture2D(1, 1);
            testTexture.SetPixel(0, 0, Color.red);
            testTexture.Apply();
            linkedPortal.screen.material.SetTexture("_MainTex", testTexture);
            return;
        }

        screen.enabled = false;
        CreateViewTexture();
        portalCam.Render();
        linkedPortal.screen.material.SetTexture ("_MainTex", viewTexture);
        screen.enabled = true;
    }

    private void CreateViewTexture() {
        if(viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height) {
            if(viewTexture != null) {
                viewTexture.Release();
            }
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            portalCam.targetTexture = viewTexture;
            linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        }
    }
}
