using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    [SerializeField] private GameObject _character;
    RaycastHit hit;
    [SerializeField] private LayerMask _groundLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ground") || other.GetComponent<BluePlayerManager>() || other.GetComponent<RedPlayerManager>() || other.CompareTag("Obstacle"))
        {
            BluePlayerManager blue = _character.GetComponent<BluePlayerManager>();
            RedPlayerManager red = _character.GetComponent<RedPlayerManager>();


            if (blue != null)
            {
                blue.jumpAble = true;
            }
            else if (red != null)
            {
                red.jumpAble = true;

            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ground") || other.GetComponent<BluePlayerManager>() || other.GetComponent<RedPlayerManager>() || other.CompareTag("Obstacle"))
        {
            if (Physics.Raycast(this.transform.position + transform.up, -transform.up, out hit, 2f, _groundLayer))
            {
                return;
            }

            BluePlayerManager blue = _character.GetComponent<BluePlayerManager>();
            RedPlayerManager red = _character.GetComponent<RedPlayerManager>();


            if (blue != null)
            {
                blue.jumpAble = false;
            }
            else if (red != null)
            {
                red.jumpAble = false;
            }
        }
    }



}
