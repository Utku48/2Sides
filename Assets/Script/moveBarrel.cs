using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBarrel : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _rb.velocity = new Vector3(-1f, -2, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<RedPlayerManager>() || other.gameObject.GetComponent<BluePlayerManager>())
        {
            transform.position = _startPosition;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("dieLine"))
        {
            Destroy(gameObject);
        }


    }
}
