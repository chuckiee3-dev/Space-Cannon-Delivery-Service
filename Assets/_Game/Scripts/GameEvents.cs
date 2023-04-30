public class GameEvents : IGameEvents
{
    public IGameEvents.PackageRequested OnPackageRequested { get; set; }
    public IGameEvents.PackageApproved OnPackageApproved { get; set; }
    public IGameEvents.OutOfPackages OnOutOfPackages { get; set; }
    public IGameEvents.PackageDelivered OnPackageDelivered { get; set; }
    public IGameEvents.CustomerCompleted OnCustomerCompleted { get; set; }
    public IGameEvents.LevelFailed OnLevelFailed { get; set; }
    public IGameEvents.LevelCompleted OnLevelCompleted { get; set; }
    public IGameEvents.LevelLoaded OnLevelLoaded { get; set; }
    public IGameEvents.NextLevelRequested OnNextLevelRequested { get; set; }
    public IGameEvents.RewindRequested OnRewindRequested { get; set; }
    public IGameEvents.LevelRewind OnLevelRewind { get; set; }
    public IGameEvents.BoxHit OnBoxHit { get; set; }

}
