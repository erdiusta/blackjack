using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public Button playBtn;
    void Start()
    {
        // Play soundtrack
        playBtn.onClick.AddListener(() => PlayClicked());     
    }

        void PlayClicked()
    {
        SceneManager.LoadScene(1);
    }
}
