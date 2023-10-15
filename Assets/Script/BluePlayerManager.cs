using DG.Tweening;
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
    [SerializeField] private Vector3 _blueSpawnPos;
    [SerializeField] private GameObject blueFlag;
    [SerializeField] private Transform[] topDownBlueFlag;
    [SerializeField] private ParticleSystem[] _particiles;

    public static bool isMoving = false;
    public float _gravityValue = 35f;

    public bool jumpAble;
    public static bool _blueReached;

    void Start()
    {
        _blueSpawnPos = gameObject.transform.position;
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
        isMoving = true;
        _moveY = 1;
        transform.rotation = Quaternion.Euler(-90, 180, 90);
        _anim.SetBool("isRun", true);
    }
    public void DownMove()
    {
        isMoving = true;
        _moveY = -1;
        transform.rotation = Quaternion.Euler(90, 90, 0);
        _anim.SetBool("isRun", true);
    }
    public void Stop()
    {
        isMoving = false;
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
            if (_blueReached)
            {
                blueFlag.transform.DOMove(topDownBlueFlag[0].position, 3f);
            }
        }

        if (other.tag == "dieLine")
        {
            transform.position = _blueSpawnPos;
            LevelManager.pastTime = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "blueF")
        {
            _blueReached = false;
            LevelManager.pastTime = 0;

            if (!_blueReached)
            {
                blueFlag.transform.DOMove(topDownBlueFlag[1].position, 3f);
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

            _rb.velocity = new Vector3(_jumpForce, _rb.velocity.y, _rb.velocity.z);
            jumpAble = false;
        }

    }

}