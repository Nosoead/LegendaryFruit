public class WeaponPotal : Potal
{
    protected override void ChagedStage()
    {
        if(GameManager.Instance.isClear == true)
            StageManager.Instance.NextStage("1-2");
    }
}
