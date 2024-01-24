using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private static int nextSceneIndex; // Başlangıçta bir sonraki seviyenin index değeri
    public static float pastTime = 0;
       

    [SerializeField] public AudioSource _walkSoil;


    private void Awake()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex;


        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


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

                UnlockedNewLevel();
                BluePlayerManager._blueReached = false;
                RedPlayerManager._redReached = false;
                pastTime = 0;

            }

        }

    }

    void UnlockedNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

}


