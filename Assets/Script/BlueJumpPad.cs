using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueJumpPad : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    public float jumpForce = 10f;
    public ParticleSystem _flipParticule;

    private void OnCollisionEnter(Collision other)
    {
        _anim = other.gameObject.GetComponent<Animator>();
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (other.gameObject.GetComponent<BluePlayerManager>())
        {
            _anim.SetBool("Flip", true);
            rb.AddForce(new Vector3(7f, 0, 0), ForceMode.Impulse);
            _flipParticule.Play();
        }

    }
}
