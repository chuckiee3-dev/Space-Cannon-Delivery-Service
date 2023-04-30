using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class PlayerController : MonoBehaviour {

    [ Header( "Cannon Movement" ) ] [ SerializeField ]
    private float rotationSpeed;

    [ SerializeField ] private Transform cannon;
    [ SerializeField ] private Transform cannonEnd;

    [ Header( "Cannon Shoot Parameters" ) ] [ SerializeField ]
    private float cooldown = .333f;
    [ Header( "Cannon Charge Bar" ) ] 
    [ SerializeField ] private Canvas chargeCanvas;
    [ SerializeField ] private Image chargeFillImage;

    [ SerializeField ] private float maxChargeDuration = .8f;
    [ SerializeField ] private float shootForceMultiplier = 1f;
    [ SerializeField ] private Vector2 shootForcePercentRange = new Vector2( .25f, 1f );

    [ Header( "Prefabs" ) ] 
    [ SerializeField ] private SpacePackage packagePrefab;

    [ Header( "Animation Parameters" ) ]
    [ SerializeField ] private SpriteRenderer cannonRenderer;
    [ SerializeField ] private Sprite idleFrame;
    [ SerializeField ] private Sprite chargeFrame;
    [ SerializeField ] private Sprite shootFrame;
    [ SerializeField ] private Sprite shootMuzzleFlashFrame;
    
    private float chargeDuration;

    private IGameEvents gameEvents;
    private IObjectResolver  diContainer;
    private bool isCannonReady;
    private bool isAbleToShoot;
    private bool inputValidShootDown;
    private bool charging;
    private void Awake() {
        isCannonReady = true;
        isAbleToShoot = true;
        cannonRenderer.sprite = idleFrame;
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
        if ( gameEvents != null ) {
            gameEvents.OnPackageApproved -= SendPackage;
            gameEvents.OnOutOfPackages -= DisableCannon;
            gameEvents.OnLevelCompleted -= DisableCannon;
            gameEvents.OnLevelLoaded -= EnableCannon;
            gameEvents.OnLevelFailed -= DisableCannon;
            gameEvents.OnLevelRewind -= EnableCannon;
        }
    }

    private void EnableCannon() {
        isAbleToShoot = true;
    }

    private void DisableCannon() {
        isAbleToShoot = false;
        inputValidShootDown = false;
        charging = false;
    }
    private void Update() {
        ProcessMovementInput();
        ProcessChargeInput();
        UpdateChargeBar();
    }

    private void UpdateChargeBar() {
        if ( charging ) {
            if ( !chargeCanvas.enabled ) {
                chargeFillImage.fillAmount = 0;
                chargeCanvas.enabled = true;
            }
            
            chargeDuration = Mathf.Clamp( chargeDuration,0, maxChargeDuration );
           float fill= chargeDuration.Remap( 0, maxChargeDuration, 0,1 );
           chargeFillImage.fillAmount = fill;
        }
        else {
            if ( chargeCanvas.enabled ) {
                chargeCanvas.enabled = false;
            }
        }
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

        if ( Input.GetKeyDown( KeyCode.Space )  ) {
            inputValidShootDown = true;
        }
        if ( Input.GetKey( KeyCode.Space )&& inputValidShootDown ) {
            chargeDuration += Time.deltaTime;
            cannonRenderer.sprite = chargeFrame;
            charging = true;
        }

        if ( Input.GetKeyUp( KeyCode.Space ) && inputValidShootDown ) {
            gameEvents.OnPackageRequested?.Invoke();
            charging = false;
        }
    }

    private void RotateCannon( int direction ) {
        cannon.Rotate( 0, 0, rotationSpeed * Time.deltaTime * direction );
    }

    private async void SendPackage() {
        isCannonReady = false;
        cannonRenderer.sprite = shootMuzzleFlashFrame;
        await UniTask.DelayFrame( 2 );
        cannonRenderer.sprite = shootFrame;
        chargeDuration = Mathf.Clamp( chargeDuration,0, maxChargeDuration );
        float chargeForce = chargeDuration.Remap( 0, maxChargeDuration, shootForcePercentRange.x, shootForcePercentRange.y );
        SpacePackage package = diContainer.Instantiate( packagePrefab, cannonEnd.transform.position, cannon.transform.rotation );
        package.Send( chargeForce * shootForceMultiplier * cannon.up );
        chargeDuration = 0;
        await UniTask.Delay( TimeSpan.FromSeconds( cooldown ), ignoreTimeScale: false );
        cannonRenderer.sprite = idleFrame;
        isCannonReady = true;
    }

}