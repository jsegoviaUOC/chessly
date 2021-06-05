using UnityEngine;
using UnityEngine.UI;

public class Queen : BasePiece
{

    // Valor assignat per matar aquesta peça
    new public int price = 7;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject newSoundMove, GameObject newSoundDead)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, newSoundMove, newSoundDead);

        // Moviment de la peça
        mMovement = new Vector3Int(13, 13, 13);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Q_Piece");
    }
}
