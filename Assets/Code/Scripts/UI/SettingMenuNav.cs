using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuNav : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    [SerializeField] public GameObject settingMenu;
    [SerializeField] public GameObject mainPagePanel; // Assign in inspector
    [SerializeField] public GameObject soundPagePanel;  // Assign in inspector



    void Start()
    {
        settingMenu.SetActive(false);
        mainPagePanel.SetActive(false);
        soundPagePanel.SetActive(false);

    }
    public void ToggleSetting()
    {
        settingMenu.SetActive(!settingMenu.activeSelf);
        if (settingMenu.activeSelf)
        {
            mainPagePanel.SetActive(true);
        }
    }
    // Update is called once per frame
    public void OpenPanel(GameObject panel)
    {
        // Disable all panels
        mainPagePanel.SetActive(false);
        soundPagePanel.SetActive(false);
        // Enable the desired panel
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        // Disable the desired panel
        panel.SetActive(false);
    }
    public void ExitGame()
    {
        // If we're running in a standalone build of the game
#if UNITY_STANDALONE
        // Quit the application
        Application.Quit();
#endif

        // If we're running in the editor
#if UNITY_EDITOR
        // Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}