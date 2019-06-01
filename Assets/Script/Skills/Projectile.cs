using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

public enum ProjectileType {
  None,
  Arrow,
  Fireball,
  FireLance,
  HealingRay,
  IceSpear
}

public enum ProjectileMovementType {
  Straight,
  Parabolic
}

public class Projectile {
  protected BattleCharacter source;
  protected Effected target;
  Action callback;
  GameObject projectile;
  Vector3 direction;

  static GameObject Arrow = Resources.Load("Projectiles/Arrow") as GameObject; //Must be a prefab containing an instance of the desired model named "Projectile"
  static GameObject Fireball = Resources.Load("Projectiles/Fireball") as GameObject;
  static GameObject FireLance = Resources.Load("Projectiles/FireLance") as GameObject;
  static GameObject HealingRay = Resources.Load("Projectiles/HealingRay") as GameObject;
  static GameObject IceSpear = Resources.Load("Projectiles/IceSpear") as GameObject;

  static Dictionary<ProjectileType,GameObject> Projectiles = new Dictionary<ProjectileType,GameObject>() {
    {ProjectileType.None, null},
    {ProjectileType.Arrow, Projectile.Arrow},
    {ProjectileType.Fireball, Projectile.Fireball},
    {ProjectileType.FireLance, Projectile.FireLance},
    {ProjectileType.HealingRay, Projectile.HealingRay},
    {ProjectileType.IceSpear, Projectile.IceSpear}
  };

  public Projectile(BattleCharacter source, Effected target, ProjectileType projType, ProjectileMovementType moveType, float speed, Action callback) {
    this.source = source;
    this.target = target;
    this.callback = callback;
    if (Projectiles[projType]) {
      projectile = GameObject.Instantiate(Projectiles[projType]) as GameObject;
      projectile.transform.position = source.model.leftHand.position;
    } else {
      projectile = null;
    }

    direction = (target.transform.position - source.transform.position).normalized;
    GameManager.get.waitFor(GameManager.get.StartCoroutine(move(projType, moveType, speed)));
  }

  IEnumerator move(ProjectileType projType, ProjectileMovementType moveType, float speed) {
    const float height = 2f;
    if (projectile) {
      bool isParabolic = moveType == ProjectileMovementType.Parabolic;
      projectile.transform.SetParent(null);

      Transform proj = projectile.transform.Find("Projectile");
      if (proj == null) {
        proj = projectile.transform;
      }

      Vector3 startDirection = isParabolic ? direction + new Vector3(0,height,0) : direction;
      Quaternion angle = Quaternion.LookRotation(startDirection);
      proj.rotation = angle;

      yield return GameManager.get.moveObject(
        projectile,
        speed,
        projectile.transform.position,
        target.transform.position,
        0,
        1,
        null,
        height,
        (t) => {
          const float totalRotation = 90f;
          proj.rotation = angle;
          proj.Rotate(Vector3.right,totalRotation*t);
        },
        false,
        isParabolic
      );
    }
    callback();

    if (projectile) {
      // Post collision effects
      switch (projType) {
        case ProjectileType.Fireball:
        case ProjectileType.FireLance:
        case ProjectileType.HealingRay:
          projectile.GetComponent<ParticleSystem>().Stop();
          yield return new WaitForSeconds(1);
          break;
      }
      GameObject.Destroy(projectile);
    }
  }
}
