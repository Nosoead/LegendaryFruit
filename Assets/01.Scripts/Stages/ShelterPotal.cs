public class ShelterPotal : Potal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear == true)
            StageManager.Instance.NextStage("0");
    }
}
