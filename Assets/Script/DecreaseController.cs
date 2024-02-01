using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseController : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        RedPlayerManager redPlayerManager = other.gameObject.GetComponent<RedPlayerManager>();
        BluePlayerManager bluePlayerManager = other.gameObject.GetComponent<BluePlayerManager>();

        if (redPlayerManager != null || bluePlayerManager != null)
        {
            Vector3 _pos = other.gameObject.transform.position;
            if (redPlayerManager != null)
            {
                redPlayerManager.GetComponent<RedPlayerManager>()._moveXspeed += 40;

                other.gameObject.transform.DOScale(Vector3.zero, .5f)
        .OnComplete(() =>
            other.gameObject.transform.DOMove(
                new Vector3(_pos.x - .5f, _pos.y, _pos.z),
                .25f)
            .OnComplete(() =>
                other.gameObject.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1f)

                )
        );


            }
            if (bluePlayerManager != null)
            {
                bluePlayerManager.GetComponent<BluePlayerManager>()._moveYspeed += 40;

                other.gameObject.transform.DOScale(Vector3.zero, .5f)
        .OnComplete(() =>
            other.gameObject.transform.DOMove(
                new Vector3(_pos.x, _pos.y +.5f, _pos.z),
                .25f)
            .OnComplete(() =>
                other.gameObject.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1f
                ))
        );
            }
        }

    }
}
