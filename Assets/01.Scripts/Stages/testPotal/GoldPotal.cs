public class GoldPotal : testPotal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear == true)
            testStageManager.Instance.StageChange("Stage2");
    }
}
