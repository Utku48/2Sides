using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BluePlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveY;
    [SerializeField] private float _moveYspeed;
    [SerializeField] private float _jumpForce;

    public bool jumpAble;
    public float yerCekimiKatsayisi = 9.81f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 yerCekimiKuvveti = new Vector3(-yerCekimiKatsayisi * _rb.mass, 0f, 0f);
        _rb.AddForce(yerCekimiKuvveti);
        _rb.velocity = new Vector3(_rb.velocity.x, _moveY * _moveYspeed * Time.deltaTime, _rb.velocity.z);

    }

    public void UpMove()
    {
        _moveY = 1;
        transform.rotation = Quaternion.Euler(-90, 180, 90);
    }
    public void DownMove()
    {
        _moveY = -1;
        transform.rotation = Quaternion.Euler(90, 90, 0);
    }
    public void Stop()
    {
        _moveY = 0;
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
            _rb.velocity = new Vector3(_jumpForce * Time.deltaTime, _rb.velocity.y, _rb.velocity.z);
            jumpAble = false;
        }

    }

}
