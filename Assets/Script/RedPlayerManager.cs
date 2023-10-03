using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class RedPlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveXspeed;
    [SerializeField] private float _jumpForce;

    public bool jumpAble;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        _rb.AddForce(new Vector3(0, -2f, 0), ForceMode.Force);
        _rb.velocity = new Vector3(_moveX * _moveXspeed * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);

    }

    public void LeftMove()
    {
        _moveX = -1;
        transform.rotation = Quaternion.Euler(0, 270, 0);
    }
    public void RightMove()
    {
        _moveX = 1;
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }
    public void Stop()
    {
        _moveX = 0;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            jumpAble = true;
        }
    }
    public void Jump()
    {
        if (jumpAble)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce * Time.deltaTime, _rb.velocity.z);
            jumpAble = false;
        }

    }

}
