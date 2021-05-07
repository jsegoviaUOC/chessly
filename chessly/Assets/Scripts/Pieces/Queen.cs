using UnityEngine;
using UnityEngine.UI;

public class Queen : BasePiece
{

    // Valor assignat per matar aquesta peça
    public int price = 8;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviment de la peça
        mMovement = new Vector3Int(13, 13, 13);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Q_Piece");
    }
}
