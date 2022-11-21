using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Weapons;
namespace Mothership{
    public class Phoenix:ModTower{
        public override string DisplayName=>"Phoenix";
        public override string BaseTower=>"HeliPilot";
        public override int Cost=>0;
        public override bool DontAddToShop=>true;
        public override int TopPathUpgrades=>0;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string TowerSet=>"Primary";
        public override string Description=>"whatthefuckdoyouthinkyouaredoingreadingthislol";
        public override SpriteReference PortraitReference=>new(){guidRef=""};
        public static SC2Tower SC2Data=new(Bundles.Bundles.phoenix);
        public override void ModifyBaseTowerModel(TowerModel phoenix){
            AirUnitModel phoenixAir=phoenix.GetBehavior<AirUnitModel>();
            HeliMovementModel phoenixMove=phoenixAir.behaviors[0].Cast<HeliMovementModel>();
            AttackModel phoenixAttack=phoenix.GetAttackModel();
            WeaponModel phoenixWeapon=phoenixAttack.weapons[0];
            ProjectileModel phoenixProjectile=phoenixWeapon.projectile;
            phoenix.targetTypes=new(new TargetType[]{new(){id="Pursuit",isActionable=false,actionOnCreate=false,intID=-1}});
            phoenix.TargetTypes=phoenix.targetTypes;
            phoenix.display=new(){guidRef=""};
            phoenix.GetBehavior<DisplayModel>().display=new(){guidRef=""};
            phoenix.AddBehavior(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
            phoenixAir.display=new(){guidRef="Phoenix-Phoenix-Prefab"};
            phoenixMove.maxSpeed=120;
            phoenixMove.rotationSpeed=0.115f;
            phoenixMove.strafeDistance=0;
            phoenixMove.movementForceEnd=99999;
            phoenixMove.otherHeliRepulsionRange=5;
            phoenix.GetAttackModel().RemoveBehavior<PatrolPointsSettingModel>();
            phoenix.GetAttackModel().RemoveBehavior<FollowTouchSettingModel>();
            phoenix.GetAttackModel().RemoveBehavior<LockInPlaceSettingModel>();
            phoenixAttack.AddBehavior(new PursuitSettingModel("",false,0,false){name="PursuitSettingModel",isSelectable=false,pursuitDistance=30,isOnSubTower=false});
            phoenixWeapon.rate=0.35f;
            phoenixProjectile.pierce=1;
            phoenixProjectile.GetDamageModel().damage=7;
            phoenixProjectile.display=new(){guidRef="6c11e1432d6321c44b216600b2cdbac6"};
        }
    }
}