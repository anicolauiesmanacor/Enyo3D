using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public GameObject player;
    public GameObject btnAct1;
    public GameObject btnAct2;
    public GameObject btnAct3;
    public GameObject btnAct4;
    public GameObject btnAct5;
    
    public void ButtonHUDPressed(int i) {
        btnAct1.GetComponent<Image>().color = btnAct2.GetComponent<Image>().color = btnAct3.GetComponent<Image>().color = 
        btnAct4.GetComponent<Image>().color = btnAct5.GetComponent<Image>().color = Color.white;
        player.GetComponent<Player>().setShield(false);

        switch (i) {
            case 0:
                    player.GetComponent<Player>().isDashing = player.GetComponent<Player>().isHooking = player.GetComponent<Player>().isThrowing = player.GetComponent<Player>().isLeaping = player.GetComponent<Player>().isOnGuard = false;
                    break;
            case 1:
                if (player.GetComponent<Player>().isDashing) {
                    player.GetComponent<Player>().isDashing = false;
                    btnAct1.GetComponent<Image>().color = Color.white;
                } else {
                    player.GetComponent<Player>().isDashing = true;
                    player.GetComponent<Player>().isHooking = player.GetComponent<Player>().isThrowing = player.GetComponent<Player>().isLeaping = player.GetComponent<Player>().isOnGuard = false;
                    btnAct1.GetComponent<Image>().color = Color.gray;    
                }
                break;
            case 2:
                if (player.GetComponent<Player>().isHooking) {
                    player.GetComponent<Player>().isHooking = false;
                    btnAct2.GetComponent<Image>().color = Color.white;
                } else {
                    player.GetComponent<Player>().isHooking = true;
                    player.GetComponent<Player>().isDashing = player.GetComponent<Player>().isThrowing = player.GetComponent<Player>().isLeaping = player.GetComponent<Player>().isOnGuard = false;
                    btnAct2.GetComponent<Image>().color = Color.gray;    
                }
                break;
            case 3:
                if (player.GetComponent<Player>().isThrowing) {
                    player.GetComponent<Player>().isThrowing = false;
                    btnAct3.GetComponent<Image>().color = Color.white;
                } else {
                    player.GetComponent<Player>().isThrowing = true;
                    player.GetComponent<Player>().isDashing = player.GetComponent<Player>().isHooking = player.GetComponent<Player>().isLeaping = player.GetComponent<Player>().isOnGuard = false;
                    btnAct3.GetComponent<Image>().color = Color.gray;
                }
                break;
            case 4:
                if (player.GetComponent<Player>().isLeaping) {
                    player.GetComponent<Player>().isLeaping = false;
                    btnAct4.GetComponent<Image>().color = Color.white;
                    player.GetComponent<Player>().unselectJumpableTiles();
                } else {
                    player.GetComponent<Player>().isLeaping = true;
                    player.GetComponent<Player>().isDashing = player.GetComponent<Player>().isHooking = player.GetComponent<Player>().isThrowing = player.GetComponent<Player>().isOnGuard = false;
                    btnAct4.GetComponent<Image>().color = Color.gray;
                    player.GetComponent<Player>().checkJumpableTiles();
                }
                break;
            case 5:
                if (player.GetComponent<Player>().isOnGuard) {
                    player.GetComponent<Player>().isOnGuard = false; 
                    player.GetComponent<Player>().isDashing = player.GetComponent<Player>().isHooking = player.GetComponent<Player>().isThrowing = player.GetComponent<Player>().isLeaping = false;
                    btnAct5.GetComponent<Image>().color = Color.white;
                    player.GetComponent<Player>().setShield(false);
                } else {
                    player.GetComponent<Player>().isOnGuard = true;
                    btnAct5.GetComponent<Image>().color = Color.gray;
                    player.GetComponent<Player>().setShield(true);
                }
                break;
        }
    } 
}
