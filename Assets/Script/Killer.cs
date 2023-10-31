using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{

    [SerializeField] private Vector3 _killerStartPos;
    [SerializeField] private GameObject RedMan;
    [SerializeField] private GameObject RedFlag;
    [SerializeField] private Rigidbody _kRb;

    [SerializeField] private ParticleSystem _boom;

    RedPlayerManager _redPlayerManager;
    private void Start()
    {
        _redPlayerManager = RedMan.GetComponent<RedPlayerManager>();
        _killerStartPos = gameObject.transform.position;
    }
    private void Update()
    {
        float _distance = Vector3.Distance(RedMan.transform.position, RedFlag.transform.position);

        if (_distance < 2.5f && _redPlayerManager.gameObject.activeInHierarchy)
        {
            _kRb = gameObject.GetComponent<Rigidbody>();
            _kRb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
            _boom.Play();

        }
        else if (other.gameObject.GetComponent<RedPlayerManager>())
        {

            _redPlayerManager.Die();
            _redPlayerManager.gameObject.SetActive(false);

            _kRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            gameObject.transform.position = _killerStartPos;


        }


    }



}
