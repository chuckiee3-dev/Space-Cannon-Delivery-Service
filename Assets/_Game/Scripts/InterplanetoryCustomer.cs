using System;
using TMPro;
using UnityEngine;
using VContainer;

public class InterplanetoryCustomer : MonoBehaviour {

    [SerializeField] private int packagesRequired;
    [SerializeField] private Transform collectTransform;
    [SerializeField] private TextMeshProUGUI collectAmountTmp;
    
    
    private int packagesCollected = 0;
    private LayerMasks layerMasks;
    private IGameEvents gameEvents;
    
    private void Awake() {
        collectAmountTmp.text = packagesCollected + "/" + packagesRequired;
    }

    [ Inject ]
    public void Construct( LayerMasks layerMasks , IGameEvents gameEvents) {
        this.layerMasks = layerMasks;
        this.gameEvents = gameEvents;
    }
    private void OnTriggerEnter2D( Collider2D other ) {
        if ( packagesCollected == packagesRequired ) {
            return;
        }
        if ( other.gameObject.layer == layerMasks.defaultPackageLayer ) {
            packagesCollected++;
            collectAmountTmp.text = packagesCollected + "/" + packagesRequired;
            other.GetComponent<SpacePackage>().CollectedBy(collectTransform.position);
            
            gameEvents?.OnPackageDelivered?.Invoke();
            if ( packagesCollected == packagesRequired ) {
                gameEvents?.OnCustomerCompleted?.Invoke();
            }
        }
    }

    public int GetPackagesRequiredCount() {
        return packagesRequired;
    }

}
