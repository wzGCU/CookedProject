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
            int scenes = SceneManager.sceneCount;
            if (scenes > 1)
            {
                for (int i = 0; i < scenes; i++)
                {
                    if (SceneManager.GetSceneAt(i).name == "Name of Maze Scene")
                    {
                        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
                    }
                }
            }
            Resources.UnloadUnusedAssets();

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
    }
}
