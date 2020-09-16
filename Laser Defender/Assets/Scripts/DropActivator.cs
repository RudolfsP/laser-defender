using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropActivator : MonoBehaviour {

    [SerializeField] int scoreBonus = 150;
    // Start is called before the first frame update
    public int GetScoreBonus() {
        return scoreBonus;
    }

    public void Caught() {
        Destroy(gameObject);
    }
}
