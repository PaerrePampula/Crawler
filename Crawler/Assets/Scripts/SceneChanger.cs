using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Changes scene when enabled, just a prototype script for simple timeline use.
/// </summary>
public class SceneChanger : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        ChangeScene();
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
