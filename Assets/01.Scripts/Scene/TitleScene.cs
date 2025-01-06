using UnityEngine;

public class TitleScene : BaseScene
{
    public override void Init()
    {
        //TODO : Sound초기화 내용 추가하면 될 듯
        UIManager.Instance.ToggleUI<PlayerCanvasUI>(isPreviousWindowActive: false, isOpened: false);
    }
}
