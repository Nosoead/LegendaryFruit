public class WeaponPotal : Potal
{
    protected override void ChagedStage()
    {
        StageManager.Instance.NextStage("1-2");
    }
}
