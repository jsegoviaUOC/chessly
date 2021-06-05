using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguagesData
{
    public Menu menu;
    public InGame game;
    public InEditor editor;
    public PiecesName pieces;
    public ColorName colors;
}

[System.Serializable]
public class Menu
{
    public GeneralMenu general;
    public OptionsMenu options;
    public LoginMenu login;
}

[System.Serializable]
public class LoginMenu
{
    public string LabelUsername;
    public string PlaceholderUsername;
    public string LabelPassword;
    public string PlaceholderPassword;
    public string FailLogin;
    public string SuccessLogin;
    public string FailCreateUser;
    public string NullValues;
    public string NotGame;

    public LoginButtons buttons;
    public Statistics statistics;
}

[System.Serializable]
public class LoginButtons
{
    public string SignInButton;
    public string SignUpButton;
    public string SearchGameButton;
    public string CreateGameButton;
    public string StatisticsButton;
    public string LogOutButton;
    public string ClassicGameButton;
    public string CustomGameButton;
    public string BackToSelectButton;

}

[System.Serializable]
public class Statistics
{
    public string TotalWinsLabel;
    public string CreatedGamesLabel;
    public string SearchedGamesLabel;
    public string ColorPiecesLabel;
    public string TotalMovesLabel;
    public string TotalWinsValue;
    public string Classics;
    public string Customs;
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
    public string EditorNPButton;
    public string TwoPlayersText;
    public string ClassicButton;
    public string NonClassicButton;
    public string EditorButton;
    public string QuitButton;
    public string OptionsButton;
    public string LoginButton;
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
    public string WaitingText;
    public string WaitingLongText;
    public string VictoryOnlineText;
    public string LoseOnlineText;
}

[System.Serializable]
public class InEditor
{
    public EditorButtons buttons;
    public EditorInfo info;
    public EditorError error;
    public EditorConfBoard conf;
}

[System.Serializable]
public class EditorButtons
{
    public string GameButton;
    public string ReturnButton;
}

[System.Serializable]
public class EditorInfo
{
    public string PieceButtonsTitle;
    public string ColorButtonsTitle;
    public string WhitePlayerText;
    public string BlackPlayerText;
}

[System.Serializable]
public class EditorError
{
    public string TitleErrorBox;
    public string QuitErrorButton;
}

[System.Serializable]
public class EditorConfBoard
{
    public string TitleAxisBox;
    public string LabelAxisX;
    public string InformationAxisX;
    public string LabelAxisY;
    public string InformationAxisY;
    public string PlaceholderEditor;
    public string ApplyAxisButton;
    public string ErrorAxisBox;
}

[System.Serializable]
public class PiecesName
{
    public string Pawn;
    public string Tower;
    public string Bishop;
    public string Knigth;
    public string Queen;
    public string King;
}


[System.Serializable]
public class ColorName
{
    public string White;
    public string Black;
}