using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public GameObject graphicsObject;
    public GameObject graphicsClone { get; set; }
    public Vector3 previousOffsetFromPortal { get; set; }
    public Material[] originalMaterials { get; set; }
    public Material[] cloneMaterials { get; set; 
    }
    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        transform.rotation = rot;
    }

    public virtual void EnterPortalThreshold()
    {
        if(graphicsClone == null)
        {
            graphicsClone = Instantiate(graphicsObject);
            graphicsClone.transform.parent = graphicsObject.transform.parent;
            graphicsClone.transform.localScale = graphicsObject.transform.localScale;
            originalMaterials = GetMaterial(graphicsObject);
            cloneMaterials = GetMaterial(graphicsClone);
        }
        else
        {
            graphicsClone.SetActive(true);
        }
    }

    public virtual void ExitPortalThreshold() {
        graphicsClone.SetActive(false);
        // 
    }

    Material[] GetMaterial(GameObject g)
    {
        var renderers = g.GetComponentsInChildren<MeshRenderer>();
        var matList = new List<Material>();
        foreach(var renderer in renderers)
        {
            foreach(var mat in renderer.materials)
            {
                matList.Add(mat);
            }
        }
        return matList.ToArray();
    }
}
