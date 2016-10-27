/*
 * Network.cs
 * Authors: Nigel. With help from Christian with the networking code
 * Description: This class handles the pregame lobby UI changes including the player colour and ready system.
 */
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

    //public LobbyPlayer player;
    public LobbyPlayer[] players = new LobbyPlayer[4];

    public void ChangeTeam(int p) {
        players[p].CmdChangeTeam();
    }

    public void UpdateTeam(int pt, int p) {
        GameObject panel = transform.Find(p.ToString()).gameObject;
        GameObject head = panel.transform.Find("Head").gameObject;
        GameObject torso = panel.transform.Find("Torso").gameObject;
        GameObject legs = panel.transform.Find("Legs").gameObject;

        switch (pt) {
            case 0:
                head.GetComponent<SpriteRenderer>().sprite = blueHead;
                torso.GetComponent<SpriteRenderer>().sprite = blueTorso;
                legs.GetComponent<SpriteRenderer>().sprite = blueLegs;
                break;
            case 1:
                head.GetComponent<SpriteRenderer>().sprite = redHead;
                torso.GetComponent<SpriteRenderer>().sprite = redTorso;
                legs.GetComponent<SpriteRenderer>().sprite = redLegs;
                break;
            case 2:
                head.GetComponent<SpriteRenderer>().sprite = yellowHead;
                torso.GetComponent<SpriteRenderer>().sprite = yellowTorso;
                legs.GetComponent<SpriteRenderer>().sprite = yellowLegs;
                break;
            case 3:
                head.GetComponent<SpriteRenderer>().sprite = greenHead;
                torso.GetComponent<SpriteRenderer>().sprite = greenTorso;
                legs.GetComponent<SpriteRenderer>().sprite = greenLegs;
                break;
        }
    }

    public void setPlayer(LobbyPlayer p) {
        players[p.slot] = p;
        GameObject.Find("GameLobby").transform.Find(p.slot.ToString()).transform.Find("Ready").GetComponent<Button>().gameObject.SetActive(true);
        GameObject.Find("GameLobby").transform.Find(p.slot.ToString()).transform.Find("Change Team").GetComponent<Button>().gameObject.SetActive(true);
    }

    public void onClickReady(int p) {
        if (players[p].readyToBegin) {
            players[p].SendNotReadyToBeginMessage();
            ChangeReadyColour(false, p);
        } else {
            players[p].SendReadyToBeginMessage();
            ChangeReadyColour(true, p);
        }
    }

    private void ChangeReadyColour(bool ready, int p) {
        GameObject panel = transform.Find(p.ToString()).gameObject;
        GameObject button = panel.transform.Find("Ready").gameObject;
        Image image = button.GetComponent<Image>();
        Text text = button.transform.Find("Text").GetComponent<Text>();

        Button teamButton = panel.transform.Find("Change Team").gameObject.GetComponent<Button>();

        if (ready) {
            Color32 color = new Color32(0, 216, 0, 255);
            image.color = color;

            text.text = "READY";
            teamButton.interactable = false;
        } else {
            Color32 color = new Color32(158, 158, 158, 255);
            image.color = color;

            text.text = "NOT READY";
            teamButton.interactable = true;
        }
    }

    public void ChangeGameType() {
        GameObject panel = transform.Find("Game Options").gameObject;
        Dropdown type = panel.transform.Find("Game Type Dropdown").gameObject.GetComponent<Dropdown>();
        GameSettings settings = FindObjectOfType<GameSettings>();
        InputField livesInput = panel.transform.Find("Lives Input").gameObject.GetComponent<InputField>();
        InputField timeInput = panel.transform.Find("Time Input").gameObject.GetComponent<InputField>();

        //Retrieve the value and set the game type accordingly, enable disable fields as necessary.
        switch (type.value) {    
            case 0:
                settings.gameType = GameSettings.GameType.Survival;
                livesInput.enabled = true;
                timeInput.interactable = false;
                break;
            case 1:
                settings.gameType = GameSettings.GameType.Time;
                livesInput.enabled = false;
                timeInput.interactable = true;
                break;
        }
    }

    public void ChangeLives() {
        GameObject panel = transform.Find("Game Options").gameObject;
        InputField livesInput = panel.transform.Find("Lives Input").gameObject.GetComponent<InputField>();
        GameSettings settings = FindObjectOfType<GameSettings>();
        int lives = 0;

        //Retrieve the value.
        if (livesInput.text != "") {
            lives = int.Parse(livesInput.text);
        }

        //Check that the value entered is not 0 or negative, otherwise, default to 1.
        if (lives <= 0) {
            lives = 1;
        }

        settings.numLives = lives;
    }

    public void ChangeTime() {
        GameObject panel = transform.Find("Game Options").gameObject;
        InputField timeInput = panel.transform.Find("Time Input").gameObject.GetComponent<InputField>();
        GameSettings settings = FindObjectOfType<GameSettings>();
        int time = 0;

        //Retrieve the value.
        if (timeInput.text != "") {
            time = int.Parse(timeInput.text);
        }

        //Check that the value entered is not 0 or negative, otherwise, default to 1.
        if (time <= 0) {
            time = 1;
        }

        settings.time = time;
    }

    public void ChangeAIPlayers() {
        GameObject panel = transform.Find("Game Options").gameObject;
        InputField aiInput = panel.transform.Find("AI Players Input").gameObject.GetComponent<InputField>();
        GameSettings settings = FindObjectOfType<GameSettings>();
        int players = 0;

        //Retrieve the value.
        if (aiInput.text != "") {
            players = int.Parse(aiInput.text);
        }

        //Check that the value entered is not negative, otherwise, default to 0.
        if (players <= 0) {
            players = 0;
        } else if (players > 3) { //Apply a cap of 3 AI players. TODO: Check how many human players are present and make the cap 4 minus that.
            players = 3;
        }

        settings.NumberOfAIPlayers = players;
    }
}
