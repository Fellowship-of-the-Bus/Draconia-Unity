using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothColorChanger : MonoBehaviour {
  public enum Mode {
    Sequential, Random
  }

  public Color[] colors;
  public new Renderer renderer;
  public int durationInFrames;
  public Mode mode = Mode.Sequential;

  private int currentFrame = 0;
  private int currentColor = 0;
  private int nextColor = 0;
  private static System.Random rand = new System.Random();

  void Start() {
    switch (mode) {
      case Mode.Sequential:
        currentColor = 0;
        nextColor = 1;
        break;
      case Mode.Random:
        currentColor = rand.Next(colors.Length);
        nextColor = rand.Next(colors.Length);
        break;
    }
  }

  void OnValidate() {
    if (colors.Length < 2) {
      Debug.Log("Smooth Color Changer requires at least 2 colors");
    }
  }

  void Update() {
    Color newColor = Color.Lerp(colors[currentColor], colors[nextColor], (float)currentFrame/durationInFrames);
    renderer.materials[2].color = newColor;
    renderer.materials[3].color = newColor;
    currentFrame++;
    if (currentFrame == durationInFrames) {
      currentFrame = 0;
      setNextColor();
    }
  }

  private void setNextColor() {
    currentColor = nextColor;
    switch (mode) {
      case Mode.Sequential:
        nextColor = (currentColor+1) % colors.Length;
        break;
      case Mode.Random:
        nextColor = rand.Next(colors.Length);
        break;
    }
  }
}
