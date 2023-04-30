using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

public class FeelsPlayer : MonoBehaviour {

    [ SerializeField ] private MMF_Player CustomerOk;
    [ SerializeField ] private MMF_Player PackageDelivered;
    [ SerializeField ] private MMF_Player CannonShoot;
    [ SerializeField ] private MMF_Player BoxHit;
    private IGameEvents gameEvents;

    [ Inject ]
    public void Construct( IGameEvents gameEvents ) {
        this.gameEvents = gameEvents;
        gameEvents.OnCustomerCompleted += PlayCustomerOkFeedback;
        gameEvents.OnPackageDelivered += PlayPackageDeliveredFeedaback;
        gameEvents.OnPackageApproved += PlayCannonShootFeedback;
        gameEvents.OnBoxHit += PlayBoxHitFx;
    }

    private void OnDestroy() {
        if ( gameEvents != null ) {
            gameEvents.OnCustomerCompleted += PlayCustomerOkFeedback;
            gameEvents.OnPackageDelivered += PlayPackageDeliveredFeedaback;
            gameEvents.OnPackageApproved += PlayCannonShootFeedback;
            gameEvents.OnBoxHit += PlayBoxHitFx;
        }
    }

    private void PlayBoxHitFx() {
        BoxHit.PlayFeedbacks();
    }

    private void PlayCannonShootFeedback() {
        CannonShoot.PlayFeedbacks();
    }

    private void PlayPackageDeliveredFeedaback() {
        PackageDelivered.PlayFeedbacks();
    }

    private void PlayCustomerOkFeedback() {
        CustomerOk.PlayFeedbacks();
    }

}
