using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BluePlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveY;
    [SerializeField] private float _moveYspeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _blueSpawnPos;


    public float _gravityValue = 35f;

    public bool jumpAble;
    public static bool _blueReached;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        Vector3 gravityValue = new Vector3(-_gravityValue * _rb.mass, 0f, 0f);
        _rb.AddForce(gravityValue);

        _rb.velocity = new Vector3(_rb.velocity.x, _moveY * _moveYspeed * Time.deltaTime, _rb.velocity.z);

    }
    #region UpDownMove
    public void UpMove()
    {
        _moveY = 1;
        transform.rotation = Quaternion.Euler(-90, 180, 90);
        _anim.SetBool("isRun", true);
    }
    public void DownMove()
    {
        _moveY = -1;
        transform.rotation = Quaternion.Euler(90, 90, 0);
        _anim.SetBool("isRun", true);
    }
    public void Stop()
    {
        _moveY = 0;
        _anim.SetBool("isRun", false);
        _anim.SetBool("isIdle", true);
    }
    #endregion

    #region OnTrigger'lar ve OnCollision'lar
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "blueF")
        {
            _blueReached = true;
        }

        if (other.tag == "dieLine")
        {
            transform.position = _blueSpawnPos.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "blueF")
        {
            _blueReached = false;

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            jumpAble = true;
            _anim.SetBool("isJump", false);
            _anim.SetBool("isIdle", true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            jumpAble = false;
        }
    }
    #endregion

    public void Jump()
    {
        if (jumpAble)
        {
            _anim.SetBool("isJump", true);
            _rb.velocity = new Vector3(_jumpForce, _rb.velocity.y, _rb.velocity.z);
            jumpAble = false;
        }

    }

}
