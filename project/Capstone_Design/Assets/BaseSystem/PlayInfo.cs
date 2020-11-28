using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfo {
    int INT_NONE = -999999;
    float FLOAT_NONE = -9999.99f;

    public string infoName;
    string infoType;
    int infoValue_typeInt;
    float infoValue_typeFloat;
    string infoValue_typeString;


    public PlayInfo(string infoName) {
        this.infoName = infoName;
        this.infoType = "none";
        this.infoValue_typeInt = INT_NONE;
        this.infoValue_typeFloat = FLOAT_NONE;
        this.infoValue_typeString = "";
    }
    public PlayInfo(string infoName, int infoValue) : this(infoName) {
        this.infoName = infoName;
        this.infoType = "int";
        this.infoValue_typeInt = infoValue;
    }
    public PlayInfo(string infoName, float infoValue) : this(infoName) {
        this.infoName = infoName;
        this.infoType = "float";
        this.infoValue_typeFloat = infoValue;
    }
    public PlayInfo(string infoName, string infoValue) : this(infoName) {
        this.infoName = infoName;
        this.infoType = "string";
        this.infoValue_typeString = infoValue;
    }

    public string getInfoName() {
        return this.infoName;
    }

    public object getInfoValue() {
        if (infoType == "int")          return infoValue_typeInt;
        else if (infoType == "float")   return infoValue_typeFloat;
        else if (infoType == "string")  return infoValue_typeString;
        else                            return null;
    }

    public string getInfoType() {
        return this.infoType;
    }

    public void setValue(object value) { 
        if (infoType == "int")          this.infoValue_typeInt      = (int) value; 
        else if (infoType == "float")   this.infoValue_typeFloat    = (float) value;
        else if (infoType == "string")  this.infoValue_typeString   = (string) value;
    }

    public void modifyValue(object value) {
        if (infoType == "int")          this.infoValue_typeInt      += (int) value; 
        else if (infoType == "float")   this.infoValue_typeFloat    += (float) value;
        else if (infoType == "string")  this.infoValue_typeString   += (string) value;        
    }
}