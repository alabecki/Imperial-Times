using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prescene : MonoBehaviour
{
    void Awake()
    {
        App app = Object.FindObjectOfType<App>();

        if (app == null)
        { UnityEngine.SceneManagement.SceneManager.LoadScene("_preload"); }
    }
}
	

