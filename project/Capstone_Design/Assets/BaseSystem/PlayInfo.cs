using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayInfo {
    int INT_NONE = -999999;
    float FLOAT_NONE = -9999.99f;
    int MIN = -999999;
    int MAX = 2000000000;

    public string infoName;
    public string type;
    string infoType;
    public int infoValue_typeInt;
    float infoValue_typeFloat;
    string infoValue_typeString;

    int min;
    int max;

    
    public PlayInfo(string type, string infoName) {
        this.type = type;
        this.infoName = infoName;
        this.infoType = "none";
        this.infoValue_typeInt = INT_NONE;
        this.infoValue_typeFloat = FLOAT_NONE;
        this.infoValue_typeString = "";
        this.min = MIN;
        this.max = MAX;
    }
    public PlayInfo(string type, string infoName, int infoValue) : this(type, infoName) {
        this.infoName = infoName;
        this.infoType = "int";
        this.infoValue_typeInt = infoValue;
    }
    public PlayInfo(string type, string infoName, int infoValue, int min, int max) : this(type, infoName, infoValue) {
        this.min = min;
        if (max >= min) this.max = max;
    }
    public PlayInfo(string type, string infoName, float infoValue) : this(type, infoName) {
        this.infoName = infoName;
        this.infoType = "float";
        this.infoValue_typeFloat = infoValue;
    }
    public PlayInfo(string type, string infoName, float infoValue, int min, int max) : this(type, infoName, infoValue) {
        this.min = min;
        if (max >= min) this.max = max;
    }
    public PlayInfo(string type, string infoName, string infoValue) : this(type, infoName) {
        this.infoName = infoName;
        this.infoType = "string";
        this.infoValue_typeString = infoValue;
        this.min = 0;
    }
    public PlayInfo(string type, string infoName, string infoValue, int min, int max) : this(type, infoName, infoValue) {
        if (min > 0) this.min = min;
        if (max >= min) this.max = max;
    }

    public string GetInfoName() {
        return this.infoName;
    }

    public object GetInfoValue() {
        if (infoType == "int")          return infoValue_typeInt;
        else if (infoType == "float")   return infoValue_typeFloat;
        else if (infoType == "string")  return infoValue_typeString;
        else                            return null;
    }

    public string GetInfoType() {
        return this.infoType;
    }

    public int[] GetRange() {
        return new int[] {min, max};
    }

    public void SetValue(object value) { 
        if (infoType == "int")          this.infoValue_typeInt      = (int) value; 
        else if (infoType == "float")   this.infoValue_typeFloat    = (float) value;
        else if (infoType == "string")  this.infoValue_typeString   = (string) value;
        fixRangeOut();
    }

    public void ModifyValue(object value) {
        if (infoType == "int")          this.infoValue_typeInt      += (int) value; 
        else if (infoType == "float")   this.infoValue_typeFloat    += (float) value;
        else if (infoType == "string")  this.infoValue_typeString   += (string) value;        
        fixRangeOut();

    }

    void fixRangeOut() {
        if (infoValue_typeInt < this.min)           infoValue_typeInt = this.min;
        else if (infoValue_typeInt > this.max)      infoValue_typeInt = this.max;
        if (infoValue_typeFloat < this.min)         infoValue_typeFloat = this.min;
        else if (infoValue_typeFloat > this.max)    infoValue_typeFloat = this.max;
    }
}