/***********************************************************************************
 * ObjectAccessor.cs - manages runtime assignment of skyboxes and matching blackhole
 * fresnel power and color via parallel arrays.
 * [See SwirlColoroer class for Swirl color runtime assignment.]
 **********************************************************************************/
using UnityEngine;

public class ObjectAccessor : MonoBehaviour {

    public static ObjectAccessor Instance = null;

    #pragma warning disable 0649
    [SerializeField] private GameObject blackHole;
    [SerializeField] private Material[] skyBoxes;        // 13 each.
    [SerializeField] private float[] fresnelPowers;        // 13 parallel.
    [SerializeField] private Color[] fresnelColors;      // 13 parallel.
    #pragma warning restore 0649

    private Material mats;

    private int _skyBoxIndex = 0;
    public int SkyBoxIndex {
        get {
            return _skyBoxIndex;
        }
        set { _skyBoxIndex = value;
              CheckOOR(_skyBoxIndex);
        }
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        InitializeSimulation();
    }

    private void InitializeSimulation() {
        RenderSettings.skybox = GetSkyBox(SkyBoxIndex);
        SetBlackHoleProperties();
        SwirlColorer.Instance.SetSwirlColors();
        UIController.Instance.SetGalaxyNameOnStart(0);
    }

    public Material GetSkyBox(int arg) {
        if (arg < 0) arg = skyBoxes.Length - 1;
        if (arg >= skyBoxes.Length) arg = 0;
        SkyBoxIndex = arg;
        return skyBoxes[arg];
    }

    public void SetBlackHoleProperties() {
        mats = blackHole.GetComponent<Renderer>().material;
        mats.SetFloat("Vector1_46D6E030", fresnelPowers[SkyBoxIndex]);
        mats.SetColor("Color_3DE1BAF", fresnelColors[SkyBoxIndex]);
        blackHole.GetComponent<Renderer>().material = mats;
    }

    private void CheckOOR(int index) {
        if (index < 0) {
            _skyBoxIndex = skyBoxes.Length - 1;
        }
        if (index >= skyBoxes.Length) {
            _skyBoxIndex = 0;
        }
    }
    

}
