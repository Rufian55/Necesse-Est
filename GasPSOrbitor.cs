/*********************************************************************
 * GasPSOrbitor.cs - Manages initialzation and dispostion of 
 * particleOrbitor GO's.  Accessing and HDR "recoloring" particle
 * swarm is not supported in Unity 2020.2.3 URP Shader Effect setup.
 * Each Orbiting PS has a ColorProperty colorPicker interface exposed.
 * Since simulation starts in manual mode (not auto indexing), we must
 * instantiate the first Particle Swarm here in Start().
 *********************************************************************/
using UnityEngine;


public class GasPSOrbitor : MonoBehaviour {

    public static GasPSOrbitor Instance = null;

    #pragma warning disable 0649
    [SerializeField] private GameObject[] particleOrbitors;
    [SerializeField] private Transform deadCenter;
    #pragma warning restore 0649

    private GameObject holder;
    private int skyBoxArrayLength;


    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        skyBoxArrayLength = ObjectAccessor.Instance.GetSkyBoxArrayLength();
        SwapParticleOrbitor(0);
    }

    public void SwapParticleOrbitor(int arg) {
        if (arg < 0) arg = skyBoxArrayLength - 1;
        if (arg >= skyBoxArrayLength) arg = 0;
        if (holder != null) {
            Destroy(holder);
        }
        holder = Instantiate(particleOrbitors[arg], deadCenter);
    }

}
