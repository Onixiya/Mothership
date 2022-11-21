using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
namespace Mothership{
    public class Interceptor:ModTower{
        public override string DisplayName=>"Interceptor";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>0;
        public override bool DontAddToShop=>true;
        public override int TopPathUpgrades=>0;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string TowerSet=>"Primary";
        public override string Description=>"whatthefuckdoyouthinkyouaredoingreadingthislol";
        public override void ModifyBaseTowerModel(TowerModel interceptor){
            TowerModel plane=Game.instance.model.GetTowerFromId("BuccaneerLesserPlane");
            AttackModel planeAttack=plane.GetAttackModel();
            AttackModel interceptorAttack=interceptor.GetAttackModel();
            AttackFilterModel interceptorFilter=interceptorAttack.GetBehavior<AttackFilterModel>();
            WeaponModel interceptorWeapon=interceptorAttack.weapons[0];
            ProjectileModel interceptorProjectile=interceptorWeapon.projectile;
            interceptor.AddBehavior(plane.GetBehavior<AirUnitModel>().Duplicate());
            AirUnitModel interceptorAir=interceptor.GetBehavior<AirUnitModel>();
            interceptorAir.display=new(){guidRef="Carrier-InteceptorPrefab"};
            interceptorAir.behaviors[0].Cast<FighterMovementModel>().turningSpeed=0.06f;
            interceptorAttack.range=200;
            interceptorAttack.RemoveBehavior<RotateToTargetModel>();
            interceptorAttack.addsToSharedGrid=false;
            interceptorFilter.filters=planeAttack.GetBehavior<AttackFilterModel>().filters.Duplicate();
            interceptorFilter.filters[0].Cast<FilterTargetAngleModel>().fieldOfView=20;
            interceptorWeapon.AddBehavior(new FireFromAirUnitModel(""){name="FireFromAirUnitModel"});
            interceptorWeapon.rate=0.08f;
            interceptorWeapon.emission=planeAttack.weapons[0].emission.Duplicate();
            interceptorProjectile.display=new(){guidRef="6c11e1432d6321c44b216600b2cdbac6"};
            interceptorProjectile.GetDamageModel().damage=5;
            interceptorProjectile.GetBehavior<TravelStraitModel>().lifespan=1;
            interceptor.display=new(){guidRef=""};
            interceptor.ignoreTowerForSelection=true;
            interceptor.GetBehavior<DisplayModel>();
            interceptor.AddBehavior(new TowerExpireOnParentDestroyedModel(""){name="TowerExpireOnParentDestroyedModel"});
            interceptor.RemoveBehavior<CreateEffectOnPlaceModel>();
            interceptor.RemoveBehavior<CreateSoundOnTowerPlaceModel>();
            interceptor.RemoveBehavior<PlayAnimationIndexModel>();
        }
    }
}