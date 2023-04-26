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

    List<PortalTraveller> trackedTravellers;

    private void Awake() {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
        portalCam.enabled = false;
        trackedTravellers = new List<PortalTraveller>();
    }

    private void LateUpdate() {
        HandleTravellers();
    }

    private void HandleTravellers() {
        for(int i = 0; i < trackedTravellers.Count; i++) {
            PortalTraveller traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;
            Vector3 offsetFromPortal = travellerT.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.previousOffsetFromPortal, transform.forward));
            if(portalSide != portalSideOld)
            {
                var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);
                linkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt (i);
                i--;
            }
            else
            {
                traveller.previousOffsetFromPortal = offsetFromPortal;
            }
        }
    }

    public void Render() {

        if(!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCam))
        {
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

    private void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if(!trackedTravellers.Contains(traveller))
        {
            traveller.EnterPortalThreshold();
            traveller.previousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add(traveller);
        }
    }

    private void OnTriggerEnter(Collider other) {
        var traveller = other.GetComponent<PortalTraveller>();
        if(traveller)
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    private void OnTriggerExit(Collider other) {
        var traveller = other.GetComponent<PortalTraveller>();
        if(traveller && trackedTravellers.Contains(traveller)) {
            traveller.ExitPortalThreshold();
            trackedTravellers.Remove(traveller);
        }
    }
}
