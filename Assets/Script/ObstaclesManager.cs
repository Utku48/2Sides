using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{

    [SerializeField] private ParticleSystem _hummerDust;
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.TryGetComponent<BluePlayerManager>(out BluePlayerManager _blueP))
        {

            _blueP.Die();
        }
        else if (other.gameObject.TryGetComponent<RedPlayerManager>(out RedPlayerManager _redP))
        {
            _redP.Die();
        }

        else if (other.gameObject.CompareTag("ground"))
        {

        }
    }


}
