using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Click : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject creditsPanel;
    public GameObject playPanel;
    public GameObject customPanel;
    [Header("Custom Input")]
    public TMP_InputField gridSizeInput;
    public TMP_InputField mineCountInput;
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
    public void SetEasy()
    {
        GameSettings.gridSize = 10;
        GameSettings.mineCount = 10;

        SceneManager.LoadScene("saper"); 
    }

    public void SetMedium()
    {
        GameSettings.gridSize = 17;
        GameSettings.mineCount = 40;

        SceneManager.LoadScene("saper");
    }

    public void SetHard()
    {
        GameSettings.gridSize = 22;
        GameSettings.mineCount = 99;

        SceneManager.LoadScene("saper");
    }
    public void StartCustomGame()
    { 
        int size;
        int mines;

        if (!int.TryParse(gridSizeInput.text, out size)) return;
        if (!int.TryParse(mineCountInput.text, out mines)) return;

        if (size < 2) size = 2;

        int maxMines = size * size - 1;

        if (mines < 1) mines = 1;
        if (mines > maxMines) mines = maxMines;

        GameSettings.gridSize = size;
        GameSettings.mineCount = mines;

        SceneManager.LoadScene("saper");
    }
}
