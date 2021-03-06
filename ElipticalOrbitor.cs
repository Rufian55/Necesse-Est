/*****************************************************************************************************
 * ElipticalOrbitor.cs - manages the eliptal orbits of the various objects this script is attached to.
 * Initial object position is the semiMajor vertex of the ellipse as defined by (semiMinorAxis,
 * inclination, semiMajorAxis). See Start().
 *****************************************************************************************************/
using UnityEngine;

public class ElipticalOrbitor : MonoBehaviour {

    #pragma warning disable 0649
    [SerializeField] private float semiMajorAxis;       // Ellipse long axis.
    [SerializeField] private float semiMinorAxis;       // Ellipse short axis.
    [SerializeField] private float inclination;         // Ellipse angle of inclination in world space. When 0, move on the x-z plane.
    [SerializeField] private float angularVelocity;     // Speed of the object as it travels on the elispse.
    [SerializeField] private Vector3 _position;         // Internal - coordinates of the elliptical orbit each frame & Start!
    #pragma warning restore 0649
    
    private float angle;                                // Angle of elliptic trigonometric function.

    void Start() {
        angle = 0f;
        transform.position = new Vector3(semiMinorAxis * Mathf.Sin(0), inclination * Mathf.Cos(0), semiMajorAxis * Mathf.Cos(0));
    }

    void FixedUpdate() {
        angle += Time.deltaTime * angularVelocity;          // Velocity.
        _position.x = semiMinorAxis * Mathf.Sin(angle);
        _position.y = inclination * Mathf.Cos(angle);       // Ellipse inclination (y = Range [h, -h].
        _position.z = semiMajorAxis * Mathf.Cos(angle);
        transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime);
    }

}
