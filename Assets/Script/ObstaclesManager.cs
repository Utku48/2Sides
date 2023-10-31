using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{


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
    }
}
