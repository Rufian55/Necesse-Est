/*****************************************************************************************************
 * CameraOrbitor.cs - manages the cameras oscillating orbit around Necesse Est's DeadCenter object, 
 * a.k.a. camRotatesAroundThis. Manual controls also provided.  Changing axis vector3 values can
 * cause Gamma Ray exposure...
 *****************************************************************************************************/
using UnityEngine;

public class CameraOrbitor : MonoBehaviour {

    public Transform camRotatesAroundThis;          // Named "DeadCenter" in heirarchy.
    private float orbitSpeed = 5f;
    private Vector3 orbitAxis;                      // Used for: primary axis of camera rotation, enable right mouse click User override.
    private float xMouseStart = 0;                  // Used to capture User's Right Mouse up/down Input.
    private Vector3 relativePosition;               // Used to hold intermediate position data as Camera transitions to new star.
    private Quaternion newRotation;                 // Used to hold intermediate rotation data as Camera "Look Rotates".
    
    private Vector3 axis = new Vector3(0.8f, 1f, 0f);
    
    
    void Start() {
        orbitAxis = axis;
    }

    void LateUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            orbitAxis = axis;
        }
        if (Input.GetMouseButtonDown(1)) {                                                                              // RH Mouse down!
            xMouseStart = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(1)) {                                                                                // RH Mouse up!
            xMouseStart = 0;
        }
        // Enable User to manually control speed and direction of simulation rotation.
        if (Input.GetMouseButton(1)) {                                                                                  // User is holding RH Down!
            orbitAxis = Input.mousePosition.x > xMouseStart ? Vector3.down : Vector3.up;                                // Get mouse direction.
            orbitSpeed = Mathf.Clamp(Mathf.Abs(Input.mousePosition.x - xMouseStart), 0, 10f);                           // Clamp rotation speed.
            transform.RotateAround(camRotatesAroundThis.transform.position, orbitAxis, orbitSpeed * Time.deltaTime);    // Rotate on orbitAxis per User Input.
        }
        else { // Otherwise, "auto" track and rotate Camera.
            // Vector subtraction get intermediate camera position.
            relativePosition = camRotatesAroundThis.position - transform.position;

            // Establish the new rotation by "looking" at the new intermediate position. 
            newRotation = Quaternion.LookRotation(relativePosition);

            // Execute the new "incremental" rotation against the Camera's transform.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, orbitSpeed * Time.deltaTime);

            // Add the overall rotation of the camera on its axis at a frame rate adjusted speed.
            transform.RotateAround(camRotatesAroundThis.transform.position, orbitAxis, orbitSpeed * Time.deltaTime);
        }
    }

}
