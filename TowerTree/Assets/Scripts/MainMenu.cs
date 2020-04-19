using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        var dontDestroyOnLoadScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("DontDestroyOnLoad");
        if (dontDestroyOnLoadScene != null && dontDestroyOnLoadScene.isLoaded)
        {
            var gameObjects = dontDestroyOnLoadScene.GetRootGameObjects();
            if (gameObjects != null)
            {
                for (var i = 0; i < gameObjects.Length; i++)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("PreLoadScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
