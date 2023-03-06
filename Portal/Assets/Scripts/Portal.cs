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
        screen.enabled = false;
        CreateViewTexture();
        portalCam.Render();
        linkedPortal.screen.material.SetInt ("displayMask", 1);
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
