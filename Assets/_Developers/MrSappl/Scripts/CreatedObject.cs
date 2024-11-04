using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedObject : MonoBehaviour
{
    [SerializeField] private string _displayName;
    [SerializeField] private string _description;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speedFastMove;
    [SerializeField] private Collider _collider;

    private Vector3 _targetPosition;
    private int _number = -1;
    private bool _needMove = false;

    public string Name
    {
        get
        {
            if (_number >= 0)
            {
                return _displayName + " " + _number.ToString();
            }
            else
            {
                return _displayName;
            }
        }
    }

    public string Description
    {
        get
        {
            return _description;
        }
    }

    public void GiveNumber(int number)
    {
        _number = number;
    }

    public void AddForceToDirection(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction.normalized * force, ForceMode.Acceleration);
    }

    public void AddExplosion(float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        _rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 0.0f, ForceMode.Acceleration);
    }

    public void FastMoveToTarget(Vector3 targetPoint)
    {
        _targetPosition = targetPoint;
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        _needMove = true;
    }

    private void FixedUpdate()
    {
        if (!_needMove)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(_targetPosition, currentPosition);

        if (distance > 0.1f)
        {
            Vector3 direction = _targetPosition - currentPosition;
            _rigidbody.MovePosition(currentPosition + (direction.normalized * _speedFastMove * Time.fixedDeltaTime));
            return;
        }
        _needMove = false;
        _rigidbody.isKinematic = false;
        _rigidbody.ResetInertiaTensor();
        _collider.enabled = true;
    }
}

