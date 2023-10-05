using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedPlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveXspeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Animator _anim;

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
    }
    public void RightMove()
    {
        _moveX = 1;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        _anim.SetBool("isRun", true);
    }
    public void Stop()
    {
        _moveX = 0;
        _anim.SetBool("isRun", false);
        _anim.SetBool("isIdle", true);
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "redF")
        {
            _redReached = true;
        }
        if (other.tag == "dieLine")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "redF")
        {
            _redReached = false;

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
    public void Jump()
    {
        if (jumpAble)
        {
            _anim.SetBool("isJump", true);
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
            jumpAble = false;
        }

    }

}
