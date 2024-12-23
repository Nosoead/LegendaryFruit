public class ShelterPotal : testPotal
{
    protected override void ChagedStage()
    {
        if (GameManager.Instance.isClear == true)
            testStageManager.Instance.StageChange("Shelter");
    }
}
