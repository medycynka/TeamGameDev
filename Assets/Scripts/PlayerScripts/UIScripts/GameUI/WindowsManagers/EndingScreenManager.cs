using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SzymonPeszek.GameUI.WindowsManagers
{
    public class EndingScreenManager : MonoBehaviour
    {
        public GameObject endingScreen;

        public void BackToMainMenu()
        {
            if (Directory.Exists($"{Application.dataPath}/Saves"))
            {
                string[] tmp = Directory.GetFiles($"{Application.dataPath}/Saves");
                for (int i = 0; i < tmp.Length; i++)
                {
                    File.Delete(tmp[i]);
                }

                if (!Directory.Exists($"{Application.dataPath}/Saves"))
                {
                    Directory.CreateDirectory($"{Application.dataPath}/Saves");
                }
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}