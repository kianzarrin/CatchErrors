using ColossalFramework.UI;
using UnityEngine;

namespace CatchErrors;

public static class UIUtil
{
    public static UITextureAtlas InGame => GetAtlas("Ingame");

    /* The code below was copied from Fine Road Tool and More Shortcuts mod by SamsamTS. Thanks! */
    public static UITextureAtlas GetAtlas(string name)
    {
        UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
        for (int i = 0; i < atlases.Length; i++)
        {
            if (atlases[i].name == name)
                return atlases[i];
        }

        return UIView.GetAView().defaultAtlas;
    }

    public static UICheckBox CreateCheckBox(UIComponent parent)
    {
        UICheckBox checkBox = (UICheckBox)parent.AddUIComponent<UICheckBox>();

        checkBox.width = 300f;
        checkBox.height = 20f;
        checkBox.clipChildren = true;

        UISprite sprite = checkBox.AddUIComponent<UISprite>();
        sprite.atlas = InGame;
        sprite.spriteName = "ToggleBase";
        sprite.size = new Vector2(16f, 16f);
        sprite.relativePosition = Vector3.zero;

        checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
        ((UISprite)checkBox.checkedBoxObject).atlas = InGame;
        ((UISprite)checkBox.checkedBoxObject).spriteName = "ToggleBaseFocused";
        checkBox.checkedBoxObject.size = new Vector2(16f, 16f);
        checkBox.checkedBoxObject.relativePosition = Vector3.zero;

        checkBox.label = checkBox.AddUIComponent<UILabel>();
        checkBox.label.text = " ";
        checkBox.label.textScale = 0.9f;
        checkBox.label.relativePosition = new Vector3(22f, 2f);

        checkBox.playAudioEvents = true;

        return checkBox;
    }

    public static UIButton CreateButton(UIComponent parent)
    {
        UIButton button = (UIButton)parent.AddUIComponent<UIButton>();

        button.atlas = InGame;
        button.size = new Vector2(90f, 30f);
        button.textScale = 0.9f;
        button.normalBgSprite = "ButtonMenu";
        button.hoveredBgSprite = "ButtonMenuHovered";
        button.pressedBgSprite = "ButtonMenuPressed";
        button.disabledBgSprite = "ButtonMenuDisabled";
        button.canFocus = false;
        button.playAudioEvents = true;

        return button;
    }

    // Ripped from Elektrix
    public static void SetupButtonStateSprites(ref UIButton button, string spriteName, bool noNormal = false)
    {
        button.normalBgSprite = spriteName + (noNormal ? "" : "Normal");
        button.hoveredBgSprite = spriteName + "Hovered";
        button.focusedBgSprite = spriteName + "Focused";
        button.pressedBgSprite = spriteName + "Pressed";
        button.disabledBgSprite = spriteName + "Disabled";
    }
}
