using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DialogueReader {
  public List<DialogueFragment> start = new List<DialogueFragment>();
  public List<DialogueFragment> end = new List<DialogueFragment>();

  public List<BFEvent> inBattle = new List<BFEvent>();
  string dataPath = Path.Combine(Application.dataPath, "Dialogues");


  public List<DialogueFragment> seen = new List<DialogueFragment>();

  public DialogueReader(string mapName) {
    if (File.Exists(Path.Combine(dataPath, mapName+"_start"))) {
      loadSE(start, mapName+"_start");
    }
    if (File.Exists(Path.Combine(dataPath, mapName+"_end"))) {
      loadSE(end, mapName+"_end");
    }
    if (File.Exists(Path.Combine(dataPath, mapName+"_battle"))) {
      loadInBattle(mapName+"_battle");
    }
  }

  //file names:
  //mapName_[start,battle,end]

  //before + after battle file format:
  //one character fragment per line
  //name line

  //in battle file format:
  //type number number
  //name line
  //name line ....
  //where type is time for BFTimeEvent
  //              bossTurn or broTurn for BFTurnEvent
  //first number is the turn/time
  //second is the number of dialogue lines

  void loadSE(List<DialogueFragment> d, string fileName) {
    foreach (string s in File.ReadAllLines(Path.Combine(dataPath, fileName))) {
      d.Add(constructDialogue(s));
    }
  }

  DialogueFragment constructDialogue(string s) {
    int index = s.IndexOf(" ");
    string charName = s.Substring(0,index);
    string dialogue = s.Substring(index+1);
    //find character with name:
    Character speaker = getChar(charName);
    Debug.AssertFormat(speaker != null, "Dialogue has unknown speaker " + charName);
    return new DialogueFragment(speaker, dialogue);
  }

  void loadInBattle(string fileName) {
    List<string> lines = new List<string>();
    foreach (string s in File.ReadAllLines(Path.Combine(dataPath, fileName))) {
      lines.Add(s);
    }
    int i = 0;
    while (i < lines.Count) {
      string[] format = lines[i].Split(' ');
      i++;
      BFEvent e = null;
      List<DialogueFragment> dialogues = new List<DialogueFragment>();
      int numLines = Int32.Parse(format[2]);
      for (int j = 0; j < numLines; j++) {
        dialogues.Add(constructDialogue(lines[i+j]));
      }
      i = i + numLines;
      if (format[0] == "time") {
        e = new TimeDialogue(Int32.Parse(format[1]), dialogues);
      } else if (format[0] == "bossTurn") {
        e = new TurnDialogue(GameManager.get.boss, Int32.Parse(format[1]), dialogues);
      } else if (format[0] == "broTurn") {
        // Brodric doesn't exist at this point, so send null and check on events
        e = new TurnDialogue(null, Int32.Parse(format[1]), dialogues);
      } else {
        Debug.AssertFormat(false, "Got a bad trigger identifier " + format[0] + " in file " + fileName);
      }
      inBattle.Add(e);
    }
  }

  Character getChar(string charName) {
    foreach(Character c in GameData.gameData.characters) {
      if (c.name == charName) {
        return c;
      }
    }
    return null;
  }

}
