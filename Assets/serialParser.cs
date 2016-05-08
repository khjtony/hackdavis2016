using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class digiFilter
{
    string filterType = "MEDIUM";
    int[] dataBank = new int[] { 0,0,0,0,0 };
    private int valueInt = 0;
    private float valueFloat = 0.0f;
    int dataNum = 5;

    public digiFilter()
    {
        // Init
        /*
        for (int i = 0; i < dataNum; i++)
        {
            dataBank[i]=0;
        }
        */
    }

    public void push(int value)
    {
        //print("digiFliter: push");
        for (int i = 0; i < dataNum-1; i++)
        {
            dataBank[i] = dataBank[i + 1];
        }
        dataBank[dataNum-1] = value;
        _updateValue();
    }

    public int getValue()
    {
        return valueInt;
    }



    private void _updateValue()
    {
        int sum = 0;
        for (int i = 0; i < dataNum; i++)
        {
            sum += dataBank[i];
        }
        valueInt = (int)sum / dataNum;
        //valueInt = (int)dataBank[0];
        valueFloat = (float)1.0f*sum / dataNum;

        //print("New value is: " + valueInt);
    }



}

public class serialParser : MonoBehaviour {
    digiFilter[] dataStore = new digiFilter[5];
    string rawString;
    bool flag = false;
    int dataNum = 5;

    private float[] dataCali = {0,976.7f,1080.0f,1166.0f,0.0f};
    private float[] slope = { 0, 1.847f, 1.8815f, 1.921f, 0.0f};
    private float _x = 0.0f;
    private float _y = 0.0f;
    private float _z = 0.0f;
    private float RAWTOCM = 8.6618f;

    public serialParser()
    {
        for (int i = 0; i < dataNum; i++)
        {
            dataStore[i] = new digiFilter();
        }

    }


    public void appendRawString(string piece)
    {
        rawString += piece;
        int iofH = rawString.IndexOf('H');
        int iofT = rawString.IndexOf('T');
        int ID = 0;
        int value = 0;
        string subset;
        while ((iofH!= -1) && (iofT!= -1) && (iofT > iofH))
        {
            subset = rawString.Substring(iofH, iofT-iofH);
            rawString = rawString.Substring(iofT+1);
            //print(subset);

            ID = subset[1]-'0';
            value = int.Parse(subset.Substring(2, (iofT-iofH-2)));
            //print("RAW: "+ subset +" Reading ID: " + ID + " Reading value: " + value+ "  Buffer length: "+rawString.Length);

            
            pushdistance(ID, _convertRawToCm(ID, value));


            iofH = rawString.IndexOf('H');
            iofT = rawString.IndexOf('T');
        }
    }

    public Vector3 getPosition()
    {
        //debug purpose
        //print("there are: " + dataStore.Length);
        
        _x = dataStore[1].getValue();
        _z = dataStore[2].getValue();
        
        return new Vector3(_x, _y, _z);
    }

    public int getValue(int ID)
    {
        return dataStore[ID].getValue();
    }

    public void pushdistance(int ID, int value)
    {
       // print("New Push: " + ID + " with value: " + value + "cm");
            dataStore[ID].push(value);
      

    }

    private int _convertRawToCm(int ID, int raw)
    {
        float adjusted = 1.0f * ((float)1.0f * (raw - dataCali[ID]) / slope[ID]); //in inches
        return (int)(adjusted * 2.54);
    }


	// Use this for initialization
	void Start () {
  


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
