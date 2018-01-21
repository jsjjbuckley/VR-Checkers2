﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    public bool isRed;
    public bool isKing;
    public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2)
    {
        //If moving on top of another piece
        if (board[x2, y2] != null)
            return false;
        int deltaMove = Mathf.Abs(x1 - x2);
        int deltaMoveY = y2-y1;
        if(isRed || isKing)
        {

            if(deltaMove == 1)
            {
                if(deltaMoveY == 1)
                {
                    return true;
                }
            }
            else if(deltaMove == 2)
            {
                if(deltaMoveY == 2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isRed != isRed)
                    {
                        return true;
                    }
                }
            }
        }

        if (!isRed || isKing)
        {

            if (deltaMove == 1)
            {
                if (deltaMoveY == -1)
                {
                    return true;
                }
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == -2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isRed != isRed)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
