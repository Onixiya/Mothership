namespace Mothership{
    public class Carrier:SC2Tower{
        public override string Name=>"TaldarimCarrier";
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
            carrier.mods=new(0);
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
			carrierBehav.Add(new TowerExpireModel("TowerExpireModel",30,9999,false,false));
            carrierBehav.RemoveModel<AttackModel>();
            carrierBehav.Add(SelectedSoundModel);
            carrierBehav.Add(gameModel.GetTowerFromId("MonkeyBuccaneer-400").behaviors.GetModel<AttackModel>("Spawner").Clone<AttackModel>());
            WeaponModel carrierWeapon=carrierBehav.GetModel<AttackModel>().weapons[0];
            SubTowerFilterModel carrierFilter=carrierWeapon.behaviors.GetModel<SubTowerFilterModel>();
            carrierFilter.maxNumberOfSubTowers=8;
            carrierFilter.baseSubTowerId="TaldarimInterceptor";
            carrierFilter.baseSubTowerIds[0]="TaldarimInterceptor";
            carrierWeapon.projectile.behaviors.GetModel<CreateTowerModel>().tower=gameModel.GetTowerFromId("TaldarimInterceptor");
			carrier.behaviors=carrierBehav.ToArray();
            SetSounds(carrier,Name+"-",false,true,false,false);
            LocManager.textTable.Add("TaldarimCarrier","Tal'darim Carrier");
			return carrier;
        }
    }
}