using UnityEngine;
using UnityEngine.UI;

public class King : BasePiece
{
    private Tower mLeftTower = null;
    private Tower mRightTower = null;

    // Valor assignat per matar aquesta peça
    public int price = 9;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviments
        mMovement = new Vector3Int(1, 1, 1);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("K_Piece");
    }

    // Funció de la mort del rei. Final de la partida
    public override void Kill()
    {
        base.Kill();

        // S'actualitza el valor del final de la partida
        mPieceManager.isEndGame = true;
    }

    // Comprovació del possible path
    // El rei té dos moviments: el base i l'enderroc. A més l'enderroc es divideix en curt o llarg en funció de la torre amb la que es faci
    protected override void CheckPathing()
    {
        // Càlcul normal del path
        base.CheckPathing();

        // Comprobació de la torre de la dreta
        mRightTower = GetTower(1, 3);

        // Comprobació de la torre de l'esquerra
        mLeftTower = GetTower(-1, 4);
            
    }

    // Funció per moure el rei
    protected override void Move()
    {
        // Moviemnt base
        base.Move();
 
        // Enderroc amb la torre dreta
        if (CanCastling(mRightTower))
            mRightTower.Castling();

        // Enderroc amb la torre esquerra
        if (CanCastling(mLeftTower))
            mLeftTower.Castling();
    }

    // Comprovació de si es pot fer l'enderroc
    private bool CanCastling(Tower tower)
    {

        // Si no hi ha torre no es pot
        if (tower == null)
        {
            return false;
        }

        // Si la cel·la on la torre pot fer l'enderroc no és la actual, no es pot fer
        if (tower.mCastlingTriggerCell != mCurrentCell)
        {
            return false;
        }

        // Si la torre no és del mateix color o si ja a mogut abans, no es pot fer
        if (tower.mColor != mColor || !tower.mIsFirstMove)
        {
            return false;
        }

        // Si tot és correcte, es pot fer l'enderroc
        return true;
    }

    // Funció per agafar la informació de la torre
    private Tower GetTower(int direction, int count)
    {
        // Si el rei ha mogut
        if (!mIsFirstMove)
        {
            return null;
        }

        // Es guarda la posició actual en variable auxiliars
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // Es comprova que les cel·les fins la torre estan buides
        for (int i = 1; i < count; i++)
        {
            int offsetX = currentX + (i * direction);
            CellState cellState = mCurrentCell.mBoard.StateCell(offsetX, currentY, this);

            // si no ho estan, retorna null
            if (cellState != CellState.Free)
                return null;
        }

        // S'agafa la informació de la cel·la de la torre
        Cell towerCell = mCurrentCell.mBoard.mAllCells[currentX + (count * direction), currentY];

        // Es crea un objecte torre buit per tractar les dades
        Tower tower = null;

        // Es comprova que la cel·la conté una torre
        if (towerCell.mCurrentPiece is Tower)
            // Si és així, es guarda la informació en l'objecte Torre creat
            tower = (Tower)towerCell.mCurrentPiece;

        // Si s'ha agafat una torre vàlida, s'afegeix la cel·la on es farà l'enderroc a la llista de cel·les possibles per al path
        if (tower != null)
            mPossiblePathCells.Add(tower.mCastlingTriggerCell);

        // Es retorna la informació de la torre per el CanCastling
        return tower;
    }
}