using UnityEngine;
public class JumpPad : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    public float jumpForce = 10f;
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
        else if (other.gameObject.GetComponent<BluePlayerManager>())
        {

            _anim.SetBool("Flip", true);
            rb.AddForce(new Vector3(7, 0, 0), ForceMode.Impulse);
            _flipParticule.Play();

        }
    }


}
