namespace Mothership{
    public class Destroyer:SC2Tower{
        public override string Name=>"Destroyer";
        public override bool AddToShop=>false;
        public override Faction TowerFaction=>Faction.Protoss;
		public override bool Upgradable=>false;
		public override bool ShowUpgradeMenu=>false;
        public override string BundleName=>"destroyer.bundle";
		public override TowerModel[]GenerateTowerModels(){
			return new TowerModel[]{
				Base()
			};
		}
        public TowerModel Base(){
			TowerModel destroyer=gameModel.GetTowerFromId("DartMonkey").Clone<TowerModel>();
			destroyer.name=Name;
			destroyer.baseId=Name;
			destroyer.radius=15;
            destroyer.range=65;
            destroyer.dontDisplayUpgrades=true;
            destroyer.display=new(Name+"-Prefab");
			destroyer.upgrades=new(0);
            destroyer.portrait=new("Ui["+Name+"-Portrait]");
			List<Model>destroyerBehav=destroyer.behaviors.ToList();
            destroyerBehav.Add(SelectedSoundModel);
			destroyerBehav.Add(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
            DisplayModel display=destroyerBehav.GetModel<DisplayModel>();
			display.display=destroyer.display;
            display.positionOffset=new(0,0,190);
            AttackModel destroyerAttack=destroyerBehav.GetModel<AttackModel>();
            destroyerAttack.range=destroyer.range;
            destroyerAttack.weapons[0]=gameModel.GetTowerFromId("BallOfLightTower").behaviors.GetModel<AttackModel>().weapons[0].Clone<WeaponModel>();
            ProjectileModel destroyerProj=destroyerAttack.weapons[0].projectile;
			List<Model>destroyerProjBehav=destroyerProj.behaviors.ToList();
            destroyerProjBehav.GetModel<DamageModel>().damage=0.6f;
            destroyerProjBehav.Add(gameModel.GetTowerFromId("BombShooter").behaviors.GetModel<AttackModel>().weapons[0].projectile.behaviors.GetModel<CreateProjectileOnContactModel>());
            destroyerProjBehav.GetModel<CreateProjectileOnContactModel>().projectile=gameModel.GetTowerFromId("Druid-200").behaviors.GetModel<AttackModel>().weapons[1].projectile.Clone<ProjectileModel>();
            Il2CppReferenceArray<Model>beamBehav=destroyerProjBehav.GetModel<CreateProjectileOnContactModel>().projectile.behaviors;
            LightningModel lightning=beamBehav.GetModel<LightningModel>();
            lightning.splits=1;
            lightning.splitRange=10;
            beamBehav.GetModel<DamageModel>().damage=0.45f;
            beamBehav.GetModel<CreateLightningEffectModel>().lifeSpan=0.1f;
			destroyer.behaviors=destroyerBehav.ToArray();
            SetSounds(destroyer,Name,true,true,false,false);
			return destroyer;
        }
    }
}