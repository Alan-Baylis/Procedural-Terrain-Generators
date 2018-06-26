using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMove : MonoBehaviour {

    public Transform Player;

    public void LateUpdate()
    {
        this.transform.position = new Vector3(Player.position.x, 0, Player.position.z);
    }
}
