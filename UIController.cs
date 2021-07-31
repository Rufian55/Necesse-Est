using UnityEngine;

public class UIController : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }    
    }

}
