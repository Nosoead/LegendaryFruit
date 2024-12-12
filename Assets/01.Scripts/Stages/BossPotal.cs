public class BossPotal : Potal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear)
            StageManager.Instance.StageChange("Boss");
    }
}
