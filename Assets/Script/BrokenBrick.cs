using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenBrick : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dust;
    [SerializeField] private Animator _shakeTile;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<RedPlayerManager>() || other.gameObject.GetComponent<BluePlayerManager>())
        {
            _shakeTile.SetBool("shake", true);

            StartCoroutine(ExplodeTile());

        }
    }

    IEnumerator ExplodeTile()
    {

        yield return new WaitForSeconds(1.5f);
        _dust.Play();
        this.gameObject.transform.DOScale(Vector3.zero, .1f);
        this.gameObject.transform.DOMove(Vector3.zero, 1f);
    }
}
