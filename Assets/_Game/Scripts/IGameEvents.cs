public interface IGameEvents{
    public delegate void PackageRequested();
    public PackageRequested OnPackageRequested { get; set; }
    public delegate void PackageApproved();
    public PackageApproved OnPackageApproved { get; set; }
    
    public delegate void OutOfPackages();
    public OutOfPackages OnOutOfPackages { get; set; }
    
    public delegate void PackageDelivered();
    public PackageDelivered OnPackageDelivered { get; set; }
    
    public delegate void CustomerCompleted();
    public CustomerCompleted OnCustomerCompleted { get; set; }
    
    public delegate void LevelFailed();

    public LevelFailed OnLevelFailed { get; set; }

    public delegate void LevelCompleted();
    public LevelCompleted OnLevelCompleted { get; set; }

    public delegate void LevelLoaded();
    public LevelLoaded OnLevelLoaded { get; set; }

    public delegate void NextLevelRequested();
    public NextLevelRequested OnNextLevelRequested { get; set; }

    public delegate void RewindRequested();
    public RewindRequested OnRewindRequested { get; set; }
    public delegate void LevelRewind();
    public LevelRewind OnLevelRewind { get; set; }
    public delegate void BoxHit();
    public BoxHit OnBoxHit { get; set; }

}
