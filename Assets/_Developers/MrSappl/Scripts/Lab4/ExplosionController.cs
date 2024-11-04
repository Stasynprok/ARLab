using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosion;

    private void Start()
    {
        _explosion.Play();
        StartCoroutine(KillExplosion());
    }

    IEnumerator KillExplosion()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
