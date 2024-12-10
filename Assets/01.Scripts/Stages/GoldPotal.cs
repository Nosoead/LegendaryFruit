public class GoldPotal : Potal
{
    protected override void ChagedStage()
    {
        StageManager.Instance.NextStage("1-1");
    }
}
