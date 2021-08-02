/**********************************************************************************
 * SwirlColorer.cs - manages runtime assignment of color gradientsto swirl particle
 * system.  Note new gradient startcolor assignment changes the PS, not the Shader.
 * [Seperate class from ObjectAccessor due to issues with maintaining runtime 
 *  references to the particle system.]
 *********************************************************************************/
using UnityEngine;

public class SwirlColorer : MonoBehaviour {

    public static SwirlColorer Instance = null;

    #pragma warning disable 0649
    [SerializeField] private Color[] swirlColorsLow;
    [Space]
    [SerializeField] private Color[] swirlColorsHigh;
    #pragma warning restore 0649

    private int arg;
    private ParticleSystem swirls;
    private ParticleSystem.MainModule settings;


    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        swirls = GetComponent<ParticleSystem>();
        settings = swirls.main;
    }

    public void SetSwirlColors () {
        arg = ObjectAccessor.Instance.SkyBoxIndex;
        settings.startColor = new ParticleSystem.MinMaxGradient(swirlColorsLow[arg], swirlColorsHigh[arg]);
    }

}
