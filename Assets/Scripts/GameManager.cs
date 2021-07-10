using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartGame", 1.5f);
    }
    
    public void StartGame()
    {
        gameStarted=true;
    }
    

    public void NextLevel2()
    {
        SceneManager.LoadScene(1);
    }
    
    public void NextLevel3()
    {
        SceneManager.LoadScene(2);
    }
    
    public void Restart1()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Restart2()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Restart3()
    {
        SceneManager.LoadScene(2);
    }
    

}
