using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Definició dels estats d'una cel·la
// Aquests estats són básics i estan pensats per el joc classic 
// Pendents de modificació
public enum CellState
{
    None,
    Friend,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    // Prefab del la cel·la
    public GameObject mCellPrefab;

    // Limits del tauler (per defualt 8)
    public static int xLimit = 8;
    public static int yLimit = 8;

    // Configuració clàssica del tauler (per default 8 x 8)
    public Cell[,] mAllCells = new Cell[xLimit, yLimit];

    // Creació del tauler
    public void Create(int xAxis, int yAxis)
    {
        xLimit = xAxis;
        yLimit = yAxis;

        mAllCells = new Cell[xLimit, yLimit];

        for (int y = 0; y < yLimit; y++)
        {
            for (int x = 0; x < xLimit; x++)
            {
                // Creació d'una casella
                GameObject newCell = Instantiate(mCellPrefab, transform);

                // posició de la casella
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                // Inicialització i creació de l'objecte
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
                
                // seguint el patró classic, canvi de color de les cel·les
                if( (x + y) % 2 == 0)
                {
                    mAllCells[x, y].GetComponent<Image>().color = new Color32(193, 137, 93, 255);
                }
            }
        }
    }

    // Comprovació de l'estat de la cel·la
    public CellState StateCell(int targetX, int targetY, BasePiece checkingPiece)
    {
        // Es comprova di está dins dels dimensions del tauler 
        if (targetX < 0 || targetX > xLimit-1)
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > yLimit-1)
            return CellState.OutOfBounds;

        // S'agafa la cel·la objectiu del moviment
        Cell targetCell = mAllCells[targetX, targetY];

        // Si la cel·la té una peça
        if (targetCell.mCurrentPiece != null)
        {
            // Si és una peça del mateix color és amiga
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friend;

            // Si no és una peça del mateix color és enemiga
            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
                return CellState.Enemy;
        }

        // La resta de casos, la cel·la està lliure
        return CellState.Free;
    }
}
