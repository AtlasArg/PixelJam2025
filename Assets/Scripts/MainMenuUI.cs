using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI: MonoBehaviour
{
    [SerializeField] private string sceneName;
    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AboutButtonClicked()
    {
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false; // Esto es solo para salir del modo Play en el editor
        #endif
    }
}
