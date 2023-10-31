using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueObstacle : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        Vector3 gravityValue = new Vector3(Physics.gravity.y/2, 0f, 0f);
        _rb.AddForce(gravityValue, ForceMode.Acceleration);

    }
}
