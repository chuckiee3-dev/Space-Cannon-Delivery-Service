using System.Collections.Generic;
using UnityEngine;

public class SatelliteManager : MonoBehaviour {

    [ SerializeField ] private List<SatelliteData> satelliteData;

    private void FixedUpdate() {
        foreach ( SatelliteData data in satelliteData ) {
            data.satelliteParent.Rotate( 0,0,data.rotationSpeed * Time.fixedDeltaTime );
        }
    }

}
