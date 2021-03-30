using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    // Imatge del borde de la cel·la
    public Image mBorderImage;

    // Vector de posició en el tauler
    public Vector2Int mBoardPosition = Vector2Int.zero;
    
    // Tauler
    public Board mBoard = null;
    public RectTransform mRectTransform = null;

    // Peça
    public BasePiece mCurrentPiece = null;

    // Funció d'inicialització
    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        // Inicialització del tauler i del vector de posicions
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }

    // Funció per eliminar una peça
    public void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            mCurrentPiece.Kill();
        }
    }

}
