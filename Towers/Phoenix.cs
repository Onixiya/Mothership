namespace Mothership{
    public class Phoenix:SC2Tower{
        public override string Name=>"Phoenix";
        public override bool AddToShop=>false;
        public override string Description=>"no";
		public override bool Upgradable=>false;
		public override Faction TowerFaction=>Faction.Protoss;
		public override TowerModel[]GenerateTowerModels(){
			return new TowerModel[]{
				Base()
			};
		}
        public TowerModel Base(){
			TowerModel phoenix=gameModel.GetTowerFromId("HeliPilot").Clone<TowerModel>();
			phoenix.name=Name;
			phoenix.baseId=phoenix.name;
			phoenix.targetTypes=new(new TargetType[]{new(){id="Pursuit",isActionable=false,actionOnCreate=false,intID=-1}});
            phoenix.TargetTypes=phoenix.targetTypes;
            phoenix.display=new(){guidRef=""};
			List<Model>phoenixBehav=phoenix.behaviors.ToList();
			phoenixBehav.Add(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
			phoenixBehav.GetModel<DisplayModel>().display=phoenix.display;
            AirUnitModel phoenixAir=phoenixBehav.GetModel<AirUnitModel>();
			phoenixAir.display=new(){guidRef="Phoenix-Prefab"};
            HeliMovementModel phoenixMove=phoenixAir.behaviors[0].Cast<HeliMovementModel>();
			phoenixMove.maxSpeed=120;
            phoenixMove.rotationSpeed=0.115f;
            phoenixMove.strafeDistance=0;
            phoenixMove.movementForceEnd=99999;
            phoenixMove.otherHeliRepulsionRange=5;
            AttackModel phoenixAttack=phoenixBehav.GetModel<AttackModel>();
			List<Model>phoenixAttackBehav=phoenixAttack.behaviors.ToList();
			phoenixAttackBehav.RemoveModel<PatrolPointsSettingModel>();
            phoenixAttackBehav.RemoveModel<FollowTouchSettingModel>();
            phoenixAttackBehav.RemoveModel<LockInPlaceSettingModel>();
            phoenixAttackBehav.Add(new PursuitSettingModel("",false,0,false){name="PursuitSettingModel",isSelectable=false,pursuitDistance=30,isOnSubTower=false});
            WeaponModel phoenixWeapon=phoenixAttack.weapons[0];
            phoenixWeapon.rate=0.35f;
			ProjectileModel phoenixProjectile=phoenixWeapon.projectile;
            phoenixProjectile.pierce=1;
            phoenixProjectile.display=new(){guidRef="6c11e1432d6321c44b216600b2cdbac6"};
			phoenixProjectile.behaviors.GetModel<DamageModel>().damage=7;
			return phoenix;
        }
    }
}