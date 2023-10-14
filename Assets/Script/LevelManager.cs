using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int nextSceneIndex = 0; // Başlangıçta bir sonraki seviyenin index değeri
    public static int reachValue = 0;
    public void Update()
    {
        if (reachValue == 2)
        {
            StartCoroutine(LoadNextLevelWithDelay());
        }
        Debug.Log(reachValue);
    }

    IEnumerator LoadNextLevelWithDelay()
    {
        yield return new WaitForSeconds(3.5f);


        nextSceneIndex++;


        SceneManager.LoadScene(nextSceneIndex);


        BluePlayerManager._blueReached = false;
        RedPlayerManager._redReached = false;
    }
}
