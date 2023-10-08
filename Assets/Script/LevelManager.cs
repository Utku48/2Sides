using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int nextSceneIndex = 0; // Başlangıçta bir sonraki seviyenin index değeri

    public void Update()
    {
        if (BluePlayerManager._blueReached && RedPlayerManager._redReached)
        {
            StartCoroutine(LoadNextLevelWithDelay());
        }
    }

    IEnumerator LoadNextLevelWithDelay()
    {
        yield return new WaitForSeconds(2f); 

   
        nextSceneIndex++;

      
        SceneManager.LoadScene(nextSceneIndex);

     
        BluePlayerManager._blueReached = false;
        RedPlayerManager._redReached = false;
    }
}
