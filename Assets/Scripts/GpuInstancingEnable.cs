using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpuInstancingEnable : MonoBehaviour
{
    private void Awake() {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
