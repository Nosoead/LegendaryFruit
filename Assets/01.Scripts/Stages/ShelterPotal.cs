public class ShelterPotal : Potal
{
    protected override void ChagedStage()
    {
        StageManager.Instance.NextStage("0");
    }
}
