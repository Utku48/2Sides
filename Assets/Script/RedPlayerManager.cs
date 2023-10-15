using DG.Tweening;
using UnityEngine;

public class RedPlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveXspeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Animator _anim;
    [SerializeField] private Vector3 _redSpawnPos;
    [SerializeField] private GameObject redFlag;
    [SerializeField] private Transform[] topDownRedFlag;
    public ParticleSystem[] _particiles;


    public static bool isMoving = false;


    public static bool _redReached;
    public bool jumpAble;

    void Start()
    {
        _redSpawnPos = gameObject.transform.position;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(_moveX * _moveXspeed * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);

    }
    #region Left Right Move
    public void LeftMove()
    {

        isMoving = true;
        _moveX = -1;
        transform.rotation = Quaternion.Euler(0, 270, 0);
        _anim.SetBool("isRun", true);
        _anim.SetBool("isIdle", false);

    }
    public void RightMove()
    {

        isMoving = true;
        _moveX = 1;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _anim.SetBool("isRun", true);
        _anim.SetBool("isIdle", false);

    }
    public void Stop()
    {
        isMoving = false;
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
            _particiles[1].Play();
            _redReached = true;
            if (_redReached)
            {
                redFlag.transform.DOMove(topDownRedFlag[0].position, 3f);
            }
        }
        if (other.tag == "dieLine")
        {
            transform.position = _redSpawnPos;
            LevelManager.pastTime = 0;


        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "redF")
        {
            _redReached = false;
            LevelManager.pastTime = 0;

            if (!_redReached)
            {
                redFlag.transform.DOMove(topDownRedFlag[1].position, 3f);
            }
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
            _particiles[0].Play();
            _anim.SetTrigger("Jump");
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            jumpAble = false;
        }

    }

}
