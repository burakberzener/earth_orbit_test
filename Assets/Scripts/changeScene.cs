using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour {

    public void GotoGTScene()
    {
        SceneManager.LoadScene("GroundTrack");
    }

    public void GotoSTScene()
    {
        SceneManager.LoadScene("SpaceTrack");
    }

    public void GotoMMScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AppExit()
    {
        Application.Quit();
    }
}