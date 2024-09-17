using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject PauseMenu;
    public ButtonClickSound ButtonSound;

    public void Pause()
    {
        ButtonSound.PlaySound();
        PauseMenu.SetActive(true);
    }

    public void Home()
    {
        ButtonSound.PlaySound();
        SceneManager.LoadScene("Game Scene");
    }

    public void Resume()
    {
        ButtonSound.PlaySound();
        PauseMenu.SetActive(false);
    }
}
