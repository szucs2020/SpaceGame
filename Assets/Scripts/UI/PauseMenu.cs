using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
    CanvasGroup menu;
    bool active = false;

	// Use this for initialization
	void Start () {
        menu = this.gameObject.GetComponent<CanvasGroup>();
        menu.alpha = 0;
        menu.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!active) {
                active = true;
                menu.alpha = 1;
                menu.interactable = true;
            } else {
                active = false;
                menu.alpha = 0;
                menu.interactable = false;
            }
        }
	}
}
