using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    void OnMouseDown() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if ((GameObject.Find("_GameLogic").GetComponent<Game>().board[1][((int)(hit.transform.position.z*-10)+(int)(hit.transform.position.x))] == 0 ||
                 GameObject.Find("_GameLogic").GetComponent<Game>().board[1][((int)(hit.transform.position.z*-10)+(int)(hit.transform.position.x))] == 1) &&
                GameObject.Find("_GameLogic").GetComponent<Game>().board[2][((int)(hit.transform.position.z*-10)+(int)(hit.transform.position.x))] == 0) {
                if (GameObject.Find("Player").GetComponent<Player>().GetComponent<Player>().isLeaping) {
                    if (GameObject.Find("Player").GetComponent<Player>().GetComponent<Player>().jumpSpots.Contains(hit.transform.gameObject))
                        GameObject.Find("Player").GetComponent<Player>().GetComponent<Player>().jumpToTile(hit.transform.gameObject);
                }
            }
        }
    }
}
