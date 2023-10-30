using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int nextSceneIndex = 0; // Başlangıçta bir sonraki seviyenin index değeri
    public static float pastTime = 0;
    public void Update()
    {
        if (RedPlayerManager._redReached && BluePlayerManager._blueReached)
        {
            pastTime += Time.deltaTime;

            Debug.Log(pastTime);
            if (pastTime >= 2f)
            {

                nextSceneIndex++;
                SceneManager.LoadScene(nextSceneIndex);

                BluePlayerManager._blueReached = false;
                RedPlayerManager._redReached = false;
                pastTime = 0;
                RedPlayerManager.a = 0;
                BluePlayerManager.b = 0;
            }

        }

    }
}


