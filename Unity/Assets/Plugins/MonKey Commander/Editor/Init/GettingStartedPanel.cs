using UnityEngine;
using System.Collections;
using MonKey;
using MonKey.Editor;
using MonKey.Editor.Internal;
using MonKey.Extensions;
using MonKey.Settings.Internal;
using UnityEditor;

public class GettingStartedPanel : EditorWindow
{
    private const string changeLog = "https://drive.google.com/open?id=1CtPaxlxHAQdTxl_qYjMUQ6prUHoTRLvb";
    private static readonly string WasShownID = "MC_GettingStartedShown"+ MonKeyInitialization.MonKeyVersion;
    private static bool forceShow = false;

    public static void OpenPanelFirstTime()
    {
        MonkeyEditorUtils.OnCommandLoadingDone += ShowPanelFirstTime;     
    }

    private static void ShowPanelFirstTime()
    {
        // force selection size to change default settings
        MonKeySettings.Instance.MaxSortedSelectionSize = 200;
        MonKeyInternalSettings.Instance.MaxSortedSelectionSize = 200;

        if (forceShow)
            EditorPrefs.SetBool(WasShownID + Application.productName, false);

        if (!EditorPrefs.GetBool(WasShownID + Application.productName))
        {
            OpenStartupPanelMenu();
        }
    }

    [Command("Open User Guide","Opens the User Guide for MonKey",Category = "Help")]
    [MenuItem("Tools/MonKey Commander/Help/User Guide", false, 1)]
    public static void UserGuide()
    {
        Application.OpenURL("https://sites.google.com/view/monkey-user-guide/home");
    }

    [MenuItem("Tools/MonKey Commander/Help/Support", false, 1)]
    public static void Support()
    {
        Application.OpenURL("https://sites.google.com/view/monkey-commander/support");
    }


    [MenuItem("Tools/MonKey Commander/Social/Discord", false, 999)]
    public static void Discord()
    {
        Application.OpenURL("https://discord.gg/wRzsqxn");
    }

    [MenuItem("Tools/MonKey Commander/Social/Facebook", false, 999)]
    public static void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/MonKeyCommander/");
    }

    [MenuItem("Tools/MonKey Commander/Social/Twitter", false, 999)]
    public static void Twitter()
    {
        Application.OpenURL("https://twitter.com/BillSansky");
    }

    [Command("Help", "Need help with MonKey?" +
                     " This command opens the getting started panel to access all the useful links!",
        AlwaysShow = true, Order = 0,Category = "Help")]
    [MenuItem("Tools/MonKey Commander/🐒 Getting Started", false, 0)]
    public static void OpenStartupPanelMenu()
    {
        MonKeyInitialization.InitMonKey();
        OpenStartupPanel();
    }

    private static void OpenStartupPanel()
    {
        GettingStartedPanel panel = GetWindow<GettingStartedPanel>();
        InitGraphics(panel);
        panel.name = "Getting Started With MonKey";
        panel.titleContent = new GUIContent("Getting Started With MonKey", MonkeyStyle.Instance.MonkeyHead);
        panel.minSize = new Vector2(600, Mathf.Min(Screen.currentResolution.height, 862));
        panel.maxSize = new Vector2(600, Mathf.Min(Screen.currentResolution.height, 862));
        panel.ShowUtility();
    }

    private static void InitGraphics(GettingStartedPanel panel)
    {
        MonkeyStyle.Instance.PostInstanceCreation();
        panel.monKeyBanner = MonkeyStyle.Instance.GetTextureFromName("MonKeyBanner");
        panel.monKeyBannerStyle = new GUIStyle()
        {
            fixedWidth = 601,
            fixedHeight = 260,
            normal = { background = panel.monKeyBanner }
        };
        panel.welcomeTitleStyle = new GUIStyle()
        {
            fontSize = 28,
            stretchWidth = true,
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(10, 10, 10, 5),
            normal = { background = MonkeyStyle.Instance.WindowBackgroundTex }
        };

        panel.rateUsSectionStyle = new GUIStyle(panel.welcomeTitleStyle)
        {
            padding = new RectOffset(15, 10, 10, 5),
            normal = { background = MonkeyStyle.Instance.TopPanelGradientTexture }
        };

        panel.sectionTitleStyle = new GUIStyle()
        {
            fontSize = 20,
            stretchWidth = true,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(5, 5, 5, 0),
            margin = new RectOffset(30, 0, 0, 0)
        };

        panel.welcomeTextStyle = new GUIStyle(MonkeyStyle.Instance.CommandNameStyle)
        {
            fontSize = 12,
            stretchWidth = true,
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(5, 5, 5, 5),
            //  normal = { background = MonkeyStyle.Instance.SelectedResultFieldTex }
        };

        panel.sectionSubtitleStyle = new GUIStyle(panel.welcomeTextStyle)
        {
            alignment = TextAnchor.MiddleLeft,
            margin = new RectOffset(30, 0, 0, 0)
        };

        panel.sectionBackgroundStyle = new GUIStyle()
        {
            normal = { background = MonkeyStyle.ColorTexture(1, 1, ColorExt.HTMLColor("3a3a3a")) }
        };

        panel.titleColor = ColorExt.HTMLColor("cbcbcb");
        panel.sectionSubtitleColor = ColorExt.HTMLColor("ae8d4d");

        panel.buttonStyle = new GUIStyle()
        {
            fixedHeight = 50,
            fixedWidth = 250,
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            normal = { background = MonkeyStyle.ColorTexture(1, 1, ColorExt.HTMLColor("4d4d4d")) },
            hover = { background = MonkeyStyle.ColorTexture(1, 1,
                ColorExt.HTMLColor("4d4d4d").DarkerBrighter(-.1f)) },
        };

        panel.newVersionButtonStyle = new GUIStyle()

        {
            fixedHeight = 30,
            stretchWidth = true,
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            normal = { background = MonkeyStyle.ColorTexture(1, 1, ColorExt.HTMLColor("4d4d4d")) },
            hover = { background = MonkeyStyle.ColorTexture(1, 1,
                ColorExt.HTMLColor("4d4d4d").DarkerBrighter(-.05f)) },
        };


        panel.rateUsButtonBoxStyle = new GUIStyle()
        {
            margin = new RectOffset(1, 1, 1, 1),
            stretchWidth = true,
        };

        panel.newVersionButtonBoxStyle = new GUIStyle()
        {
            margin = new RectOffset(1, 1, 1, 1),
            padding = new RectOffset(5, 5, 0, 0),
            stretchWidth = true,

        };

        panel.buttonTop = new GUIStyle()
        {
            fixedHeight = 2,
            stretchWidth = true,
            normal = { background = MonkeyStyle.ColorTexture(1, 1, ColorExt.HTMLColor("616161")) },
            hover = { background = MonkeyStyle.ColorTexture(1, 1,
                 ColorExt.HTMLColor("616161").DarkerBrighter(-.1f))}
        };

        panel.buttonBottom = new GUIStyle()
        {
            fixedHeight = 2,
            stretchWidth = true,
            normal = { background = MonkeyStyle.ColorTexture(1, 1, ColorExt.HTMLColor("2d2d2d")) },
            hover = { background = MonkeyStyle.ColorTexture(1, 1,
                ColorExt.HTMLColor("2d2d2d").DarkerBrighter(-.1f))}
        };

        panel.buttonHighlightStyle = new GUIStyle()
        {
            margin = new RectOffset(10, 10, 10, 10),
            normal =
            {
                background = MonkeyStyle.ColorTexture(1, 1, ColorExt.HTMLColor("8FC225"))
            }
        };
    }

    private Color titleColor;
    private Color sectionSubtitleColor;

    private Texture2D monKeyBanner;

    private GUIStyle monKeyBannerStyle;

    private GUIStyle sectionBackgroundStyle;

    private GUIStyle welcomeTitleStyle;

    private GUIStyle rateUsSectionStyle;

    private GUIStyle welcomeTextStyle;

    private GUIStyle sectionTitleStyle;
    private GUIStyle sectionSubtitleStyle;

    private GUIStyle buttonStyle;
    private GUIStyle newVersionButtonStyle;
    private GUIStyle newVersionButtonBoxStyle;

    //  private GUIStyle rateUsButtonBoxStyle;

    private GUIStyle rateUsButtonBoxStyle;

    private GUIStyle buttonTop;
    private GUIStyle buttonBottom;

    private GUIStyle buttonHighlightStyle;

    private Vector2 scrollIndex;

    private readonly string mainTitleText = "A Wild MonKey Appeared!";

    private readonly string thankYouText =
        "Thank you for choosing us to assist you!" +
            " \n This should help you maximize your productivity:";

    private readonly string rateUsTitle = "RATE US!";

    private readonly string rateUs = "Liking MonKey? If you could rate us on the asset store, it would be great :)".Bold();

    private readonly string gettingStartedTitle = "Getting Started";

    private readonly string gettingStartedText = "Learn how to be efficient with MonKey, and how to understand everything";
    private readonly string userManualText = "CHECK THE MANUAL!";
    private readonly string top10Commands = "OUR TOP 10 COMMANDS";

    private readonly string commandsInDepthTitle = "Commands In Depth";
    private readonly string commandsInDepthText = "Want to know more advanced things?";
    private readonly string checkCommandsText = "FULL LIST OF COMMANDS";
    private readonly string writeYourOwnText = "HOW TO WRITE COMMANDS?";
    private readonly string needHelpTitle = "Need Help?";
    private readonly string needHelpText = "Something isn't right, you have a problem or you want to give us feedback?";
    private readonly string chatOnDiscordText = "CHAT ON DISCORD!";
    private readonly string supportText = "SUPPORT";
    private readonly string changeLogText = "Check what's new in version " + MonKeyInitialization.MonKeyVersion + "!";

    public void OnDestroy()
    {
        if (wasOverWindow)
        {
            EditorPrefs.SetBool(WasShownID + Application.productName, true);
        }
    }

    private bool wasOverWindow = false;

    public void OnGUI()
    {
        if (focusedWindow != this)
            Focus();

        if (mouseOverWindow == this)
            wasOverWindow = true;

        scrollIndex = GUILayout.BeginScrollView(scrollIndex, new GUIStyle() { stretchWidth = true });

        TitleSection();

        GettingStartedSection();
        InDepthSection();
        NeedHelpSection();

        RateUseSection();
        GUILayout.EndScrollView();
        Repaint();
    }

    private void TitleSection()
    {
        if (monKeyBannerStyle == null)
        {
            InitGraphics(this);
        }
        GUILayout.TextField("", monKeyBannerStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideSecondLineStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideLineStyle);

        GUILayout.BeginVertical(welcomeTitleStyle);

        GUILayout.Label(mainTitleText.Colored(titleColor),
            welcomeTitleStyle);

        GUILayout.Label(thankYouText.Colored(MonkeyStyle.Instance.CommandHelpTextColor),
            welcomeTextStyle);

        GUILayout.BeginHorizontal(new GUIStyle() { stretchWidth = true });

        GUILayout.BeginVertical(newVersionButtonBoxStyle);

        GUILayout.Label("", buttonTop);
        bool changlog = GUILayout.Button
            (changeLogText.Colored(titleColor).Bold(), newVersionButtonStyle);
        if (changlog)
            Application.OpenURL(
               changeLog);
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideSecondLineStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideLineStyle);
    }

    private void RateUseSection()
    {
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideSecondLineStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideLineStyle);
        GUILayout.BeginHorizontal(rateUsSectionStyle);
        GUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(0, 0, 10, 0) });
        GUILayout.Label("", MonkeyStyle.Instance.MonkeyLogoStyleHappy);
        GUILayout.EndVertical();
        GUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(5, 5, 5, 5) });

        GUILayout.Label(rateUs.Colored(MonkeyStyle.Instance.CommandHelpTextColor),
            welcomeTextStyle);

        GUILayout.BeginVertical(buttonHighlightStyle);
        GUILayout.BeginVertical(rateUsButtonBoxStyle);
        GUILayout.Label("", buttonTop);
        bool button = GUILayout.Button(rateUsTitle.Colored(titleColor).Bold()
            , newVersionButtonStyle);
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();
        GUILayout.EndVertical();

        if (button)
            Application.OpenURL(
                "https://assetstore.unity.com/packages/tools/utilities/monkey-commander-productivity-booster-119938");

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void GettingStartedSection()
    {
        GUILayout.BeginVertical(sectionBackgroundStyle);

        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine1Style);
        // GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine2Style);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine3Style);

        GUILayout.Label(gettingStartedTitle.Colored(MonkeyStyle.Instance.QuickNameTextColor),
            sectionTitleStyle);
        GUILayout.Label(gettingStartedText.Colored(sectionSubtitleColor),
            sectionSubtitleStyle);
        GUILayout.BeginHorizontal(welcomeTextStyle);
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(newVersionButtonBoxStyle);
        GUILayout.Label("", buttonTop);
        bool userManual = GUILayout.Button(userManualText.Colored(titleColor).Bold()
            , buttonStyle);
        if (userManual)
            Application.OpenURL(
                "https://sites.google.com/view/monkey-user-guide/home");
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(newVersionButtonBoxStyle);
        GUILayout.Label("", buttonTop);
        bool top10 = GUILayout.Button(top10Commands.Colored(titleColor).Bold()
            , buttonStyle);
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        if (top10)
            Application.OpenURL(
                "https://sites.google.com/view/monkey-user-guide/getting-started");
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideSecondLineStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideLineStyle);
        GUILayout.EndVertical();

    }


    private void InDepthSection()
    {
        GUILayout.BeginVertical(sectionBackgroundStyle);

        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine1Style);
        //    GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine2Style);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine3Style);

        GUILayout.Label(commandsInDepthTitle.Colored(MonkeyStyle.Instance.QuickNameTextColor),
            sectionTitleStyle);
        GUILayout.Label(commandsInDepthText.Colored(sectionSubtitleColor),
            sectionSubtitleStyle);

        GUILayout.BeginHorizontal(welcomeTextStyle);
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(newVersionButtonBoxStyle);
        GUILayout.Label("", buttonTop);

        bool commandlink = GUILayout.Button(checkCommandsText.Colored(titleColor).Bold()
            , buttonStyle);

        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        if (commandlink)
            Application.OpenURL(
                "https://sites.google.com/view/monkey-user-guide/command-list");

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(newVersionButtonBoxStyle);
        GUILayout.Label("", buttonTop);
        bool writeYourOwn = GUILayout.Button(writeYourOwnText.Colored(titleColor).Bold()
            , buttonStyle);
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        if (writeYourOwn)
            Application.OpenURL(
                "https://sites.google.com/view/monkey-user-guide/command-creation");
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideSecondLineStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideLineStyle);
        GUILayout.EndVertical();
    }

    private void NeedHelpSection()
    {
        GUILayout.BeginVertical(sectionBackgroundStyle);

        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine1Style);
        //    GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine2Style);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSearchResultLine3Style);

        GUILayout.Label(needHelpTitle.Colored(MonkeyStyle.Instance.QuickNameTextColor),
            sectionTitleStyle);
        GUILayout.Label(needHelpText.Colored(sectionSubtitleColor),
            sectionSubtitleStyle);
        GUILayout.BeginHorizontal(welcomeTextStyle);
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(newVersionButtonBoxStyle);
        GUILayout.Label("", buttonTop);
        bool discord = GUILayout.Button(chatOnDiscordText.Colored(titleColor).Bold()
            , buttonStyle);
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        if (discord)
            Application.OpenURL(
                "https://discordapp.com/invite/wRzsqxn");

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(newVersionButtonBoxStyle);
        GUILayout.Label("", buttonTop);
        bool support = GUILayout.Button(supportText.Colored(titleColor).Bold()
            , buttonStyle);
        if (support)
            Application.OpenURL(
                "https://sites.google.com/view/monkey-commander/support");
        GUILayout.Label("", buttonBottom);
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideSecondLineStyle);
        GUILayout.Label("", MonkeyStyle.Instance.HorizontalSideLineStyle);
        GUILayout.EndVertical();
    }
}
