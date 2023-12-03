using System.Collections;
using UnityEngine;


public class BluePlayerManager : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _moveY;
    [SerializeField] private float _moveYspeed;
    [SerializeField] public float _jumpForce;
    [SerializeField] private Animator _anim;
    [SerializeField] private Vector3 _blueSpawnPos;
    [SerializeField] private GameObject blueFlag;
    [SerializeField] private GameObject _uButton;
    [SerializeField] private GameObject _dButton;


    [SerializeField] private ParticleSystem[] _particiles;



    public static bool isMoving = false;
    public float _gravityValue = 10f;

    public bool jumpAble;
    public static bool _blueReached;

    GameObject _emmisionParent;
    Material material;

    void Start()
    {
        _blueSpawnPos = gameObject.transform.position;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _emmisionParent = gameObject.transform.GetChild(1).gameObject;
        material = _emmisionParent.GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        Vector3 gravityValue = new Vector3(Physics.gravity.y, 0f, 0f);
        _rb.AddForce(gravityValue, ForceMode.Acceleration);

        _rb.velocity = new Vector3(_rb.velocity.x, _moveY * _moveYspeed * Time.deltaTime, _rb.velocity.z);
        if (Mathf.Abs(_rb.velocity.y) > 0.5f)
        {
            _anim.SetBool("isRun", true);

        }

    }
    #region UpDownMove
    public void UpMove()
    {
        isMoving = true;
        _moveY = 1;
        transform.rotation = Quaternion.Euler(-90, 180, 90);


        material.EnableKeyword("_EMISSION");
    }
    public void DownMove()
    {
        isMoving = true;
        _moveY = -1;
        transform.rotation = Quaternion.Euler(90, 90, 0);



        material.EnableKeyword("_EMISSION");
    }
    public void Stop()
    {
        isMoving = false;
        _moveY = 0;
        _anim.SetBool("isRun", false);
        _anim.SetBool("isIdle", true);

        material.DisableKeyword("_EMISSION");
    }
    #endregion

    #region OnTrigger'lar ve OnCollision'lar
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "blueF")
        {
            _blueReached = true;
            other.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);

            Material _bFlagMaterial = other.GetComponent<Renderer>().material;
            _bFlagMaterial.EnableKeyword("_EMISSION");
        }

        if (other.tag == "dieLine")
        {
            ReSpawnBlue();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "blueF")
        {
            _blueReached = false;
            LevelManager.pastTime = 0;

            Material _bFlagMaterial = other.GetComponent<Renderer>().material;
            _bFlagMaterial.DisableKeyword("_EMISSION");

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
    public void ReSpawnBlue()
    {
        transform.position = _blueSpawnPos;
        LevelManager.pastTime = 0;
        _particiles[1].Play();

    }

    public void Die()
    {
        LevelManager.Instance.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(ReSpawnDelayBlue());

    }
    IEnumerator ReSpawnDelayBlue()
    {
        gameObject.SetActive(false);
        ParticleSystem p = Instantiate(_particiles[3], _particiles[3].transform.position, transform.rotation);
        p.gameObject.SetActive(true);
        p.Play();
        Destroy(p.gameObject, 1.5f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(true);
        ReSpawnBlue();




    }

}