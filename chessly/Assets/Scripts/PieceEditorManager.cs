using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PieceEditorManager : MonoBehaviour
{
    // Control per saber si ha acabat la partida
    public bool isEndGame = false;

    // Prefab de les peces
    public GameObject mPieceEditorPrefab;
    
    public void Setup(BoardEditor board)
    {
        // Llistat de peces incials de cada color
        for (int i = 0; i < BoardEditor.xLimit; i++)
        {
            for (int j = 0; j < BoardEditor.yLimit; j++)
            {
                // Es crea la peça
                CellActionPiece newCell = CreateActionCell(typeof(CellVoid));
                
                // S'inicialitza la peça amb el seu color i l'equip al que pertany
                newCell.Setup(new Color32(255, 255, 255, 0), this);

                newCell.Place(board.mAllCells[i, j]);
            }
        }
        
    }

    // Funció per crear una peça
    private CellActionPiece CreateActionCell(Type pieceType)
    {
        // Es genera una nova peça a partir el prefab
        GameObject newCellObject = Instantiate(mPieceEditorPrefab);
        newCellObject.transform.SetParent(transform);

        // S'aplica l'escala i rotació del objete
        newCellObject.transform.localScale = new Vector3(1, 1, 1);
        newCellObject.transform.localRotation = Quaternion.identity;

        // Es guarda la peça creada
        CellActionPiece newCell = (CellActionPiece)newCellObject.AddComponent(pieceType);

        //retorn de la peça a la funció anterior
        return newCell;
    }

}
