using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    private Button _ExitGameButton;

    private void Awake()
    {
        _ExitGameButton = transform.GetComponent<Button>();
    }

    private void Start()
    {
        _ExitGameButton.onClick.AddListener(onButtonClick);
    }
    private void onButtonClick()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
