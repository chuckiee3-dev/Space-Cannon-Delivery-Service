using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerController : MonoBehaviour {

    [ Header( "Cannon Movement" ) ] [ SerializeField ]
    private float rotationSpeed;

    [ SerializeField ] private Transform cannon;
    [ SerializeField ] private Transform cannonEnd;

    [ Header( "Cannon Shoot Parameters" ) ] [ SerializeField ]
    private float cooldown = .333f;

    [ SerializeField ] private float maxChargeDuration = .8f;
    [ SerializeField ] private float shootForceMultiplier = 1f;
    [ SerializeField ] private Vector2 shootForcePercentRange = new Vector2( .25f, 1f );

    [ Header( "Prefabs" ) ] [ SerializeField ]
    private SpacePackage packagePrefab;

    private float chargeDuration;

    private IGameEvents gameEvents;
    private IObjectResolver  diContainer;
    private bool isCannonReady;
    private bool isAbleToShoot;
    private bool inputValidShootDown;

    private void Awake() {
        isCannonReady = true;
        isAbleToShoot = true;
    }

    [ Inject ]
    public void Construct( IGameEvents gameEvents, IObjectResolver  diContainer ) {
        this.gameEvents = gameEvents;
        this.diContainer = diContainer;
        gameEvents.OnPackageApproved += SendPackage;
        gameEvents.OnOutOfPackages += DisableCannon;
        gameEvents.OnLevelCompleted += DisableCannon;
        gameEvents.OnLevelLoaded += EnableCannon;
        gameEvents.OnLevelFailed += DisableCannon;
        gameEvents.OnLevelRewind += EnableCannon;
    }


    private void OnDestroy() {
        gameEvents.OnPackageApproved -= SendPackage;
        gameEvents.OnOutOfPackages -= DisableCannon;
        gameEvents.OnLevelCompleted -= DisableCannon;
        gameEvents.OnLevelLoaded -= EnableCannon;
        gameEvents.OnLevelFailed -= DisableCannon;
        gameEvents.OnLevelRewind -= EnableCannon;
    }

    private void EnableCannon() {
        isAbleToShoot = true;
    }

    private void DisableCannon() {
        isAbleToShoot = false;
        inputValidShootDown = false;
    }
    private void Update() {
        ProcessMovementInput();
        ProcessChargeInput();
    }

    private void ProcessMovementInput() {
        if ( Input.GetKey( KeyCode.LeftArrow ) || Input.GetKey( KeyCode.A ) ) {
            RotateCannon( 1 );
        }
        else if ( Input.GetKey( KeyCode.RightArrow ) || Input.GetKey( KeyCode.D ) ) {
            RotateCannon( -1 );
        }
    }

    private void ProcessChargeInput() {
        if ( !isAbleToShoot || !isCannonReady ) {
            inputValidShootDown = false;
            return;
        }

        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            inputValidShootDown = true;
        }
        if ( Input.GetKey( KeyCode.Space ) ) {
            chargeDuration += Time.deltaTime;
        }

        if ( Input.GetKeyUp( KeyCode.Space ) && inputValidShootDown ) {
            gameEvents.OnPackageRequested?.Invoke();
        }
    }

    private void RotateCannon( int direction ) {
        cannon.Rotate( 0, 0, rotationSpeed * Time.deltaTime * direction );
    }

    private async void SendPackage() {
        isCannonReady = false;
        await UniTask.Delay( TimeSpan.FromSeconds( cooldown ), ignoreTimeScale: false );
        isCannonReady = true;
        chargeDuration = Mathf.Clamp( chargeDuration,0, maxChargeDuration );
        float chargeForce = chargeDuration.Remap( 0, maxChargeDuration, shootForcePercentRange.x, shootForcePercentRange.y );
        SpacePackage package = diContainer.Instantiate( packagePrefab, cannonEnd.transform.position, cannon.transform.rotation );
        package.Send( chargeForce * shootForceMultiplier * cannon.up );
        chargeDuration = 0;
    }

}