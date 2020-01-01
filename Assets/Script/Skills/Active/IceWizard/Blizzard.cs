using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Blizzard: CircleAoE {

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorIceWizard; }}

  public Blizzard() {
    range = 3;
    useWepRange = false;
    aoe = 5;
    useLos = false;
    name = "Blizzard";
    effectsTiles = false;
    maxCooldown = 2;
    dType = DamageType.magical;
    dEle = DamageElement.ice;

    targetAlly(true);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Deal " + tooltipDamage + " damage to all characters in the affected area";
  }}

  public override int damageFormula() {
    return (int)(attributes.intelligence*(1+level*0.1));
  }

  static GameObject effectObject = Resources.Load("ParticleEffects/Blizzard") as GameObject;
  public override void playAVEffects(Action callback, Tile target) {
    GameManager.get.waitFor(GameManager.get.StartCoroutine(AVTiming(callback, target)));
  }

  // Same as Firestorm
  IEnumerator AVTiming(Action callback, Tile target) {
    Vector3 stormLocation = target.position;
    stormLocation.y = stormLocation.y + 5f;
    GameObject storm = GameObject.Instantiate(effectObject, stormLocation, Quaternion.identity) as GameObject;
    ParticleSystem particleEffect = storm.GetComponent<ParticleSystem>();
    var shape = particleEffect.shape;
    shape.radius = aoe;
    yield return new WaitForSeconds(3f);
    callback();
    particleEffect.Stop();
    yield return new WaitForSeconds(3f);
    GameObject.Destroy(storm);
  }
}
