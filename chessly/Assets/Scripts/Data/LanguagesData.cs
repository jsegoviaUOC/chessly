using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguagesData
{
    public Menu menu;
    public InGame game;
}

[System.Serializable]
public class Menu
{
    public GeneralMenu general;
    public OptionsMenu options;
}

[System.Serializable]
public class OptionsMenu
{
    public string ReturnMenuButton;
    public string LangEngButton;
    public string LangCatButton;

    public string SelectLanguageText;
    public string SelectWhitePiecesColorText;
    public string SelectBlackPiecesColorText;
    public string SelectBoardColorText;

    public WhitePiecesColors whitePiecesColor;
    public BlackPiecesColors blackPiecesColor;
    public BoardColors boardColor;
}

[System.Serializable]
public class GeneralMenu
{
    public string OnePlayerText;
    public string ClassicNPButton;
    public string NonClassicNPButton;
    public string TwoPlayersText;
    public string ClassicButton;
    public string NonClassicButton;
    public string QuitButton;
    public string OptionsButton;
}

[System.Serializable]
public class WhitePiecesColors
{
    public string PW;
    public string IV;
    public string SB;
}

[System.Serializable]
public class BlackPiecesColors
{
    public string PB;
    public string EB;
    public string DR;
}

[System.Serializable]
public class BoardColors
{
    public string BW;
    public string WC;
    public string NE;
}

[System.Serializable]
public class InGame
{
    public GameButtons buttons;
    public GameInfo info;
}

[System.Serializable]
public class GameButtons
{
    public string OkButton;
    public string NewGameButton;
    public string ReturnButton;
}

[System.Serializable]
public class GameInfo
{
    public string WinWhiteText;
    public string WinBlackText;
    public string TurnWhiteText;
    public string TurnBlackText;
}