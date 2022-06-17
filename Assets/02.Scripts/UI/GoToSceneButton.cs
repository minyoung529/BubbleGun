using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToSceneButton : MonoBehaviour
{
    private Button button;
    public string sceneName;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => GoToScene(sceneName));
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
