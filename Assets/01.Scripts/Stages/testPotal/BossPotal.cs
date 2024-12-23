public class BossPotal : testPotal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear)
            testStageManager.Instance.StageChange("Boss");
    }
}
