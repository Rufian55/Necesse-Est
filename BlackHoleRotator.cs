/***********************************************************************************
 * BlackHoleRotator.cs - simple rotation script.
 ***********************************************************************************/
using UnityEngine;

public class BlackHoleRotator : MonoBehaviour {

    #pragma warning disable 0649
    [SerializeField] private GameObject coreAssembly;
    [SerializeField] private float rotationSpeed;
    #pragma warning restore 0649

    void FixedUpdate() {
        coreAssembly.transform.Rotate(0f, rotationSpeed * Time.fixedDeltaTime, 0f);
    }

}
