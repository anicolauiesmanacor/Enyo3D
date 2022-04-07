using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour  {
    public int currentLevel;
    /*Capa 0
        board[0] selected tiles  -> 0:no selected   1:selected
        board[1] tiles ->  0:normal  1:broken  2:destroyed
        board[2] walls & columns ->  0:nothing  1:wall  2:column
        board[3] player & enemies ->  0:empty  1:player  2:enemyType1  3:enemyType2 ....
    */
    public int[][] board;
}
