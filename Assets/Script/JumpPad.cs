using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f; // Zıplama kuvveti

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<RedPlayerManager>() || other.gameObject.GetComponent<BluePlayerManager>()) // Karakter nesnesi jump pad'in üzerine geldiğinde
        {
            Debug.Log("Jumppad");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Karakteri yukarı doğru zıplat
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
