using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FileBrowser : MonoBehaviour {
  private struct Selection {
    public Image image;
    public FileInfo file;
    public Selection(Image image, FileInfo file) {
      this.image = image;
      this.file = file;
    }
  }

  private Selection curSelection;
  public GameObject fileSelection;
  public Transform content;
  public InputField textField;
  public bool isSaveBrowser;
  public GameObject saveButton;
  public GameObject loadButton;

  public void createOptions(IEnumerable<FileInfo> files) {
    // fill scrollbar content area
    content.clear();
    foreach (FileInfo f in files) {
      GameObject o = GameObject.Instantiate(fileSelection, content);
      Text t = o.transform.Find("Text").gameObject.GetComponent<Text>();
      Image background = o.GetComponent<Image>();
      t.text = f.Name;
      Button b = o.GetComponent<Button>();
      Selection sel = new Selection(background, f);
      b.onClick.AddListener(() => {
        optionSelected(sel);
      });
    }
    curSelection = new Selection();

    // disable save/load buttons while nothing selected
    saveButton.SetActive(isSaveBrowser);
    loadButton.SetActive(! isSaveBrowser);
    textField.gameObject.SetActive(isSaveBrowser);
    gameObject.SetActive(true);
  }

  private void optionSelected(Selection sel){
    if (curSelection.image != null) {
      curSelection.image.color = Color.white;
    }
    curSelection = sel;
    curSelection.image.color = Color.red;
    textField.text = curSelection.file.Name;
    // enable button when something is selected
  }

  public void onTextFieldEnter(string filename) {
    SaveLoad.save(filename);
    gameObject.SetActive(false);
  }

  public void saveButtonClicked() {
    onTextFieldEnter(textField.text);
  }

  public void loadButtonClicked() {
    // do something with selected
    LoadingScreen.load("OverWorld");
    SaveLoad.load(curSelection.file.Name);
    gameObject.SetActive(false);
  }

  public void cancelButtonClicked() {
    gameObject.SetActive(false);
  }
}
