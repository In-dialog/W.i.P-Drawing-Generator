using UnityEngine;
using MidiJack;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class NoteIndicator : MonoBehaviour
{
    public CreateLine _createLine;
    public int noteNumber;
    bool wasActiv;

    void Update()
    {
        GameObject[] _gameObjects = GameObject.FindGameObjectsWithTag("Player");
        _createLine = _gameObjects[0].GetComponent<CreateLine>();
        float note = MidiMaster.GetKey(MidiChannel.All, noteNumber);
        //Debug.Log(note);
        transform.localScale = Vector3.one * (0.01f + MidiMaster.GetKey(MidiChannel.All,noteNumber));
        Color color = MidiMaster.GetKeyDown(noteNumber) ? Color.red : Color.white;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetColor("_EmissionColor", color);
        GetComponent<Renderer>().SetPropertyBlock(block);
        //Debug.Log(noteNumber);

        if (note > 0 & _createLine != null)
        {
            _createLine.SetKnot = noteNumber;
            wasActiv = true;
        }
        else if (note == 0 & wasActiv & _createLine != null)
        {
            _createLine.RemoveKnot = noteNumber;
        }
    }

    public PlayerStatistics LocalCopyOfData;
    public PlayerStatistics localPlayerData = new PlayerStatistics();



    public void SaveData()
    {
        Debug.Log("save");
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/save.binary" + this.transform.name);

        //LocalCopyOfData = PlayerState.Instance.localPlayerData;

        localPlayerData.PositionX = transform.position.x;
        localPlayerData.PositionY = transform.position.y;
        localPlayerData.PositionZ = transform.position.z;

        formatter.Serialize(saveFile, localPlayerData);

        saveFile.Close();
    }

    public void LoadData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = "Saves/save.binary" + this.transform.name;
        if (File.Exists(filePath))
        {
            FileStream saveFile = File.Open(filePath, FileMode.Open);

            LocalCopyOfData = (PlayerStatistics)formatter.Deserialize(saveFile);

            saveFile.Close();
            transform.position = new Vector3(LocalCopyOfData.PositionX, LocalCopyOfData.PositionY, LocalCopyOfData.PositionZ);
        }
    }
}
