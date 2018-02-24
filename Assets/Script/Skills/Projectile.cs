using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;
using System.Linq;

public enum ProjectileType {
  None,
  Arrow
}


public class Projectile {
  protected Effected target;
  Action callback;
  GameObject projectile;
  Vector3 direction;

  static GameObject Arrow = Resources.Load("ArrowProjectile") as GameObject; //Must be a prefab containing an instance of the desired model named "Projectile"

  static Dictionary<ProjectileType,GameObject> Projectiles = new Dictionary<ProjectileType,GameObject>() {
    {ProjectileType.None, null},
    {ProjectileType.Arrow, Projectile.Arrow}
  };

  public Projectile(BattleCharacter source, Effected target, ProjectileType projType, Action callback) {
    this.target = target;
    if (Projectiles[projType]) {
      projectile = GameObject.Instantiate(Projectiles[projType], source.leftHand) as GameObject;
      direction = (target.transform.position - source.transform.position).normalized;
      Debug.Log(Vector3.up);
      Quaternion angle = Quaternion.LookRotation(direction);
      projectile.transform.Find("Projectile").rotation = angle;
      Debug.Log(projectile.transform.Find("Projectile").rotation);
    }
    else projectile = null;
    this.callback = callback;
    GameManager.get.waitFor(GameManager.get.StartCoroutine(move()));
  }

  IEnumerator move() {
    const float speed = 0.5f;
    if (projectile) {
      projectile.transform.SetParent(null);
      Quaternion angle = Quaternion.LookRotation(direction);
      projectile.transform.Find("Projectile").rotation = angle;
      direction = projectile.transform.InverseTransformDirection(direction);
      Vector3 d = speed*(direction)/Options.FPS;
      for (int i = 0; i < Options.FPS/speed; i++) {
        projectile.transform.Translate(d);
        yield return new WaitForSeconds(1/Options.FPS);
      }
    }
    GameObject.Destroy(projectile);
    callback();
  }
}