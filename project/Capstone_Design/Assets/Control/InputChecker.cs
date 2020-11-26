using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecker {
    // 입력 상태 배열 리스트
    public List<int> keyInputList = new List<int>();
    public List<int> mouseInputList = new List<int>();
    private Vector2 mousePos = new Vector2();

    public enum Key {
        // 알파벳
        a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
        // 방향키
        arrowL, arrowD, arrowU, arrowR, 
        // 각종 기능키
        space, enter, backspace, ctrlL, ctrlR, altL, altR, shiftL, shiftR, tab, caps, esc,
        // 숫자
        n1, n2, n3, n4, n5, n6, n7, n8, n9, n0,
        // 상단 특수문자(` - =)
        backQuote, minus, equal,
        // 특수문자(. , / ; ' [ ] \)
        dot, comma, slash, semicolon, quote, bracketL, barcketR, backslash,
        // 펑션키
        f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12,
        // 마우스         
        mouseL, mouseR, mouseM, mouseEx1, mouseEx2, mouseEx3 
    }
    KeyCode[] KeyCodeList = {
        // 알파벳
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z,
        // 방향키
        KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.RightArrow,
        // 각종 기능키
        KeyCode.Space, KeyCode.Return, KeyCode.Backspace, 
        KeyCode.LeftControl, KeyCode.RightControl, 
        KeyCode.LeftAlt, KeyCode.RightAlt, 
        KeyCode.LeftShift, KeyCode.RightShift,
        KeyCode.Tab, KeyCode.CapsLock, KeyCode.Escape,
        // 숫자
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0,
        // 상단 특수문자
        KeyCode.BackQuote, KeyCode.Minus, KeyCode.Equals,
        // 특수문자
        KeyCode.Period, KeyCode.Comma, KeyCode.Slash, 
        KeyCode.Semicolon, KeyCode.Quote,
        KeyCode.LeftBracket, KeyCode.RightBracket, KeyCode.Backslash,
        // 펑션키
        KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, 
        KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8,
        KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12,
        // 마우스 입력
        KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5,
    };

    string[] keyStringList = {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "arrowL", "arrowD", "arrowU", "arrowR", 
        "space", "enter", "backspace", "ctrlL", "ctrlR", 
        "altL", "altR", "shiftL", "shiftR", "tab", "caps", "esc",
        "n1", "n2", "n3", "n4", "n5", "n6", "n7", "n8", "n9", "n0",
        "backQuote", "minus", "equal",
        "dot", "comma", "slash", "semicolon", "quote", "bracketL", "barcketR", "backslash",
        "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12",
        "mouseL", "mouseR", "mouseM", "mouseEx1", "mouseEx2", "mouseEx3" 
    };
    // Key enum 개수 저장
    int keyEnumCount = System.Enum.GetValues(typeof(Key)).Length;        

    int FALSE = -1;
    int NONE = 0;
    int TRUE = 1;

    public InputChecker() {
        for(int i = 0; i < keyEnumCount; i++) {keyInputList.Add(0);}
    }

    public void Update() {
        for (int i = 0; i < keyEnumCount; i++) {
            if (Input.GetKey(KeyCodeList[i])) {
                if (keyInputList[i] <= 0) {keyInputList[i] = 1;}
                else {keyInputList[i] = 2;}
                }            
            else {
                if (keyInputList[i] >= 1) {keyInputList[i] = -1;}
                else {keyInputList[i] = 0;}
            }
        }
    }

    enum Mode {
        down,       // 입력 감지 1회
        downStay,   // 입력 감지 지속
        up,         // 뗀 순간 감지.
        upStay      // 키가 입력되지 않은 상태를 감지.
    }

    public int patternMatch(string pattern) {        
        int checker = NONE;

        string[] splitedPattern = pattern.Split(',');
        List<Mode> patternMode = new List<Mode>();
        for (int i = 0; i < splitedPattern.Length; i++) {
            // 좌 우 공백 제거
            splitedPattern[i] = splitedPattern[i].Trim();
            // 모드 문자열 분리.
            if (splitedPattern[i][0] == '_') {
                patternMode.Add(Mode.downStay);
                splitedPattern[i] = splitedPattern[i].Substring(1);
            }
            else if (splitedPattern[i][0] == '^') {
                patternMode.Add(Mode.up);
                splitedPattern[i] = splitedPattern[i].Substring(1);
            }
            else if (splitedPattern[i][0] == '!') {
                patternMode.Add(Mode.upStay);
                splitedPattern[i] = splitedPattern[i].Substring(1);
            }
            else {
                patternMode.Add(Mode.down);
            }
        }

        for (int i = 0; i < splitedPattern.Length; i++) {
            for (int k = 0; k < keyStringList.Length; k++) {
                // 입력 패턴이랑 문자열 매칭. 매칭 시점 확인 -> k
                if (splitedPattern[i] == keyStringList[k]) {
                    // 입력 중이지 않음
                    if (stateCheck(k, patternMode[i])) checker = TRUE;
                    else checker = FALSE;
                    break;
                }
            }
            if (checker == FALSE) break;
        }

        if (checker == NONE || checker == FALSE) return FALSE;
        else return TRUE;
    }

    bool stateCheck (int k, Mode mode) {
        int checker = NONE;
        if (mode == Mode.down) {
            if (keyInputList[k] == 1) checker = TRUE;
            else checker = FALSE;
        }
        else if (mode == Mode.downStay) {
            if (keyInputList[k] >= 1) checker = TRUE;
            else checker = FALSE;
        }
        else if (mode == Mode.up) {
            if (keyInputList[k] == -1) checker = TRUE;
            else checker = FALSE;
        }
        else if (mode == Mode.upStay) {
            if (keyInputList[k] <= 0) checker = TRUE;
            else checker = FALSE;
        }

        if (checker == TRUE) return true;
        else return false;
    }

    public Vector2 getMousePos() {
        mousePos = Input.mousePosition;
        return mousePos;
    }
} 