using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    //DEBUG
    private bool _isBebug;
    public GameObject board;
    public GameObject gameLogic;

    //Shield
    private bool _isShieldThrown;
    
    //jump
    private bool _isJumping;
    private GameObject jumpTo;
    
    //Player movements
    private bool _movePlayer;
    private float _xInit, _zInit;
    private float _xActual, _zActual;
    private bool _moveHor;
    private bool _moveVer;
    private bool _moveRight;
    private bool _moveLeft;
    private bool _moveUp;
    private bool _moveDown;

    private List<GameObject> path;
    private int _iPath;
    
    public List<GameObject> jumpSpots;

    public bool _isPlayerMoving;
    private Vector3 _newPlayerPosition;
    
    private bool _isCollidingWithObstacle;
    private float _xObstacle, _zObstacle;
    
    //HUD
    public bool isDashing;
    public bool isHooking;
    public bool isThrowing;
    public bool isLeaping;
    public bool isOnGuard;

    void Start () {
        _isBebug = true;
        
        _moveHor = _moveVer = _movePlayer = false;
        _moveRight = _moveLeft = _moveUp = _moveDown = false;
        _xActual = _zActual = _xInit = _zInit = _xObstacle = _zObstacle = 0;
        _iPath = 0;
        isDashing = isHooking = isThrowing = isLeaping =isOnGuard = false;
        _isShieldThrown = false;
        path = new List<GameObject>();
        jumpSpots = new List<GameObject>();
    }

    void FixedUpdate() {
        if (isDashing) {
            if (_isPlayerMoving)
                movePlayer();
        } else if (isLeaping && _isJumping)
            jump();
        
        debugVars();
    }

    void jump() {
        if ((Mathf.Abs(this.transform.position.x - jumpTo.transform.position.x) >= 0.05) || (Mathf.Abs(this.transform.position.z - jumpTo.transform.position.z) >= 0.05)) {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(jumpTo.transform.position.x, 0.4f, jumpTo.transform.position.z), 0.1f);
            if ((Mathf.Abs(this.transform.position.x - jumpTo.transform.position.x) < 0.05) || (this.transform.position.z != _newPlayerPosition.z && (Mathf.Abs(this.transform.position.z - jumpTo.transform.position.z) < 0.05))) {
                this.transform.position = new Vector3(jumpTo.transform.position.x, 0.4f, jumpTo.transform.position.z);
            }
        } else {
            isLeaping = _isJumping = false;
            GameObject.Find("HUD").GetComponent<HUDController>().ButtonHUDPressed(0);
            board.GetComponent<Board>().updateTile(jumpTo);
            unselectJumpableTiles();            
        }
    }

    void movePlayer() {
        if (this.transform.position != _newPlayerPosition) {
            setShield(true);
            this.transform.position = Vector3.Lerp(this.transform.position, _newPlayerPosition, 0.1f);
            if (this.transform.position.x != _newPlayerPosition.x && (Mathf.Abs(this.transform.position.x - _newPlayerPosition.x) < 0.05) || 
                (this.transform.position.z != _newPlayerPosition.z && (Mathf.Abs(this.transform.position.z - _newPlayerPosition.z) < 0.05)))
                this.transform.position = _newPlayerPosition;
        } else if (_isPlayerMoving) {
            transform.position = new Vector3((int)Mathf.Round(this.transform.position.x), this.transform.position.y, (int)Mathf.Round(this.transform.position.z));
            _isPlayerMoving = false;
            setShield(false);
            resetPlayerMovementVars();
        }
    }

    void OnMouseDown() {
        if (isDashing) {
            startingDash();
        } else if (isOnGuard) {
            Debug.Log("");
        } else if (isHooking) {
            Debug.Log("");
        } else if (isLeaping) {
            Debug.Log("is leaping!!");
        } else if (isThrowing) {
            Debug.Log("");
        }
    }

    void OnMouseDrag() {
        if (isDashing) {
            makingDash();
        } else if (isOnGuard) {
            Debug.Log("");
        } else if (isHooking) {
            Debug.Log("");
        } else if (isLeaping) {
            Debug.Log("");
        } else if (isThrowing) {
            Debug.Log("");
        }
    }

    void OnMouseUp() {
        if (isDashing) {
            finishingDash();
        } else if (isOnGuard) {
            Debug.Log("");
        } else if (isHooking) {
            Debug.Log("");
        } else if (isLeaping) {
            Debug.Log("");
        } else if (isThrowing) {
            Debug.Log("");
        }
        board.GetComponent<Board>().setUnselectAllTiles();
        GameObject.Find("HUD").GetComponent<HUDController>().ButtonHUDPressed(0);
    }
    
    //DASH
    void startingDash() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            //newPosition = hit.point;
            if (hit.transform.name == "Player") {
                _newPlayerPosition = this.transform.position;
                _movePlayer = true;
                _xActual = _xInit = (int)hit.transform.position.x;
                _zActual = _zInit = (int)hit.transform.position.z;
                GameObject go = GameObject.Find("" + Mathf.Abs(_zInit) + _xInit);
                path.Insert(_iPath,go);
                _iPath++;
                //board.GetComponent<Board>().setSelectedTile(hit.transform.gameObject);
            }
        }
    }

    void makingDash() {
        if (_movePlayer) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.name == "Player") {
                    _xActual = _xInit;
                    _zActual = _zInit;
                    _moveDown = _moveUp = _moveLeft = _moveUp = _moveHor = _moveVer = false;
                }

                if (hit.transform.tag == "obstacle" || hit.transform.tag == "deadZone") {
                    _isCollidingWithObstacle = true;
                    _xObstacle = (int) hit.transform.position.x;
                    _zObstacle = (int) hit.transform.position.z;
                } 
                if (hit.transform.tag == "tile" || hit.transform.tag == "tileBroken") {
                    if (!_moveHor && !_moveVer) {
                        if (Mathf.Abs(hit.transform.position.x - _xInit) < 0.25 &&
                            Mathf.Abs(hit.transform.position.z - _zInit) >= 0.25) {
                            _moveVer = true;
                            if (hit.transform.position.z > _zInit) {
                                _moveUp = true;
                                rotatePlayer(180.0f);
                            } else {
                                _moveDown = true;
                                rotatePlayer(0);
                            }
                        } else if (Mathf.Abs(hit.transform.position.z - _zInit) < 0.25 &&
                                   Mathf.Abs(hit.transform.position.x - _xInit) >= 0.25)  {
                            _moveHor = true;
                            if (hit.transform.position.x > _xInit) {
                                _moveRight = true;
                                rotatePlayer(270.0f);
                            } else {
                                _moveLeft = true;
                                rotatePlayer(90.0f);
                            }
                        } else {
                            board.GetComponent<Board>().setSelectedTile(hit.transform.gameObject);
                        }
                        setShield(true);
                    }
                    
                    if (_moveHor && !_moveVer) {
                        if (_moveRight && hit.transform.position.x >= _xInit) {
                            if (!_isCollidingWithObstacle) {
                                if (hit.transform.position.x >= _xActual) {
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath,go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x+1));
                                    if (path.Contains(go)) {
                                        go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            } else {
                                if (hit.transform.position.x >= _xActual && hit.transform.position.x < _xObstacle) {
                                    _isCollidingWithObstacle = false;
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath, go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x-1));
                                    if (path.Contains(go)) {
                                        go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            }
                            //if (!_isCollidingWithObstacle)
                                _xActual = (int) hit.transform.position.x;
                        } else if (_moveLeft && hit.transform.position.x <= _xInit) {
                            if (!_isCollidingWithObstacle) {
                                if (hit.transform.position.x <= _xActual) {
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath,go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x-1));
                                    board.GetComponent<Board>().setUnselectedTile(go);
                                    if (path.Contains(go)) {
                                        go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            } else {
                                if (hit.transform.position.x <= _xActual && hit.transform.position.x > _xObstacle) {
                                    _isCollidingWithObstacle = false;
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath,go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    Debug.Log("Entra");
                                    GameObject go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x+1));
                                    board.GetComponent<Board>().setUnselectedTile(go);
                                    if (path.Contains(go)) {
                                        go = GameObject.Find("" +Mathf.Abs(_zInit)+(int)(hit.transform.position.x));
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            }
                            if (!_isCollidingWithObstacle) {
                                _xActual = (int) hit.transform.position.x;
                            }
                        }
                    } else if (!_moveHor && _moveVer) {
                        if (_moveUp && hit.transform.position.z >= _zInit) {
                            if (!_isCollidingWithObstacle) {
                                if (hit.transform.position.z >= _zActual) {
                                    GameObject go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath,go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +(int)((hit.transform.position.z+1)*-1)+_xInit);
                                    board.GetComponent<Board>().setUnselectedTile(go);
                                    if (path.Contains(go)) {
                                        go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            } else {
                                if (hit.transform.position.z <= _zActual && hit.transform.position.z < _zObstacle) {
                                    _isCollidingWithObstacle = false;
                                    GameObject go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath,go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +(int)((hit.transform.position.z-1)*-1)+_xInit);
                                    board.GetComponent<Board>().setUnselectedTile(go);
                                    if (path.Contains(go)) {
                                        go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            }
                            if (!_isCollidingWithObstacle) {
                                _zActual = (int) hit.transform.position.z;
                            }
                        } else if (_moveDown && hit.transform.position.z <= _zInit) {
                            if (!_isCollidingWithObstacle) {
                                if (hit.transform.position.z <= _zActual) {
                                    GameObject go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath,go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                        
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                    if (path.Contains(go)) {
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            } else {
                                if (hit.transform.position.z >= _zActual && hit.transform.position.z > _zObstacle) {
                                    _isCollidingWithObstacle = false;
                                    GameObject go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                    if (!path.Contains(go)) {
                                        path.Insert(_iPath, go);
                                        _iPath++;
                                        board.GetComponent<Board>().setSelectedTile(go);
                                    }
                                } else {
                                    GameObject go = GameObject.Find("" +(int)(hit.transform.position.z*-1)+_xInit);
                                    if (path.Contains(go)) {
                                        path.RemoveAt(_iPath-1);
                                        _iPath--;
                                        board.GetComponent<Board>().setUnselectedTile(go);
                                    }
                                }
                            }
                            if (!_isCollidingWithObstacle) {
                                _zActual = (int) hit.transform.position.z;
                            }
                        }
                    }
                }
            }   
        }
    }
    
    void finishingDash() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!(_xInit == _xActual && _zInit == _zActual)) {
            if (_moveHor) {
                if (_moveRight) {
                    //if(hit.transform.position.x > _xInit) {
                    if(_xActual > _xInit) {
                        if (!_isPlayerMoving) {
                            _isPlayerMoving = true;
                            if (!_isCollidingWithObstacle) {
                                _newPlayerPosition = new Vector3(_xActual, transform.position.y, _zInit);
                            } else
                                _newPlayerPosition = new Vector3(_xObstacle-1, transform.position.y, _zInit);
                        }
                    }
                } else if (_moveLeft) {
                    //if(hit.transform.position.x < _xInit) {
                    if(_xActual < _xInit) {
                        if (!_isPlayerMoving) {
                            _isPlayerMoving = true;
                            if (!_isCollidingWithObstacle)
                                _newPlayerPosition = new Vector3(_xActual, transform.position.y, _zInit);
                            else
                                _newPlayerPosition = new Vector3(_xObstacle+1, transform.position.y, _zInit);
                        }
                    } 
                } 
            } else if (_moveVer) {
                if (_moveUp) {
                    //if (hit.transform.position.z > _zInit) {
                    if (_zActual > _zInit) {
                        if (!_isPlayerMoving) {
                            _isPlayerMoving = true;
                            if (!_isCollidingWithObstacle)
                                _newPlayerPosition = new Vector3(_xInit, transform.position.y, _zActual);
                            else
                                _newPlayerPosition = new Vector3(_xInit, transform.position.y, _zObstacle-1);
                        }
                    } 
                } else if (_moveDown) {
                    //if (hit.transform.position.z < _zInit) {
                    if (_zActual < _zInit) {
                        if (!_isPlayerMoving) {
                            _isPlayerMoving = true;
                            if (!_isCollidingWithObstacle)
                                _newPlayerPosition = new Vector3(_xInit, transform.position.y, _zActual);
                            else
                                _newPlayerPosition = new Vector3(_xInit, transform.position.y, _zObstacle+1);
                        }
                    } 
                }
            }
        } else {
            resetPlayerMovementVars();
        }
    }

    void resetPlayerMovementVars() {
        _moveHor = _moveVer = _movePlayer = _moveRight = _moveLeft = _moveUp = _moveDown = _isCollidingWithObstacle = _isPlayerMoving = false;
        _xInit = _zInit = _xActual = _zActual = _xObstacle =_zObstacle =0;
        path = new List<GameObject>();
        _iPath = 0;
    }

    void rotatePlayer(float f) {
        Vector3 temp = this.transform.localRotation.eulerAngles;
        temp.y = f;
        this.transform.localRotation = Quaternion.Euler(temp);
    }
    
    
    //DASH & ONGUARD
    public void setShield(bool shieldUp) {
        Transform shield = transform.Find("Shield");
        if (shieldUp)  {
            Vector3 temp = shield.transform.localRotation .eulerAngles;
            temp.y = shield.transform.localRotation .y - 90.0f;
            shield.transform.localRotation  = Quaternion.Euler(temp);
            
            shield.transform.localPosition = new Vector3(0, 0, -0.667f);
        } else {
            Vector3 temp = shield.transform.localRotation .eulerAngles;
            temp.y = shield.transform.localRotation .y;
            shield.transform.localRotation  = Quaternion.Euler(temp);
            
            shield.transform.localPosition = new Vector3(0.667f, 0, 0);
        }
    }
    
     //STUNT LEAP (JUMP)
     public void checkJumpableTiles() {
         int xPlayer = (int) this.transform.position.x;
         int zPlayer = (int) this.transform.position.z;
         int xJump = 0;
         int zJump = 0;
         for (int i = 0; i < 8; i++) {
             switch (i) {
                 case 0:
                     zJump = Mathf.Abs(zPlayer - 1);
                     xJump = xPlayer - 1;
                     break;
                 case 1:
                     zJump = Mathf.Abs(zPlayer - 1);
                     xJump = xPlayer;
                     break;
                 case 2:
                     zJump = Mathf.Abs(zPlayer - 1);
                     xJump = xPlayer+1;
                     break;
                 case 3:
                     zJump = Mathf.Abs(zPlayer);
                     xJump = xPlayer-1;
                     break;
                 case 4:
                     zJump = Mathf.Abs(zPlayer);
                     xJump = xPlayer+1;
                     break;
                 case 5:
                     zJump = Mathf.Abs(zPlayer+1);
                     xJump = xPlayer-1;
                     break;
                 case 6:
                     zJump = Mathf.Abs(zPlayer+1);
                     xJump = xPlayer;
                     break;
                 case 7:
                     zJump = Mathf.Abs(zPlayer+1);
                     xJump = xPlayer+1;
                     break;
             }
             if ((xJump >= 0 && xJump <= 9) && (zJump >= 0 && zJump <= 9)) {
                 if ((gameLogic.GetComponent<Game>().board[2][(zJump * 10) + xJump] == 0) &&
                     (gameLogic.GetComponent<Game>().board[3][(zJump * 10) + xJump] == 0)) {
                     GameObject go = GameObject.Find(""+zJump+xJump);
                     board.GetComponent<Board>().setSelectedTile(go);
                     jumpSpots.Add(go);
                 }
             }
             
         }
     }

    public void unselectJumpableTiles() {
        foreach (GameObject go in jumpSpots) {
            board.GetComponent<Board>().setUnselectedTile(go);
        }
        jumpSpots.Clear();
    }

    public void jumpToTile(GameObject go) {
        //setting update jump in update
        _isJumping = true;
        jumpTo = go;
        
        //update array
        gameLogic.GetComponent<Game>().board[3][(int) ((this.transform.position.z * -10)+this.transform.position.x)] = 0;
        gameLogic.GetComponent<Game>().board[3][(int) ((this.transform.position.z * -10)+this.transform.position.x)] = 1;
    }

    //DEBUG
     void debugVars() {
         if (_isBebug) {
            GameObject.Find("xInit").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("xInit").transform.GetComponent<Text>().text = "xInit: "+_xInit;
            
            GameObject.Find("zInit").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("zInit").transform.GetComponent<Text>().text = "zInit: "+_zInit;
            
            GameObject.Find("xActual").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("xActual").transform.GetComponent<Text>().text = "xActual: "+_xActual;

            GameObject.Find("zActual").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("zActual").transform.GetComponent<Text>().text = "zActual: "+_zActual;

            GameObject.Find("xObstacle").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("xObstacle").transform.GetComponent<Text>().text = "xObstacle: "+_xObstacle;

            GameObject.Find("zObstacle").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("zObstacle").transform.GetComponent<Text>().text = "zObstacle: "+_zObstacle;

            GameObject.Find("isCollidingWithObstacle").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("isCollidingWithObstacle").transform.GetComponent<Text>().text = "_isCollidingWithObstacle: "+_isCollidingWithObstacle;

            GameObject.Find("moveHor").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("moveHor").transform.GetComponent<Text>().text = "moveHor: "+_moveHor;

            GameObject.Find("moveVer").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("moveVer").transform.GetComponent<Text>().text = "moveVer: "+_moveVer;

            GameObject.Find("left").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("left").transform.GetComponent<Text>().text = "Left: "+_moveLeft;

            GameObject.Find("right").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("right").transform.GetComponent<Text>().text = "Right: "+_moveRight;

            GameObject.Find("up").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("up").transform.GetComponent<Text>().text = "Up: "+_moveUp;

            GameObject.Find("down").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("down").transform.GetComponent<Text>().text = "Down: "+_moveDown;

            GameObject.Find("isPlayerMoving").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("isPlayerMoving").transform.GetComponent<Text>().text = "_isPlayerMoving: "+_isPlayerMoving;

            GameObject.Find("movePlayer").transform.GetComponent<Text>().enabled = true;
            GameObject.Find("movePlayer").transform.GetComponent<Text>().text = "movePlayer: "+_movePlayer;
         }
        
    }
}