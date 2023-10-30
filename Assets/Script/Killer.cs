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
    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private ParticleSystem _boom;

    private void Start()
    {
        _killerStartPos = gameObject.transform.position;
    }
    private void Update()
    {
        float _distance = Vector3.Distance(RedMan.transform.position, RedFlag.transform.position);

        if (_distance < 2.5f)
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
            ContactPoint contact = other.contacts[0];
            Vector3 collisionPoint = contact.point;  //Karakter ile Killer Topun tam çarpıştıgı yerin Vector3 değerleri

            ParticleSystem particleSystem = Instantiate(_blood, new Vector3(collisionPoint.x, collisionPoint.y - .25f, collisionPoint.z), Quaternion.Euler(0f, 90f, -90f));
            particleSystem.Play();

            Vector3 _target = other.gameObject.GetComponent<RedPlayerManager>()._redSpawnPos;
            other.gameObject.transform.position = _target;

            other.gameObject.SetActive(false);
            StartCoroutine(_moveDelay(other.gameObject));

            _kRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            gameObject.transform.position = _killerStartPos;


        }


    }

    IEnumerator _moveDelay(GameObject redMan)
    {
        yield return new WaitForSeconds(1f);
        redMan.SetActive(true);
        redMan.GetComponent<RedPlayerManager>()._particiles[2].Play();
    }


}
