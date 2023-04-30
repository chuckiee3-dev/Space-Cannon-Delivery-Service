using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameManager : MonoBehaviour {

    [ SerializeField ] private List<LevelData> levels;
    [SerializeField] private int playerPackages;
    [SerializeField] private PackageCanvas packageCanvas;
    [SerializeField] private RewindCanvas rewindCanvas;
    [SerializeField]private bool testLevel;
    [SerializeField]private int testIndex;
    private float levelStabilizeCheckDuration = 15;
    private float levelStabilizeTimer;
    private IGameEvents gameEvents;
    private ISaveManager saveManager;
    private IObjectResolver resolver;
    private LevelData currentLevel;
    private int packagesDelivered = 0;
    private bool levelCompleted;
    private bool stabilizeTimerActive;
    [ Inject ]
    public void Construct( IGameEvents gameEvents , ISaveManager saveManager, IObjectResolver resolver) {
        this.gameEvents = gameEvents;
        this.saveManager = saveManager;
        this.resolver = resolver;
        gameEvents.OnPackageRequested += PackageRequested;
        gameEvents.OnNextLevelRequested += LevelRequested;
        gameEvents.OnPackageDelivered += PackageDelivered;
        gameEvents.OnRewindRequested += RewindLevel;
        gameEvents.OnOutOfPackages += StartStabilizeTimer;
    }

    private void StartStabilizeTimer() {
        stabilizeTimerActive = true;
    }

    private void Update() {
        if ( !stabilizeTimerActive || levelCompleted) {
            return;
        }

        levelStabilizeTimer += Time.deltaTime;
        if ( levelStabilizeTimer >= levelStabilizeCheckDuration) {
            stabilizeTimerActive = false;
            gameEvents.OnLevelFailed?.Invoke();
        }
    }

    private void OnDestroy() {
        if(gameEvents != null){
            gameEvents.OnPackageRequested -= PackageRequested;
            gameEvents.OnNextLevelRequested -= LevelRequested;
            gameEvents.OnPackageDelivered -= PackageDelivered;
            gameEvents.OnRewindRequested -= RewindLevel;
            gameEvents.OnOutOfPackages -= StartStabilizeTimer;
        }
    }

    private void PackageDelivered() {
        if ( levelCompleted ) {
            return;
        }
        packagesDelivered++;
        Debug.Log( "Package delivered: " + packagesDelivered+"/"+currentLevel.packagesToDeliver  );
        if ( packagesDelivered == currentLevel.packagesToDeliver) {
            LevelCompleted();
        }
    }

    private void PackageRequested() {
        if ( playerPackages > 0 ) {
            playerPackages--;
            packageCanvas.SetAmount( playerPackages );
            if ( playerPackages <= currentLevel.packagesToDeliver - packagesDelivered ) {
                rewindCanvas.Show();
            }
            gameEvents.OnPackageApproved?.Invoke();
        }else if ( playerPackages == 0 ) {
            gameEvents.OnOutOfPackages?.Invoke();
        }
    }

    private void Start() {
        Debug.Log( "Loading Initial Level" );
        LoadLevel( saveManager.CurrentLevel() );
    }

    private void LevelRequested() {
        packagesDelivered = 0;
        Debug.Log( "Loading Next Level" );
        LoadLevel( saveManager.CurrentLevel() );
    }
    
    private void LevelCompleted() {
        rewindCanvas.Hide();
        Debug.Log( "Level Completed" );
        levelCompleted = true;
        gameEvents.OnLevelCompleted?.Invoke();
        saveManager.LevelCompleted();
    }
    private void RewindLevel() {
        rewindCanvas.Hide();
        Debug.Log( "RewindLevel!" );
        LoadLevel( saveManager.CurrentLevel() );
        gameEvents.OnLevelRewind?.Invoke();
    }
    private void LoadLevel( int currentLevelIndex ) {
        if ( currentLevel != null ) {
            Destroy( currentLevel.gameObject );
            List<SpacePackage> spacePackages = FindObjectsOfType<SpacePackage>().ToList();
            for ( int i = spacePackages.Count - 1; i >= 0; i-- ) {
                Destroy(spacePackages[i].gameObject);
                spacePackages.RemoveAt( i );
            }
        }

        levelStabilizeTimer = 0;
        levelCompleted = false;
        packagesDelivered = 0;
        currentLevel = resolver.Instantiate(levels[testLevel? testIndex: currentLevelIndex % levels.Count]);
        playerPackages = currentLevel.totalPlayerPackages;
        packageCanvas.SetAmount( playerPackages );
        gameEvents.OnLevelLoaded?.Invoke();
        if ( currentLevelIndex % levels.Count == 0 ) {
            packageCanvas.Hide();
        }
        else {
            packageCanvas.Show();
        }
        rewindCanvas.Hide();
    }

}
