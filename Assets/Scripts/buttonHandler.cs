using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class buttonHandler : MonoBehaviour {

    public Button spacetrackButton;

    void Start()
    {
        Button spacetrackBtn = spacetrackButton.GetComponent<Button>();
        spacetrackBtn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene("GroundTrack");
    }
}