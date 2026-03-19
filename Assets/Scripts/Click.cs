using UnityEngine;

public class Click : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject playPanel;
    public GameObject customPanel;
    void Start()
    {
        mainPanel.SetActive(true);
    }

    void Update()
    {
        
    }
    public void Credits()
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void PlayButtonMain()
    {
        mainPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    public void ExitToMainMenu()
    {
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void Custom()
    {
        playPanel.SetActive(false);
        customPanel.SetActive(true);
    }
    public void CustomBack()
    {
        playPanel.SetActive(true);
        customPanel.SetActive(false);
    }
}
