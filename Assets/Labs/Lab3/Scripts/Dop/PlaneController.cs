using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneController : MonoBehaviour
{
    private ARPlaneManager _planeManager;

    private List<GameObject> _planeObjects = new List<GameObject>();
    private List<LineRenderer> _lines = new List<LineRenderer>();
    private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
    private List<Material> _meshMaterials = new List<Material>();

    private List<Color> _startColors = new List<Color>();
    private List<Color> _onNonSelectColor = new List<Color>();

    private float _lineWidth = -1;

    private void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }

    public void OnSelectObject()
    {
        OnUnselectObject();
        ClearAll();

        GetPlanesObject();
        GetLinesRenderer();
        GetMeshRenderers();
        GetStartColors();

        DisableLines();
        ChangeColorOnSelectObject();
    }

    public void OnUnselectObject()
    {
        SetStartColors();
        EnableLines();

        ClearAll();
    }

    private void ClearAll()
    {
        _planeObjects.Clear();
        _lines.Clear();
        _meshRenderers.Clear();
        _meshMaterials.Clear();
        _startColors.Clear();
        _onNonSelectColor.Clear();
    }

    private void SetStartColors()
    {
        for (int i = 0; i < _meshMaterials.Count; i++)
        {
            _meshMaterials[i].color = _startColors[i];
        }
    }

    private void EnableLines()
    {
        if (!(_lines.Count > 0))
        {
            return;
        }

        foreach (LineRenderer line in _lines)
        {
            line.startWidth = _lineWidth;
            line.endWidth = _lineWidth;
        }
    }

    private void ChangeColorOnSelectObject()
    {
        for (int i = 0; i < _meshMaterials.Count; i++)
        {
            _meshMaterials[i].color = _onNonSelectColor[i];
        }
    }

    private void GetStartColors()
    {
        if (!(_meshRenderers.Count > 0))
        {
            return;
        }

        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            foreach (Material mat in meshRenderer.materials)
            {
                _meshMaterials.Add(mat);
                _startColors.Add(mat.color);
            }
        }
        SetNonselectMaterials();
    }

    private void SetNonselectMaterials()
    {
        for (int i = 0; i < _startColors.Count; i++)
        {
            Color startColor = _startColors[i];

            float alpha = startColor.a;
            float newAlpha = alpha * 0.01f * 10f;
            startColor.a = newAlpha;
            _onNonSelectColor.Add(startColor);
        }
    }

    private void GetMeshRenderers()
    {
        if (!(_planeObjects.Count > 0))
        {
            return;
        }

        foreach (GameObject plane in _planeObjects)
        {
            MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                _meshRenderers.Add(meshRenderer);
            }
        }
    }

    private void GetPlanesObject()
    {
        foreach (ARPlane plane in _planeManager.trackables)
        {
            _planeObjects.Add(plane.gameObject);
        }
    }

    private void GetLinesRenderer()
    {
        if (!(_planeObjects.Count > 0))
        {
            return;
        }

        foreach (GameObject plane in _planeObjects)
        {
            LineRenderer lineRenderer = plane.GetComponent<LineRenderer>();
            if (lineRenderer)
            {
                _lines.Add(lineRenderer);
            }
        }

        if (_lineWidth < 0)
        {
            _lineWidth = _lines[0].endWidth;
        }
    }

    private void DisableLines()
    {
        if (!(_lines.Count > 0))
        {
            return;
        }

        foreach (LineRenderer line in _lines)
        {
            line.startWidth = 0;
            line.endWidth = 0;
        }
    }
}
