using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class InputController : MonoBehaviour {

    Player player;

    void Start () {
        player = GetComponent<Player>();
    }

	void Update () {

        player.setMovementAxis(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        player.setbuttonPressedJump(Input.GetButtonDown("Jump"));
        player.setbuttonHeldJump(Input.GetButton("Jump"));
        player.setbuttonReleasedJump(Input.GetButtonUp("Jump"));

        player.setbuttonPressedShoot(Input.GetButtonDown("Shoot"));
        if (Input.GetButton("Shoot") || Input.GetAxis("Shoot") != 0) {
            player.setbuttonHeldShoot(true);
        } else {
            player.setbuttonHeldShoot(false);
        }
        
    }
}
