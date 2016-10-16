using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PregameMenu : MonoBehaviour {
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

    public LobbyPlayer player;
    public int slot;

    public void ChangeTeam (string team) {
        GameObject head = transform.Find("Head").gameObject;
        GameObject torso = transform.Find("Torso").gameObject;
        GameObject legs = transform.Find("Legs").gameObject;

        if (team.ToUpper() == "BLUE") {
            head.GetComponent<SpriteRenderer>().sprite = blueHead;
            torso.GetComponent<SpriteRenderer>().sprite = blueTorso;
            legs.GetComponent<SpriteRenderer>().sprite = blueLegs;
        } else if (team.ToUpper() == "RED") {
            head.GetComponent<SpriteRenderer>().sprite = redHead;
            torso.GetComponent<SpriteRenderer>().sprite = redTorso;
            legs.GetComponent<SpriteRenderer>().sprite = redLegs;
        } else if (team.ToUpper() == "YELLOW") {
            head.GetComponent<SpriteRenderer>().sprite = yellowHead;
            torso.GetComponent<SpriteRenderer>().sprite = yellowTorso;
            legs.GetComponent<SpriteRenderer>().sprite = yellowLegs;
        } else if (team.ToUpper() == "GREEN") {
            head.GetComponent<SpriteRenderer>().sprite = greenHead;
            torso.GetComponent<SpriteRenderer>().sprite = greenTorso;
            legs.GetComponent<SpriteRenderer>().sprite = greenLegs;
        }
    }

    public void onClickReady() {
        player.SendReadyToBeginMessage();
    }

    public void setPlayer(LobbyPlayer p) {
        this.player = p;
        transform.Find("Ready").GetComponent<Button>().interactable = true;
    }

}
