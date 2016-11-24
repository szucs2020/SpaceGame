﻿/*
 * Network.cs
 * Authors: Nigel. With help from Christian with the networking code
 * Description: This class handles the pregame lobby UI changes including the player colour and ready system.
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Lobby : MonoBehaviour {
    public Sprite blue;
    public Sprite red;
    public Sprite yellow;
    public Sprite green;

    //public LobbyPlayer player;
    public LobbyPlayer[] players = new LobbyPlayer[4];

    public void ChangeTeam(int p) {
        players[p].CmdChangeTeam();
    }

    // Use this for initialization
    void Start() {
        
    }

    void OnEnable() {
        StartCoroutine("InitSelect");
    }

    public IEnumerator InitSelect() {
        GameObject panel = transform.Find("Game Options").gameObject;
        Scrollbar type = panel.transform.Find("Game Type Scrollbar").gameObject.GetComponent<Scrollbar>();

        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(type.gameObject);
    }

    public void UpdateTeam(int pt, int p) {
        GameObject panel = transform.Find(p.ToString()).gameObject;
        GameObject player = panel.transform.Find("Player").gameObject;

        switch (pt) {
            case 0:
                player.GetComponent<SpriteRenderer>().sprite = blue;
                break;
            case 1:
                player.GetComponent<SpriteRenderer>().sprite = red;
                break;
            case 2:
                player.GetComponent<SpriteRenderer>().sprite = yellow;
                break;
            case 3:
                player.GetComponent<SpriteRenderer>().sprite = green;
                break;
        }
    }

	public void UpdateName(string name, int p){
		Text nameText = transform.Find(p.ToString()).transform.Find("Name").GetComponent<Text>();
		nameText.text = name;
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
        Scrollbar type = panel.transform.Find("Game Type Scrollbar").gameObject.GetComponent<Scrollbar>();
        Scrollbar lives = panel.transform.Find("Lives Scrollbar").gameObject.GetComponent<Scrollbar>();
        Scrollbar time = panel.transform.Find("Time Scrollbar").gameObject.GetComponent<Scrollbar>();
        Text typeText = panel.transform.Find("Game Type Value").gameObject.GetComponent<Text>();
        GameSettings settings = FindObjectOfType<GameSettings>();
    
        //Retrieve the value and set the game type accordingly, enable disable fields as necessary.
        if (type.value == 0) {
            settings.gameType = GameSettings.GameType.Survival;
            typeText.text = "SURVIVAL";
            lives.value = 0.3f;
            time.value = 0f;
        }
        else {
            settings.gameType = GameSettings.GameType.Time;
            typeText.text = "TIME";
            lives.value = 0f;
            time.value = 0.2f;
        }
    }
        
    public void ChangeLives() {
        GameObject panel = transform.Find("Game Options").gameObject;
        Scrollbar type = panel.transform.Find("Game Type Scrollbar").gameObject.GetComponent<Scrollbar>();
        Scrollbar lives = panel.transform.Find("Lives Scrollbar").gameObject.GetComponent<Scrollbar>();
        Text liveText = panel.transform.Find("Lives Value").gameObject.GetComponent<Text>();
        GameSettings settings = FindObjectOfType<GameSettings>();

        int value = (int)(lives.value * 10);

        //Retrieve the value and set the game type accordingly, enable disable fields as necessary.
        settings.numLives = value;
        if (value == 0) {
            liveText.text = "INFINITE";
        } 
        else {
            liveText.text = "" + value;
        }
    }
        
    public void ChangeTime() {
        GameObject panel = transform.Find("Game Options").gameObject;
        Scrollbar time = panel.transform.Find("Time Scrollbar").gameObject.GetComponent<Scrollbar>();
        Text timeText = panel.transform.Find("Time Value").gameObject.GetComponent<Text>();
        GameSettings settings = FindObjectOfType<GameSettings>();

        int value = (int)(time.value * 10);

        //Retrieve the value and set the game type accordingly, enable disable fields as necessary.
        settings.time = value * 60;
        if (value == 0) {
            timeText.text = "INFINITE";
        } 
        else {
            timeText.text = value + ":00";
        }
    }
        
    public void ChangeAIPlayers() {
        GameObject panel = transform.Find("Game Options").gameObject;
        Scrollbar AI = panel.transform.Find("AI Players Scrollbar").gameObject.GetComponent<Scrollbar>();
        Text AIText = panel.transform.Find("AI Players Value").gameObject.GetComponent<Text>();
        GameSettings settings = FindObjectOfType<GameSettings>();

        int value = (int)(AI.value * 10);

        if (value == 0) {
            settings.NumberOfAIPlayers = 0;
            AIText.text = "0";
        }
        else if (value == 3) {
            settings.NumberOfAIPlayers = 1;
            AIText.text = "1";
        }
        else if (value == 6) {
            settings.NumberOfAIPlayers = 2;
            AIText.text = "2";
        }
        else if (value == 10) {
            settings.NumberOfAIPlayers = 3;
            AIText.text = "3";
        }

    }
}
