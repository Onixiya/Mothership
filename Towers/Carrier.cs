namespace Mothership{
    public class Carrier:SC2Tower{
        public override string Name=>"Carrier";
		public override string Description=>"no";
        public override bool AddToShop=>false;
        public override Faction TowerFaction=>Faction.Protoss;
        public override int Order=>1;
		public override bool Upgradable=>false;
		public override bool ShowUpgradeMenu=>false;
       	public override Dictionary<string,Il2CppSystem.Type>Components=>new(){{"Carrier-Prefab",Il2CppType.Of<CarrierBehaviour>()}};
		[RegisterTypeInIl2Cpp]
		public class CarrierBehaviour:MonoBehaviour{
			public CarrierBehaviour(IntPtr ptr):base(ptr){}
			int SelectSound=0;
			public void PlaySelectSound(){
				if(SelectSound>5){
					SelectSound=0;
				}
				SelectSound+=1;
				PlaySound("Carrier-Select"+SelectSound);
			}
		}
		public override TowerModel[]GenerateTowerModels(){
			return new TowerModel[]{
				Base()
			};
		}
        public TowerModel Base(){
			TowerModel carrier=gameModel.GetTowerFromId("DartMonkey").Clone<TowerModel>();
			carrier.name=Name;
			carrier.baseId=Name;
            carrier.display=new(){guidRef="Carrier-Prefab"};
			carrier.portrait=new(){guidRef="Ui[Carrier-Portrait]"};
            carrier.radius=15;
            carrier.range=25;
            carrier.dontDisplayUpgrades=true;
			carrier.upgrades=new(0);
			List<Model>carrierBehav=carrier.behaviors.ToList();
			DisplayModel display=carrierBehav.GetModel<DisplayModel>();
            display.display=new(){guidRef=carrier.display.guidRef};
            display.positionOffset=new(0,-10,190);
			carrierBehav.Add(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
            carrierBehav.RemoveModel<AttackModel>();
            carrierBehav.Add(gameModel.GetTowerFromId("MonkeyBuccaneer-400").behaviors.GetModel<AttackModel>("Spawner").Clone<AttackModel>());
            WeaponModel carrierWeapon=carrierBehav.GetModel<AttackModel>().weapons[0];
            SubTowerFilterModel carrierFilter=carrierWeapon.behaviors.First(a=>a.GetIl2CppType().Name=="SubTowerFilterModel").Cast<SubTowerFilterModel>();
            carrierFilter.maxNumberOfSubTowers=8;
            carrierFilter.baseSubTowerId="Interceptor";
            carrierFilter.baseSubTowerIds[0]="Interceptor";
            carrierWeapon.projectile.behaviors.GetModel<CreateTowerModel>().tower=gameModel.GetTowerFromId("Interceptor");
			carrier.behaviors=carrierBehav.ToArray();
			return carrier;
        }
        public override void Create(Tower tower){
            PlaySound("Carrier-Birth");
        }
        public override void Select(Tower tower){
            tower.Node.graphic.GetComponent<CarrierBehaviour>().PlaySelectSound();
        }
    }
}