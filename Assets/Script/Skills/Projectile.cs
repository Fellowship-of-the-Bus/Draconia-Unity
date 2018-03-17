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
  Arrow,
  Fireball
}

public enum ProjectileMovementType {
  Straight,
  Parabolic
}

public class Projectile {
  protected Effected target;
  Action callback;
  GameObject projectile;
  Vector3 direction;
  Vector3 distance;

  static GameObject Arrow = Resources.Load("ArrowProjectile") as GameObject; //Must be a prefab containing an instance of the desired model named "Projectile"
  static GameObject Fireball = Resources.Load("FireballProjectile") as GameObject;

  static Dictionary<ProjectileType,GameObject> Projectiles = new Dictionary<ProjectileType,GameObject>() {
    {ProjectileType.None, null},
    {ProjectileType.Arrow, Projectile.Arrow},
    {ProjectileType.Fireball, Projectile.Fireball}
  };

  public Projectile(BattleCharacter source, Effected target, ProjectileType projType, ProjectileMovementType moveType, Action callback) {
    this.target = target;
    this.callback = callback;
    if (Projectiles[projType]) {
      projectile = GameObject.Instantiate(Projectiles[projType], source.leftHand) as GameObject;
    }
    else projectile = null;
    if (moveType == ProjectileMovementType.Straight) {
      distance = (target.transform.position - source.transform.position);
      direction = distance.normalized;
      GameManager.get.waitFor(GameManager.get.StartCoroutine(straightMove()));
    }// else if (moveType == ProjectileMovementType.Parabolic)
  }

  IEnumerator straightMove() {
    const float speed = 1.5f;
    if (projectile) {
      projectile.transform.SetParent(null);
      Quaternion angle = Quaternion.LookRotation(direction);
      projectile.transform.Find("Projectile").rotation = angle;
      direction = projectile.transform.InverseTransformDirection(direction);
      distance = projectile.transform.InverseTransformDirection(distance);
      Vector3 d = speed*(distance)/Options.FPS;
      for (int i = 0; i < Options.FPS/speed; i++) {
        projectile.transform.Translate(d);
        yield return new WaitForSeconds(1/Options.FPS);
      }
    }
    GameObject.Destroy(projectile);
    callback();
  }

  IEnumerator parabolicMove() {
    yield return null;
  }
}