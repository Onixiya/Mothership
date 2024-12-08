namespace Mothership{
    public class Interceptor:SC2Tower{
        public override string Name=>"Interceptor";
        public override bool AddToShop=>false;
        public override Faction TowerFaction=>Faction.Protoss;
		public override bool Upgradable=>false;
		public override TowerModel[]GenerateTowerModels(){
			return new TowerModel[]{
				Base()
			};
		}
		public override bool HasBundle=>false;
        public TowerModel Base(){
			TowerModel interceptor=gameModel.GetTowerFromId("DartMonkey").Clone<TowerModel>();
			interceptor.name=Name;
			interceptor.baseId=Name;
			interceptor.display=new("");
            interceptor.ignoreTowerForSelection=true;
            interceptor.dontDisplayUpgrades=true;
            interceptor.isSubTower=true;
            interceptor.upgrades=new(0);
			List<Model>interceptorBehav=interceptor.behaviors.ToList();
            interceptorBehav.Add(new TowerExpireOnParentDestroyedModel(""){name="TowerExpireOnParentDestroyedModel"});
            interceptorBehav.RemoveModel<CreateEffectOnPlaceModel>();
            interceptorBehav.RemoveModel<CreateSoundOnTowerPlaceModel>();
            interceptorBehav.RemoveModel<PlayAnimationIndexModel>();
            TowerModel plane = gameModel.GetTowerFromId("MonkeyBuccaneer-400").behaviors.GetModel<AttackModel>("Spawner").
                weapons[0].projectile.behaviors.GetModel<CreateTowerModel>().tower.Clone<TowerModel>();
			interceptorBehav.Add(plane.behaviors.GetModel<AirUnitModel>());
            AirUnitModel interceptorAir=interceptorBehav.GetModel<AirUnitModel>();
            interceptorAir.display=new("Carrier-InteceptorPrefab");
            interceptorAir.behaviors[0].Cast<FighterMovementModel>().turningSpeed=0.06f;
			AttackModel interceptorAttack=interceptorBehav.GetModel<AttackModel>();
            interceptorAttack.range=200;
			List<Model>interceptorAttackBehav=interceptorAttack.behaviors.ToList();
            interceptorAttackBehav.RemoveModel<RotateToTargetModel>();
			interceptorAttack.behaviors=interceptorAttackBehav.ToArray();
            interceptorAttack.addsToSharedGrid=false;
			AttackFilterModel interceptorAttackFilter=interceptorAttack.behaviors.GetModel<AttackFilterModel>();
			AttackModel planeAttack=plane.behaviors.GetModel<AttackModel>();
            interceptorAttackFilter.filters=planeAttack.behaviors.GetModel<AttackFilterModel>().filters;
            interceptorAttackFilter.filters[0].Cast<FilterTargetAngleModel>().fieldOfView=20;
			WeaponModel interceptorWeapon=interceptorAttack.weapons[0];
			interceptorWeapon.behaviors=new List<WeaponBehaviorModel>(){new FireFromAirUnitModel(""){name="FireFromAirUnitModel"}}.ToArray();
            interceptorWeapon.rate=0.08f;
            interceptorWeapon.emission=planeAttack.weapons[0].emission;
			ProjectileModel interceptorProjectile=interceptorWeapon.projectile;
            interceptorProjectile.display=new("6c11e1432d6321c44b216600b2cdbac6");
			Il2CppReferenceArray<Model>interceptorProjectileBehav=interceptorProjectile.behaviors;
            interceptorProjectileBehav.GetModel<DamageModel>().damage=5;
            interceptorProjectileBehav.GetModel<TravelStraitModel>().lifespan=1;
			interceptor.behaviors=interceptorBehav.ToArray();
			return interceptor;
        }
    }
}