using System.Collections;
using UnityEngine;

public class RedPlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveXspeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject redFlag;

    public ParticleSystem[] _particiles;

    public Vector3 _redSpawnPos;
    public static int a = 0;

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
            _redReached = true;
            a++;
            if (a == 1)
            {
                _particiles[2].Play();
            }


        }
        if (other.tag == "dieLine")
        {
            ReSpawnRed();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "redF")
        {
            _redReached = false;
            LevelManager.pastTime = 0;
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


    public void ReSpawnRed()
    {
        transform.position = _redSpawnPos;
        LevelManager.pastTime = 0;
        _particiles[1].Play();
        a = 0;

    }

    public void Die()
    {
        LevelManager.Instance.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(ReSpawnDelayRed());

    }
    IEnumerator ReSpawnDelayRed()
    {
        gameObject.SetActive(false);
        ParticleSystem p = Instantiate(_particiles[3], _particiles[3].transform.position, transform.rotation);
        p.gameObject.SetActive(true);
        p.Play();
        Destroy(p.gameObject, 1.5f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(true);
        ReSpawnRed();
    }
}
