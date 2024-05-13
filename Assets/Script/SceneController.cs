using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
   public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;

        // Kiểm tra nếu đang chuyển sang Select Character Scene, hủy bỏ đối tượng CharacterSelector cũ nếu có
        if (name == "Select Character Scene")
        {
            CharacterSelector.DestroySingleton();
        }
    }    

    public void ExitScene()
    {
        Application.Quit();
    }
}
