using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropActivator : MonoBehaviour {

    [SerializeField] int scoreBonus = 150;
    [SerializeField] float minDropSpeed = 2f;
    [SerializeField] float maxDropSpeed = 10f;
    // Start is called before the first frame update

    public float GetMinDropSpeed() {
        return minDropSpeed;
    }

    public float GetMaxDropSpeed() {
        return maxDropSpeed;
    }

    public int GetScoreBonus() {
        return scoreBonus;
    }

    public void Caught() {
        Destroy(gameObject);
    }
}
