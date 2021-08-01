using UnityEngine;

public class ObjectAccessor : MonoBehaviour {

    public static ObjectAccessor Instance = null;

    #pragma warning disable 0649
    [SerializeField] private GameObject blackHole;
    [SerializeField] private Material[] skyBoxes;
    #pragma warning restore 0649

    private Material mats;

    // Annoying shorthand that is much less readable!
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
        mats = blackHole.GetComponent<Material>();
    }

    public Material GetSkyBox(int arg) {
        if (arg < 0) arg = skyBoxes.Length - 1;
        if (arg >= skyBoxes.Length) arg = 0;
        _skyBoxIndex = arg;
        return skyBoxes[arg];
    }

    public void SetBlackHoleProperties(float arg, Color color) {
        mats = blackHole.GetComponent<Renderer>().material;
        mats.SetFloat("Vector1_46D6E030", arg);
        mats.SetColor("Color_3DE1BAF", color);
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
