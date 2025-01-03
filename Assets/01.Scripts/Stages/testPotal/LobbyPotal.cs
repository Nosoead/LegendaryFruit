
public class LobbyPotal : testPotal
{
    protected override void ChagedStage()
    {
        testStageManager.Instance.StageChange("Stage1");
    }
}
