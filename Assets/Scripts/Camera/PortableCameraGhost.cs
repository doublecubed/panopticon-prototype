using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PortableCameraGhost : MonoBehaviour
{
    #region REFERENCES

    [SerializeField] private Material _placeableMaterial;
    [SerializeField] private Material _notPlaceableMaterial;

    [SerializeField] private List<MeshRenderer> _meshRenderers;

    public Camera GhostCam { get; private set; }
    
    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        GhostCam = GetComponentInChildren<Camera>();
        GetAllMeshRenderers();
        UpdateMeshRenderers(true);
    }

    #endregion

    #region METHODS

    private void GetAllMeshRenderers()
    {
        transform.GetComponentsInChildren<MeshRenderer>(true, _meshRenderers);
    }

    private void UpdateMeshRenderers(bool placeable)
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            Material[] materials = Enumerable.Repeat(placeable ? _placeableMaterial : _notPlaceableMaterial, meshRenderer.materials.Length).ToArray();
            meshRenderer.materials = materials;
        }
    }

    public void RenderGhostCameraPlaceability(bool placeable)
    {
        UpdateMeshRenderers(placeable);
        GhostCam.enabled = placeable;
    }
    
    #endregion

}
