using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    InputChecker inputChecker = new InputChecker();

    void Update() {
        inputChecker.Update();
        if (inputChecker.patternMatch("a") == 1) {Debug.Log("Key down = a");}
        // if (inputChecker.patternMatch("_a") == 1) {Debug.Log("Key downStay = a");}
        if (inputChecker.patternMatch("^a") == 1) {Debug.Log("Key up = a");}
        // if (inputChecker.patternMatch("!a") == 1) {Debug.Log("Key upStay = a");}
        if (inputChecker.patternMatch("q, w, e") == 1) {Debug.Log("KeyCombination = q, w, e");}
        if (inputChecker.patternMatch("_a, _s, !w") == 1) {Debug.Log("KeyCombination = _a, _s, !d");}
    }
}