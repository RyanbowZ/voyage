using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Pgame;
    [SerializeField] GameObject Ppause;

    #region 暂停菜单的各个按钮功能
    public void PauseGame(){
        Pgame.SetActive(false);
        Ppause.SetActive(true);
        Time.timeScale=0f;
    }
    public void ResumeGame(){
        Ppause.SetActive(false);
        Pgame.SetActive(true);
        Time.timeScale=1f;
    }
    public void ReloadScene(){
        Time.timeScale=1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitMenu(){
        Time.timeScale=1f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame(){
        Application.Quit();
    }
    #endregion
}
