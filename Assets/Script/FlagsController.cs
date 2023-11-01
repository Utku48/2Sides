using UnityEngine;

public class FlagsController : MonoBehaviour
{
    public Collider targetCollider;

    void Update()
    {

        if (targetCollider.bounds.Contains(transform.position))
        {

        }
        else
        {

        }
    }
}
