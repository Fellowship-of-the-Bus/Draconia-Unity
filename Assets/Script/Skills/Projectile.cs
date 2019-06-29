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
  FlameArrow,
  Fireball,
  FireLance,
  HealingRay,
  IceSpear
}

public enum ProjectileMovementType {
  Straight,
  Parabolic,
  Laser
}

public class Projectile {
  protected BattleCharacter source;
  protected Effected target;
  Action callback;
  GameObject projectile;
  Vector3 direction;

  static GameObject Arrow = Resources.Load("Projectiles/Arrow") as GameObject; // Must be a prefab containing an instance of the desired model named "Projectile"
  static GameObject FlameArrow = Resources.Load("Projectiles/FlameArrow") as GameObject;
  static GameObject Fireball = Resources.Load("Projectiles/Fireball") as GameObject;
  static GameObject FireLance = Resources.Load("Projectiles/FireLance") as GameObject;
  static GameObject HealingRay = Resources.Load("Projectiles/HealingRay") as GameObject;
  static GameObject IceSpear = Resources.Load("Projectiles/IceSpear") as GameObject;

  static Dictionary<ProjectileType,GameObject> Projectiles = new Dictionary<ProjectileType,GameObject>() {
    {ProjectileType.None, null},
    {ProjectileType.Arrow, Projectile.Arrow},
    {ProjectileType.FlameArrow, Projectile.FlameArrow},
    {ProjectileType.Fireball, Projectile.Fireball},
    {ProjectileType.FireLance, Projectile.FireLance},
    {ProjectileType.HealingRay, Projectile.HealingRay},
    {ProjectileType.IceSpear, Projectile.IceSpear}
  };

  static GameObject FireballExplosion = Resources.Load("Projectiles/FireballExplosion") as GameObject;
  static GameObject IceShatter = Resources.Load("Projectiles/IceShatter") as GameObject;

  static Dictionary<ProjectileType,GameObject> ImpactEffects = new Dictionary<ProjectileType,GameObject>() {
    {ProjectileType.None, null},
    {ProjectileType.Arrow, null},
    {ProjectileType.FlameArrow, null},
    {ProjectileType.Fireball, Projectile.FireballExplosion},
    {ProjectileType.FireLance, null},
    {ProjectileType.HealingRay, null},
    {ProjectileType.IceSpear, Projectile.IceShatter}
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
      projectile.transform.SetParent(null);
      if (moveType == ProjectileMovementType.Laser) {
        LineRenderer laser = projectile.GetComponent<LineRenderer>();

        laser.SetPosition(0, source.transform.position);
        laser.SetPosition(1, target.transform.position);
        yield return new WaitForSeconds(1);
      } else {
        bool isParabolic = moveType == ProjectileMovementType.Parabolic;

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
    }
    callback();

    if (projectile) {
      // Post collision effects

      // Play impact effects
      if (ImpactEffects[projType]) {
        GameManager.get.waitFor(GameManager.get.StartCoroutine(playImpact(projType)));
      }

      // Wind down particle emitters
      switch (projType) {
        case ProjectileType.Fireball:
        case ProjectileType.FireLance:
          projectile.GetComponent<ParticleSystem>().Stop();
          yield return new WaitForSeconds(1);
          break;
      }

      GameObject.Destroy(projectile);
    }
  }

  IEnumerator playImpact(ProjectileType projType) {
    Quaternion angle = Quaternion.LookRotation(direction);
    GameObject impactEffect = GameObject.Instantiate(
      ImpactEffects[projType],
      target.transform.position,
      angle
    ) as GameObject;

    yield return new WaitForSeconds(1);

    GameObject.Destroy(impactEffect);
  }
}
