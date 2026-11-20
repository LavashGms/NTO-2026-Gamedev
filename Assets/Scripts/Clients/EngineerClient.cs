using UnityEngine;

public class EngineerClient : Client
{
    public override void OnCheckout() => GameManager.singleton.UnlockUpgradeMenu();
    public override void Finished()
    {
        base.Finished();
        GameManager.singleton.LockUpgradeMenu();
    }
}