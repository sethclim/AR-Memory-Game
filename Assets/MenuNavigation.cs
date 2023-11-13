using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject startPagePanel; // Assign in inspector
    [SerializeField] public GameObject lobbyPagePanel;  // Assign in inspector
    [SerializeField] public GameObject roomsPagePanel;
    [SerializeField] public GameObject createRoomPagePanel; // Assign in inspector
  

    void Start()
    {
        startPagePanel.SetActive(true);
        lobbyPagePanel.SetActive(false);
        roomsPagePanel.SetActive(false);
        createRoomPagePanel.SetActive(false);
    }

    // Update is called once per frame
    public void OpenPanel(GameObject panel)
    {
        // Disable all panels
        startPagePanel.SetActive(false);
        lobbyPagePanel.SetActive(false);
        roomsPagePanel.SetActive(false);
        createRoomPagePanel.SetActive(false);
        // Enable the desired panel
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        // Disable the desired panel
        panel.SetActive(false);
    }
}
