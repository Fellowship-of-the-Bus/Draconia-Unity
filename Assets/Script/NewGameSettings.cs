using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "NewGameSettings", order = 100)]
public class NewGameSettings : ScriptableObject {
  public int numGeneratedCharacters;
  public WeaponData[] weapons;
  public ArmourData[] armor;
}
