using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Definició dels estats d'una cel·la
// Aquests estats són básics i estan pensats per el joc classic 
// Pendents de modificació
public enum CellStateEditor
{
    None,
    Friend,
    Enemy,
    Free,
    OutOfBounds
}

public class BoardEditor : MonoBehaviour
{
    // Prefab del la cel·la
    public GameObject mCellPrefab;

    // Limits del tauler (per defualt 8)
    public static int xLimit = 8;
    public static int yLimit = 8;

    // Configuració clàssica del tauler (per default 8 x 8)
    public CellEditor[,] mAllCells = new CellEditor[xLimit, yLimit];

    // Creació del tauler
    public void Create(int xAxis, int yAxis)
    {
        xLimit = xAxis;
        yLimit = yAxis;

        mAllCells = new CellEditor[xLimit, yLimit];

        for (int y = 0; y < yLimit; y++)
        {
            for (int x = 0; x < xLimit; x++)
            {
                // Creació d'una casella
                GameObject newCell = Instantiate(mCellPrefab, transform);

                // posició de la casella
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 80) + 50, (y * 80) + 50);

                // Inicialització i creació de l'objecte
                mAllCells[x, y] = newCell.GetComponent<CellEditor>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);

                // seguint el patró classic, canvi de color de les cel·les
                if ((x + y) % 2 == 0)
                {
                    mAllCells[x, y].GetComponent<Image>().color = GameButton.getColor(EditorManager.optionsData.colors.boardColor + "Dark");
                }
                else
                {
                    mAllCells[x, y].GetComponent<Image>().color = GameButton.getColor(EditorManager.optionsData.colors.boardColor + "Light");
                }
            }
        }
    }

}
