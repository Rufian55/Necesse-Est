using UnityEngine;

public class SwirlColorer : MonoBehaviour {

    public static SwirlColorer Instance = null;
    [SerializeField] private Color[] swirlColorsLow;
    [Space]
    [SerializeField] private Color[] swirlColorsHigh;

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
