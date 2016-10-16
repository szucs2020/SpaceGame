using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lobby : MonoBehaviour {
    public Sprite blueHead;
    public Sprite blueTorso;
    public Sprite blueLegs;

    public Sprite redHead;
    public Sprite redTorso;
    public Sprite redLegs;

    public Sprite yellowHead;
    public Sprite yellowTorso;
    public Sprite yellowLegs;

    public Sprite greenHead;
    public Sprite greenTorso;
    public Sprite greenLegs;

    private int playerTeam = 0;
    private bool ready = false;


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeTeam () {
        GameObject head = transform.Find("Head").gameObject;
        GameObject torso = transform.Find("Torso").gameObject;
        GameObject legs = transform.Find("Legs").gameObject;

        switch (playerTeam) {
            case 0:
                head.GetComponent<SpriteRenderer>().sprite = redHead;
                torso.GetComponent<SpriteRenderer>().sprite = redTorso;
                legs.GetComponent<SpriteRenderer>().sprite = redLegs;
                playerTeam = 1;
                break;
            case 1:
                head.GetComponent<SpriteRenderer>().sprite = yellowHead;
                torso.GetComponent<SpriteRenderer>().sprite = yellowTorso;
                legs.GetComponent<SpriteRenderer>().sprite = yellowLegs;
                playerTeam = 2;
                break;
            case 2:
                head.GetComponent<SpriteRenderer>().sprite = greenHead;
                torso.GetComponent<SpriteRenderer>().sprite = greenTorso;
                legs.GetComponent<SpriteRenderer>().sprite = greenLegs;
                playerTeam = 3;
                break; 
            case 3:
                head.GetComponent<SpriteRenderer>().sprite = blueHead;
                torso.GetComponent<SpriteRenderer>().sprite = blueTorso;
                legs.GetComponent<SpriteRenderer>().sprite = blueLegs;
                playerTeam = 0;
                break;
            default:
                head.GetComponent<SpriteRenderer>().sprite = blueHead;
                torso.GetComponent<SpriteRenderer>().sprite = blueTorso;
                legs.GetComponent<SpriteRenderer>().sprite = blueLegs;
                playerTeam = 0;
                break;
        }
    }

    public void Ready() {
        GameObject button = transform.Find("Ready Button").gameObject;
        Image image = button.GetComponent<Image>();
        Text text = button.transform.Find("Text").GetComponent<Text>();

        Button teamButton = transform.Find("Change Team Button").gameObject.GetComponent<Button>();

        if (ready) {
            Color32 color = new Color32(158, 158, 158, 255);
            image.color = color;

            text.text = "NOT READY";
            teamButton.interactable = true;
        }
        else {
            Color32 color = new Color32(0, 216, 0, 255);
            image.color = color;

            text.text = "READY";
            teamButton.interactable = false;
        }

        ready = !ready;
    }
}
