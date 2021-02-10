﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;

public class LevelEditor : MonoBehaviour
{
    public static LevelEditor instance;
    public int selectedUnit;
    public bool selected = false;
    public List<GameObject> unitPrefabs;
    public InputField stageName;
    public Level newLevel;
    public Level loadLevel;
    private Level emptyLevel;
    public List<Transform> vaccineUnits;
    public List<GameObject> placements;
    private GameManager gameManager;
    public Text coughc, feverc, redEyesc, sneezec, insomniac, sweatc, stuporc, vomitc, baldc, snotc;
    public InputField coughI, feverI, redEyesI, sneezeI, insomniaI, sweatI, stuporI, vomitI, baldI, snotI;



    private void Awake()
    {
        instance = this;
        newLevel = new Level();
        emptyLevel = new Level();
        
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.editMode = true;
        emptyInputRestriction();
    }

    public void CellInstantiate(Level level)
    {
        selected = false;
        selectedUnit = 0;

        foreach (Transform child in vaccineUnits)
        {
            if (child.childCount > 0)
            {
                Destroy(child.GetChild(0).gameObject);
            }
        }

        //int index = 0;
        for(int i=0; i<5; i++)
        {
            for(int j=0; j<8; j++)
            {
                int temp = i + j * 5;
                placements[temp].GetComponent<UnitPlacements>().instantiateUnit(level.arr[i, j]);
                newLevel.arr[i, j] = level.arr[i, j];
                
            }
        }

        coughc.text = level.cough.ToString();
        feverc.text = level.fever.ToString();
        redEyesc.text = level.redEyes.ToString();
        sneezec.text = level.sneeze.ToString();
        insomniac.text = level.insomnia.ToString();
        sweatc.text = level.sweat.ToString();
        stuporc.text = level.stupor.ToString();
        vomitc.text = level.vomit.ToString();
        baldc.text = level.bald.ToString();
        snotc.text = level.snot.ToString();

        applyRestriction();
        emptyInputRestriction();
        
    }

    public void LoadLevel()
    {
        try
        {
            string str = File.ReadAllText(Application.dataPath + "/Resources/Levels/" + stageName.text + ".json");
            loadLevel = JsonConvert.DeserializeObject<Level>(str);

        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e);
            return;
        }
        CellInstantiate(loadLevel);
        Debug.Log("Load complete.");
    }

    public void NewLevel()
    {
        CellInstantiate(emptyLevel);
        newLevel = new Level();
        Debug.Log("Created a new Level");
    }

    public void resetLevel()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");

        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    public void Save()
    {
        if(stageName.text == "")
        {
            Debug.Log("Enter a stage name!");
        }
        else
        {
            string filePath = Application.dataPath + "/Resources/Levels";

            if (!Directory.Exists(filePath))
                CreateSaveDirectory();

            string levelstr = JsonConvert.SerializeObject(newLevel);
            File.WriteAllText(Application.dataPath + "/Resources/Levels/" + stageName.text + ".json", levelstr);
            Debug.Log("Save complete.");
        }
        

    }

    public void applyRestrictionButton()
    {
        coughc.text = coughI.text;
        feverc.text = feverI.text;
        redEyesc.text = redEyesI.text;
        sneezec.text = sneezeI.text;
        insomniac.text = insomniaI.text;
        sweatc.text = sweatI.text;
        stuporc.text = stuporI.text;
        vomitc.text = vomitI.text;
        baldc.text = baldI.text;
        snotc.text = snotI.text;

        applyRestriction();
    }

    public void applyRestriction()
    {
        newLevel.cough = int.Parse(coughc.text);
        newLevel.fever = int.Parse(feverc.text);
        newLevel.redEyes = int.Parse(redEyesc.text);
        newLevel.sneeze = int.Parse(sneezec.text);
        newLevel.insomnia = int.Parse(insomniac.text);
        newLevel.sweat = int.Parse(sweatc.text);
        newLevel.stupor = int.Parse(stuporc.text);
        newLevel.vomit = int.Parse(vomitc.text);
        newLevel.bald = int.Parse(baldc.text);
        newLevel.snot = int.Parse(snotc.text);
    }

    public void emptyInputRestriction()
    {
        coughI.text = "0";
        feverI.text = "0";
        redEyesI.text = "0";
        sneezeI.text = "0";
        insomniaI.text = "0";
        sweatI.text = "0";
        stuporI.text = "0";
        vomitI.text = "0";
        baldI.text = "0";
        snotI.text = "0";
    }
}