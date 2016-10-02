using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadScene(string scene) {
        LoadSceneMode mode = LoadSceneMode.Single;
        SceneManager.LoadSceneAsync(scene, mode);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
