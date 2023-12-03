using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTeleport : MonoBehaviour
{
    [SerializeField] private Transform _upTeleportPos;
    [SerializeField] private ParticleSystem _upRedPortal;
    [SerializeField] private ParticleSystem _downRedPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _upRedPortal.Play();
            other.transform.DOScale(Vector3.zero, .5f).OnComplete(() => other.transform.DOMove(_upTeleportPos.position, .2f).OnComplete(() => other.transform.DOScale(new Vector3(.5f, .5f, .5f), .5f).OnComplete(() => _upRedPortal.Stop())));
            _downRedPortal.Stop();
        }
    }

}
