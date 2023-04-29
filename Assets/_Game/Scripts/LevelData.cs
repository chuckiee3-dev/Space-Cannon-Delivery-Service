using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[ExecuteAlways]
public class LevelData : MonoBehaviour {

    [ field: SerializeField ] public int totalPlayerPackages { get; set; }

    [ SerializeField ] private List<InterplanetoryCustomer> customers;

    [ field: SerializeField ] public int packagesToDeliver { get; set; }
    
    private void UpdateCustomers() {
        
        if ( customers == null) {
            customers = new List<InterplanetoryCustomer>();
        }

        customers = transform.GetComponentsInChildren<InterplanetoryCustomer>(true).ToList();
        packagesToDeliver = customers.Sum( t => t.GetPackagesRequiredCount() );
    }
#if UNITY_EDITOR
    private void OnValidate() {
        UpdateCustomers();
    }


    private void OnTransformChildrenChanged() {
        UpdateCustomers();
    }
#endif
}
