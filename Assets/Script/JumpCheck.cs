using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    [SerializeField] GameObject _character;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ground" || other.gameObject.GetComponent<BluePlayerManager>() || other.gameObject.GetComponent<RedPlayerManager>())
        {
            if (_character.TryGetComponent<BluePlayerManager>(out BluePlayerManager blue))
            {

                blue.jumpAble = true;
            }
            else if (_character.TryGetComponent<RedPlayerManager>(out RedPlayerManager red))
            {
                red.jumpAble = true;

            }
        }

    }



}
