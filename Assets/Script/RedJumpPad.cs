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
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            _anim.SetBool("ObstacleFlip", true);
            rb.AddForce(new Vector3(.5f, .75f, 0) * jumpForce, ForceMode.Impulse);
            _flipParticule.Play();
        }

    }


}
