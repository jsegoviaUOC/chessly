using UnityEngine;
using UnityEngine.UI;

public class CellVoid : CellActionPiece
{
    public override void Setup( Color32 newSpriteColor, PieceEditorManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newSpriteColor, newPieceManager);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Void");
    }
}
