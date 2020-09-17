using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropActivator : MonoBehaviour {

    [SerializeField] int scoreBonus = 150;
    [SerializeField] int minItemCount = 0;
    [SerializeField] int maxItemCount = 1;
    [SerializeField] float minDropSpeed = 2f;
    [SerializeField] float maxDropSpeed = 10f;
    // Start is called before the first frame update

    public int GetMaxItemCount() {
        return maxItemCount;
    }

    public int GetMinItemCount() {
        return minItemCount;
    }

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
