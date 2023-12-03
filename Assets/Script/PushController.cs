using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("pushControl"))
        {
            Debug.Log("pushMan");
            Animator animator = other.gameObject.transform.parent.gameObject.GetComponent<Animator>();

            if (animator != null && other.gameObject.transform.parent.gameObject.GetComponent<RedPlayerManager>())
            {
                animator.SetBool("Obstacle", RedPlayerManager.isMoving);
            }
            else if (animator != null && other.gameObject.transform.parent.gameObject.GetComponent<BluePlayerManager>())
            {
                animator.SetBool("Obstacle", BluePlayerManager.isMoving);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("pushControl"))
        {
            Animator animator = other.gameObject.transform.parent.gameObject.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetBool("Obstacle", false);
            }
        }
    }

}
