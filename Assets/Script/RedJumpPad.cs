using UnityEngine;
public class RedJumpPad : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    public float jumpForce;
    public ParticleSystem _flipParticule;

    private void OnCollisionEnter(Collision other)
    {
        _anim = other.gameObject.GetComponent<Animator>();
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (other.gameObject.GetComponent<RedPlayerManager>())
        {
            _anim.SetBool("Flip", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _flipParticule.Play();
        }

    }


}
