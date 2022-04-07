using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public GameObject tile;
    public GameObject tileBroken;
    public GameObject tileGone;
    
    public GameObject player;
    public GameObject shield;
    public GameObject wall;
    public GameObject column;
    public GameObject gameLogic;
    
    void Start() {
        initBoard();
    }

    void Update() {
        if (player.GetComponent<Player>()._isPlayerMoving) {
            foreach(Transform go in transform) {
                if ((int) player.transform.position.x == (int) go.position.x &&
                    (int) player.transform.position.z == (int) go.position.z) {
                    if (go.gameObject.tag == "tileBroken") {
                        GameObject _tileGone = Instantiate(tileGone, go.transform.position, Quaternion.identity);
                        _tileGone.transform.SetParent(this.gameObject.transform);
                        _tileGone.transform.name = go.name;
                        _tileGone.transform.tag = "deadZone";
                        Destroy(go.gameObject);
                    } 
                }    
            }
        }
    }

    public void setSelectedTile(GameObject go) {
        if (go.tag == "tile" || go.tag == "tileBroken")
            go.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
    }
    
    public void setUnselectedTile(GameObject go) {
        if (go.tag == "tile" || go.tag == "tileBroken")
            go.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void setUnselectAllTiles() {
        foreach (Transform child in this.transform)
            if (child.tag == "tile" || child.tag == "tileBroken" || child.tag == "deadZone")
                child.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
  
    public void updateTile(GameObject go) {
        if (go.transform.tag == "tileBroken"){
            GameObject _tileGone = Instantiate(tileGone, go.transform.position, Quaternion.identity);
            _tileGone.transform.SetParent(this.gameObject.transform);
            _tileGone.transform.name = go.name;
            _tileGone.transform.tag = "deadZone";
            Destroy(go.gameObject);
        }
    }

    void loadLevel() {
        int currentLevel = GameObject.Find("_GameLogic").GetComponent<Game>().currentLevel;
        switch  (currentLevel) {
            case 0:
                Debug.Log("level 1 uploaded");
                //tiles
                gameLogic.GetComponent<Game>().board[1] = new[] {
                    0,0,0,0,1,0,0,1,0,0,
                    0,0,0,0,0,1,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,1,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,1,0,0,0,0,
                    0,0,0,1,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0
                };
                
                //walls and columns
                gameLogic.GetComponent<Game>().board[2] = new[] {
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,1,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,2,0,0,0,
                    2,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                };
                
                //player and enemies
                gameLogic.GetComponent<Game>().board[3] = new[] {
                    0,0,0,0,1,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                    0,0,0,0,0,0,0,0,0,0,
                };
                break;
            
            case 1:
                Debug.Log("level 2 uploaded");
                break;
            
            case 2:
                Debug.Log("level 3 uploaded");
                break;
            
            case 3:
                Debug.Log("level 4 uploaded");
                break;
            
            default:
                Debug.Log("no level uploaded");
                break;
        }
    }

    void setTiles() {
        for (int i = 0; i < gameLogic.GetComponent<Game>().board[1].Length; i++) {
             //Si gameobject es un tile
             if (gameLogic.GetComponent<Game>().board[1][i] == 0) {
                 GameObject _tile = Instantiate(tile, new Vector3(i % 10, 0, (i / 10)*-1), Quaternion.identity);
                 _tile.transform.SetParent(this.gameObject.transform);
                 _tile.transform.name = ""+i / 10+""+i % 10;
                 _tile.transform.tag = "tile";
             } else if (gameLogic.GetComponent<Game>().board[1][i] == 1) {
                 GameObject _tileBroken = Instantiate(tileBroken, new Vector3(i % 10, 0, (i / 10)*-1), Quaternion.identity);
                 _tileBroken.transform.SetParent(this.gameObject.transform);
                 _tileBroken.transform.name = ""+i / 10+""+i % 10;
                 _tileBroken.transform.tag = "tileBroken";
             } else if (gameLogic.GetComponent<Game>().board[1][i] == 2) {
                 GameObject _tileGone = Instantiate(tileGone, new Vector3(i % 10, 0, (i / 10)*-1), Quaternion.identity);
                 _tileGone.transform.SetParent(this.gameObject.transform);
                 _tileGone.transform.name = ""+i / 10+""+i % 10;
                 _tileGone.transform.tag = "deadZone";
             }
        }
    }

    void setElements() {
        foreach (Transform child in gameObject.transform) {
            //Si gameobject es un tile
            if (child.transform.gameObject.name.Length == 2) {
                //si 1er digit es 0
                if ((int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(0, 1)[0]) == 0) {
                    if (gameLogic.GetComponent<Game>().board[2][(int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0])] == 1)
                        Instantiate(wall, new Vector3((int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0]),0,(int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(0, 1)[0]) * -1), Quaternion.identity);
                    else if (gameLogic.GetComponent<Game>().board[2][(int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0])] == 2)  {
                        Instantiate(column, new Vector3((int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0]),0,(int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(0, 1)[0]) * -1), Quaternion.Euler(270, 0, 0));
                    }
                    //si 1er digit NO es 0
                } else  {
                    int i = (int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(0, 1)[0]) * 10 +
                            (int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0]);
                    if (gameLogic.GetComponent<Game>().board[2][i] == 1)
                        Instantiate(wall, new Vector3((int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0]),0,(int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(0, 1)[0]) * -1), Quaternion.identity);
                    else if (gameLogic.GetComponent<Game>().board[2][i] == 2)
                        Instantiate(column, new Vector3((int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(1, 1)[0]),0,(int) System.Char.GetNumericValue(child.transform.gameObject.name.ToCharArray(0, 1)[0]) * -1), Quaternion.Euler(270, 0, 0));
                }
            }                
        }
    }

    void setPlayer() {
        int xPlayer = 0;
        int zPlayer = 0;
        for (int i = 0; i < gameLogic.GetComponent<Game>().board[3].Length; i++)
            if (gameLogic.GetComponent<Game>().board[3][i] == 1)  {
                xPlayer = i % 10;
                zPlayer = (i / 10) * -1;
            }
        player.transform.position = new Vector3(xPlayer, 0.4f, zPlayer);
        shield.transform.position = new Vector3(xPlayer+0.5f, 0.4f, zPlayer);
    }
    
    void setEnemies(){}
    
    void initBoard() {
        //Init array
        gameLogic.GetComponent<Game>().board = new int[10][];
        for(int x = 0; x < gameLogic.GetComponent<Game>().board.Length; x++) { 
            gameLogic.GetComponent<Game>().board[x] = new int[100];
            for (int y = 0; y < gameLogic.GetComponent<Game>().board[x].Length; y++)
                gameLogic.GetComponent<Game>().board[x][y] = 0;
        }
        
        //Carrega de tots els elements (totes les capes de l'array board
        loadLevel();
        
        //pintat
        setTiles();
        setElements();
        setPlayer();
        setEnemies();
    }
    
    
}