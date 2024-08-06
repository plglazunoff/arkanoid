using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button StartButton;
    public Button ExitButton;

    private void Start()
    {
        StartButton.onClick.AddListener(()=> SceneManager.LoadScene(0));
        ExitButton.onClick.AddListener(()=> Application.Quit());
    }
    
   
}
