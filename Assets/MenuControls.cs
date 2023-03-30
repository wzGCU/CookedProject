using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Undercooked
{
    public class MenuControls : MonoBehaviour
    {
       
        public void OpenMenu()
        {
            Time.timeScale=1.0f; 
            Destroy(GameObject.Find("[Debug Updater]"));
            SceneManager.LoadScene("MAINMENU");
        }
        public void OpenMapA()
        {
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("LevelA_Map");
        }

        
        public void OpenMapB()
        {
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("LevelB_Map");
        
        }

        public void QuitGame()
        {
           Application.Quit();
           
        }

        public void OpenIntroductionA()
        {
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("LevelA_Introduction");
        }

        public void OpenIntroductionB()
        {
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene("LevelB_Introduction");
        }
    }
}
