using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBreak : MonoBehaviour {

    public CrystalCount winPortal;

    private void OnDestroy()
    {
        winPortal.CrystalBroken();
    }
}
