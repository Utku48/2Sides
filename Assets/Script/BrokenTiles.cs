using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenTiles : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;  
    [SerializeField] private GameObject[] _brokenBlocks;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<RedPlayerManager>())
        {
            gameObject.transform.DOScale(Vector3.zero, .5f);
            _particles[0].Play();
            _particles[1].Play();

        }
    }
}
