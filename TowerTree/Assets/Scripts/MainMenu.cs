using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        var dontDestroyOnLoadScene = GameManager.Instance?.gameObject.scene;
        if (dontDestroyOnLoadScene != null)
        {
            var gameObjects = dontDestroyOnLoadScene.Value.GetRootGameObjects();
            if (gameObjects != null)
            {
                for (var i = 0; i < gameObjects.Length; i++)
                {
                    Destroy(gameObjects[i]);
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
