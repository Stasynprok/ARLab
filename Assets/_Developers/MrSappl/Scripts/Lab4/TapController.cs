using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    [SerializeField] private LeanFingerTap _doubleTap;
    [SerializeField] private LeanFingerTap _tripleTap;

    public bool ActivateDoubleTap = false;
    public bool ActivateTripleTap = false;

    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _explosionRadius = 10f;
    [SerializeField] private GameObject _explosionPrefab;
    Vector3 _explosionPosition = Vector3.zero;
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_explosionPosition, 0.1f);
    }
    private void OnEnable()
    {
        _doubleTap.OnScreen.AddListener(OnDoubleTap);
        _tripleTap.OnWorld.AddListener(OnTripleTap);
    }

    private void OnDisable()
    {
        _doubleTap.OnScreen.RemoveListener(OnDoubleTap);
        _tripleTap.OnWorld.RemoveListener(OnTripleTap);
    }

    private void OnDoubleTap(Vector2 vector)
    {
        if (!ActivateDoubleTap)
        {
            return;
        }
        _explosionPosition = GetPositionOnRaycast(vector);
        AddExplosion(_explosionPosition);
    }

    private void OnTripleTap(Vector3 vector)
    {
        if (!ActivateTripleTap)
        {
            return;
        }
        InteractionManager.Instance.ReturnToDefaultMode();
    }

    private void AddExplosion(Vector3 tapPosition)
    {
        GameObject explosion;
        explosion = Instantiate(_explosionPrefab, tapPosition, Quaternion.identity);
        ObjectsOnScene.Instance.AddExplosion(_explosionForce, tapPosition, _explosionRadius);
    }

    private Vector3 GetPositionOnRaycast(Vector2 startRay)
    {
        Ray ray = InteractionManager.Instance.ARCamera.ScreenPointToRay(startRay);
        RaycastHit hit;
        Vector3 position = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag != "AR")
            {
                Debug.Log("DOntFind" + hit.collider.tag);
                return position;
            }
            position = hit.point;
            Debug.Log(position);
        }
        return position;
    }
}
