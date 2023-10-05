using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Update()
    {

        if (BluePlayerManager._blueReached && RedPlayerManager._redReached)
        {
            StartCoroutine(NextLevet());

        }

    }

    IEnumerator NextLevet()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneBuildIndex: 1);

        BluePlayerManager._blueReached = false;
        RedPlayerManager._redReached = false;
    }
}
