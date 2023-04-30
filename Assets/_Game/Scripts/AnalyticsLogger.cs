
using VContainer.Unity;

public class AnalyticsLogger: IStartable {
/*
   private readonly ISaveManager saveManager;
   private readonly IGameEvents gameEvents;
   [ Inject ]
   public AnalyticsLogger( ISaveManager saveManager, IGameEvents gameEvents ) {
      this.gameEvents = gameEvents;
      this.saveManager = saveManager;
      gameEvents.OnPackageApproved += PackageFired;
      gameEvents.OnNextLevelRequested += LevelCompleted;
      gameEvents.OnLevelFailed += LevelFailed;
      gameEvents.OnPackageDelivered += PackageDelivered;
   }

   ~AnalyticsLogger() {
      if ( gameEvents != null ) {
         gameEvents.OnPackageApproved -= PackageFired;
         gameEvents.OnNextLevelRequested -= LevelCompleted;
         gameEvents.OnLevelFailed -= LevelFailed;
         gameEvents.OnPackageDelivered -= PackageDelivered;
      }
   }
   private void PackageDelivered() {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
      Analytics.CustomEvent("PackageFired");
#endif
      Debug.Log( nameof(PackageDelivered) );
   }

   private void LevelCompleted() {      
#if ENABLE_CLOUD_SERVICES_ANALYTICS
      Analytics.CustomEvent("LevelEnd", new Dictionary<string, object> {
         { "Level", saveManager.CurrentLevel()},
         {"Status", "Complete"}
      });
#endif
      Debug.Log( nameof(LevelCompleted) );
   }


   private void LevelFailed() {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
      Analytics.CustomEvent("LevelEnd", new Dictionary<string, object> {
         { "Level", saveManager.CurrentLevel()},
         {"Status", "Failed"}
      });
#endif
      Debug.Log( nameof(LevelFailed) );
   }
   private void PackageFired() {
      
#if ENABLE_CLOUD_SERVICES_ANALYTICS
      Analytics.CustomEvent("PackageFired");
#endif
      Debug.Log( nameof(PackageFired) );
   }

   public void Start() {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
      Analytics.CustomEvent("GameStart");
      #endif
      Debug.Log( nameof(Start) );
   }
*/
   public void Start() {
   }

}
