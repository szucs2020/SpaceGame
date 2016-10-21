using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncFlip : NetworkBehaviour {

    //local player for callbacks
    public Player player;
    private RectTransform health;

    //flip
	[SyncVar(hook="FlipHook")]
	private bool facingRight = false;

    void Start() {
        health = transform.FindChild("HealthCanvas").GetComponent<RectTransform>();
    }

    [Command]
	public void CmdSyncFlip(bool direction){

        float scale;
		facingRight = direction;
		Vector3 temp = transform.localScale;

		if (facingRight){
			temp.x = -1;
            scale = -1;
        } else{
			temp.x = 1;
            scale = 1;
        }
        transform.localScale = temp;
        health.localScale = new Vector3(scale, health.localScale.y, health.localScale.z);
    }

	void FlipHook(bool direction){

        float scale;
        facingRight = direction;
		Vector3 temp = transform.localScale;

		if (facingRight){
			temp.x = -1;
            scale = -1;
        } else {
			temp.x = 1;
            scale = 1;
        }
		transform.localScale = temp;
        health.localScale = new Vector3(scale, health.localScale.y, health.localScale.z);
    }

	public bool getFacingRight() {
		return facingRight;
	}
}