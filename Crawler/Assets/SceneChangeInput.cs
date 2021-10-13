using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeInput : MonoBehaviour
{
    SceneChanger sceneChanger;
    void Start()
    {
        sceneChanger = GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChanger.ChangeScenetoMainMenu();
        }
    }
}
