using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellEditor : MonoBehaviour
{
    // Imatge del borde de la cel·la
    public Image mBorderImage;

    // Vector de posició en el tauler
    public Vector2Int mBoardPosition = Vector2Int.zero;
    
    // Tauler
    public BoardEditor mBoard = null;
    public RectTransform mRectTransform = null;

    // Peça
    public BasePiece mCurrentPiece = null;

    // puntuació per decidir el moviment de la IA
    public int score = 0;

    // Funció d'inicialització
    public void Setup(Vector2Int newBoardPosition, BoardEditor newBoard)
    {
        // Inicialització del tauler i del vector de posicions
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }

}
