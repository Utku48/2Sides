using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JumpPad : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    public float jumpForce = 10f;
    public ParticleSystem _flipParticule;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<RedPlayerManager>() || other.gameObject.GetComponent<BluePlayerManager>()) // Karakter nesnesi jump pad'in üzerine geldiğinde
        {
            _anim = other.gameObject.GetComponent<Animator>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {

                _anim.SetBool("Flip", true);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _flipParticule.Play();
            }
        }
    }


}
