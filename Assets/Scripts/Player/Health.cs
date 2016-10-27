using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour {

    public float maxHealth;
    private Image HealthBar;

    [SyncVar(hook = "UpdateHealthBar")]
    private float health;
    [SyncVar]
    private float damage;

    public void Start() {
        HealthBar = transform.FindChild("HealthCanvas").FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
        health = maxHealth;
        UpdateHealthBar(health);
    }

    public void Damage(float damage) {

        //damage should only be done on the server
        if (!isServer) {
            return;
        }

        this.damage += damage;
        this.health -= damage;

        //player death
        if (this.health <= 0) {
            Die();
        }
    }

    public void Kill()
    {
        Die();
    }

    //updates character health bar
    private void UpdateHealthBar(float curHealth) {
        HealthBar.fillAmount = (curHealth / this.maxHealth);
    }

    //kill player and attempt respawn
    private void Die() {
        GetComponent<Player>().Die();
        GameObject.Find("GameSettings").GetComponent<GameController>().AttemptSpawnPlayer(this.connectionToClient, this.playerControllerId, GetComponent<Player>().playerSlot);
    }

}
