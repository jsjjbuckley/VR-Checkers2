using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkers : MonoBehaviour
{

    public Piece[,] pieces = new Piece[8, 8];
    public GameObject redCheckerPrefab;
    public GameObject blackCheckerPrefab;

    private bool isRedTurn;
    private bool isRed;

    private Vector3 boardOffset = new Vector3(-0.58f, 0.802f, 0.555f);
    private Vector3 pieceOffset = new Vector3(0.1f, 0, 0.1f);

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;
    private Piece selectedPiece;
    private List<Piece> forcedPieces;
    private bool hasKilled;

    private void Start()
    {
        GenerateBoard();
    }

    private void Update()
    {
        UpdateMouseOver();

        //If it is my turn
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if (selectedPiece != null)
            {
                UpdatePieceDrag(selectedPiece);
            }
            //Replace with collision and click of piece and control
            if (Input.GetMouseButtonDown(0))
            {
                SelectPiece(x, y);
            }
            if (Input.GetMouseButtonUp(0))
            {
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);
            }
        }
    }

    private void UpdateMouseOver()
    {
        //If it's my turn
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    private void UpdatePieceDrag(Piece p)
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            p.transform.position = hit.point + Vector3.up;
        }
    }
    private void SelectPiece(int x, int y)
    {
        //out of bounds
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
        {
            return;
        }
        Piece p = pieces[x, y];
        if (p != null && p.isRed == isRed)
        {
            if(forcedPieces.Count == 0)
            {
                selectedPiece = p;
                startDrag = mouseOver;
            }
            else
            {
                //Look for the piece under our forced pieces list
                if(forcedPieces.Find(fp => fp == p) == null)
                {
                    return;
                }
            }

            
        }
    }

    private void TryMove(int x1, int y1, int x2, int y2)
    {
        forcedPieces = ScanForPossibleMove();
        //Multiplayer Support
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];
        //Out of bounds
        if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8)
        {
            if (selectedPiece != null)
            {
                MovePiece(selectedPiece, x1, y1);
            }
            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }
        //Check if piece selected
        if (selectedPiece != null)
        {
            //If it has not moved
            if (endDrag == startDrag)
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }
            //Check if valid move
            if (selectedPiece.ValidMove(pieces, x1, y1, x2, y2))
            {
                //Did we kill anything
                //If this is a jump
                if (Mathf.Abs(x2 - x1) == 2)
                {
                    Piece p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null)
                    {
                        pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(p.gameObject);
                        hasKilled = true;
                    }
                }
                //Were we supposed to kill anything?
                if (forcedPieces.Count != 0 && !hasKilled)
                {
                    MovePiece(selectedPiece, x1, y1);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    return;
                }

                pieces[x2, y2] = selectedPiece;
                pieces[x1, y1] = null;
                MovePiece(selectedPiece, x2, y2);

                EndTurn();
            }
            else
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }
        }

    }
    private void EndTurn()
    {
        selectedPiece = null;
        startDrag = Vector2.zero;
        isRedTurn = !isRedTurn;
        hasKilled = false;
        CheckVictory();
    }
    private void CheckVictory()
    {

    }
    private List<Piece> ScanForPossibleMove()
    {
        forcedPieces = new List<Piece>();

        //Check all the pieces
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(pieces[i,j] != null && pieces[i,j].isRed == isRedTurn)
                {
                    if(pieces[i,j].IsForceToMove(pieces, i, j))
                    {
                        forcedPieces.Add(pieces[i, j]);
                    }
                }
            }
        }
        return forcedPieces;
    }
    private void GenerateBoard()
    {
        //Fill board with null by default
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                pieces[x, y] = null;
            }
        }

        //Generate Red team
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate our piece
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }

        //Generate Black team
        for (int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate their piece
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }
    }
    private void GeneratePiece(int x, int y)
    {
        bool isPieceRed = (y > 3) ? false : true;
        GameObject go = Instantiate((isPieceRed) ? redCheckerPrefab : blackCheckerPrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }
}