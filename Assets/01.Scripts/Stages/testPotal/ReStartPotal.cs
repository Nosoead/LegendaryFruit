
public class ReStartPotal : testPotal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear == true)
            testStageManager.Instance.StageChange("Lobby");
    }
}
