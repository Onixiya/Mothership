namespace Mothership{
    public class Carrier:SC2Tower{
        public override string Name=>"Tal'darim Carrier";
        public override bool AddToShop=>false;
        public override Faction TowerFaction=>Faction.Protoss;
        public override int Order=>1;
		public override bool Upgradable=>false;
		public override bool ShowUpgradeMenu=>false;
        public override string BundleName=>"carrier.bundle";
		public override TowerModel[]GenerateTowerModels(){
			return new TowerModel[]{
				Base()
			};
		}
        public TowerModel Base(){
			TowerModel carrier=gameModel.GetTowerFromId("DartMonkey").Clone<TowerModel>();
			carrier.name=Name;
			carrier.baseId=Name;
            carrier.display=new(Name+"-Prefab");
			carrier.portrait=new("Ui["+Name+"-Portrait]");
            carrier.radius=15;
            carrier.range=25;
            carrier.dontDisplayUpgrades=true;
			carrier.upgrades=new(0);
			List<Model>carrierBehav=carrier.behaviors.ToList();
			DisplayModel display=carrierBehav.GetModel<DisplayModel>();
            display.display=carrier.display;
            display.positionOffset=new(0,-10,190);
			carrierBehav.Add(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
            carrierBehav.RemoveModel<AttackModel>();
            carrierBehav.Add(SelectedSoundModel);
            carrierBehav.Add(gameModel.GetTowerFromId("MonkeyBuccaneer-400").behaviors.GetModel<AttackModel>("Spawner").Clone<AttackModel>());
            WeaponModel carrierWeapon=carrierBehav.GetModel<AttackModel>().weapons[0];
            SubTowerFilterModel carrierFilter=carrierWeapon.behaviors.First(a=>a.GetIl2CppType().Name=="SubTowerFilterModel").Cast<SubTowerFilterModel>();
            carrierFilter.maxNumberOfSubTowers=8;
            carrierFilter.baseSubTowerId="Tal'darim Interceptor";
            carrierFilter.baseSubTowerIds[0]="Tal'darim Interceptor";
            carrierWeapon.projectile.behaviors.GetModel<CreateTowerModel>().tower=gameModel.GetTowerFromId("Tal'darim Interceptor");
			carrier.behaviors=carrierBehav.ToArray();
            SetSounds(carrier,Identifier,false,true,false,false);
			return carrier;
        }
    }
}