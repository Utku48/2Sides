using UnityEngine;

public class RedPlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveXspeed;
    [SerializeField] private float _jumpForce;
    public Animator _anim;
    [SerializeField] private Transform _redSpawnPos;

    [SerializeField] private GameObject Obstacle;


    public static bool _redReached;
    public bool jumpAble;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //_rb.AddForce(new Vector3(0, -2f, 0), ForceMode.Force);
        _rb.velocity = new Vector3(_moveX * _moveXspeed * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);

    }
    #region Left Right Move
    public void LeftMove()
    {
        _moveX = -1;
        transform.rotation = Quaternion.Euler(0, 270, 0);
        _anim.SetBool("isRun", true);
        _anim.SetBool("isIdle", false);

    }
    public void RightMove()
    {
        _moveX = 1;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _anim.SetBool("isRun", true);
        _anim.SetBool("isIdle", false);

    }
    public void Stop()
    {
        _moveX = 0;
        _anim.SetBool("isRun", false);
        _anim.SetBool("isIdle", true);

    }
    #endregion
    #region OnTrigger'lar ve OnCollision'lar
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "redF")
        {
            _redReached = true;
        }
        if (other.tag == "dieLine")
        {
            transform.position = _redSpawnPos.position;
        }

        if (other.tag == "Obstacle" && Vector3.Distance(this.gameObject.transform.position, Obstacle.transform.position) <= 0.45)
        {
            Debug.Log(Vector3.Distance(this.gameObject.transform.position, Obstacle.transform.position));
            _anim.SetBool("Obstacle", true);
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "redF")
        {
            _redReached = false;

        }
        if (other.tag == "Obstacle" && Vector3.Distance(this.gameObject.transform.position, Obstacle.transform.position) > 0.45)
        {
            Debug.Log(Vector3.Distance(this.gameObject.transform.position, Obstacle.transform.position));
            _anim.SetBool("Obstacle", false);
        }


    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            _anim.SetBool("isIdle", true);
        }

    }
    #endregion
    public void Jump()
    {
        if (jumpAble)
        {
            _anim.SetTrigger("Jump");

            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            jumpAble = false;
        }

    }

}
