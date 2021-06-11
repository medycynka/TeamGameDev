using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    public class EndingScreenManager : MonoBehaviour
    {
        public GameObject endingScreen;

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}