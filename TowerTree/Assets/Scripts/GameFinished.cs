using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFinished : MonoBehaviour
{
     [SerializeField] 
     private Text _pointsText;

     private void Start()
     {
          _pointsText.text = $"Points: {GameManager.Instance.CurrentPoints}";
     }

     private void Update()
     {
          if (Input.GetMouseButtonDown(0))
          {
               SceneManager.LoadScene("MainMenu");
          }
     }
}
