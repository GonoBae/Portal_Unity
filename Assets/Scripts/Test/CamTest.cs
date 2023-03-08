using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTest : MonoBehaviour
{
    PortalTest[] portals;

    private void Awake() {
        portals = FindObjectsOfType<PortalTest>();
    }

    private void OnPreCull() {
        for(int i = 0; i < portals.Length; i++) {
            portals[i].Render();
        }
    }
}
