using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class FireStorm: CircleAoE {
  public new bool targetsTiles = true;

  public override string animation { get { return "Cast"; }}
  public override Color castColor { get { return castColorPyromancer; }}

  public FireStorm() {
    range = 3;
    useWepRange = false;
    aoe = 2;
    useLos = false;
    name = "Fire Storm";
    effectsTiles = false;
    maxCooldown = 2;

    dType = DamageType.magical;
    dEle = DamageElement.fire;
    targetAlly(false);
    targetEnemy(true);
  }

  protected override string tooltipDescription { get {
    return "Summon a storm of flames from the sky dealing " + tooltipDamage + " damage."
    + " Does not require line of sight.";
  }}

  public override int damageFormula() {
    return (int)(attributes.intelligence*(1+level*0.1));
  }

  static GameObject effectObject = Resources.Load("ParticleEffects/FireStorm") as GameObject;
  public override void playAVEffects(Action callback, Tile target) {
    GameManager.get.waitFor(GameManager.get.StartCoroutine(AVTiming(callback, target)));
  }

  // Same as Blizzard TODO: share code
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
