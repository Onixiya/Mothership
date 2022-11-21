using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
namespace Mothership{
    public class Carrier:ModTower{
        public override string DisplayName=>"Carrier";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>0;
        public override bool DontAddToShop=>true;
        public override int TopPathUpgrades=>0;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string TowerSet=>"Primary";
        protected override int Order=>1;
        public override string Description=>"whatthefuckdoyouthinkyouaredoingreadingthislol";
        public override SpriteReference PortraitReference=>new(){guidRef="Ui[Carrier-Portrait]"};
        public static Dictionary<string,string>SoundNames=new(){{"Carrier","carrier-"}};
        public static SC2Tower SC2Data=new(Bundles.Bundles.carrier,/*null,*/null,null,null,Create,Select);
        public override void ModifyBaseTowerModel(TowerModel carrier){
            DisplayModel display=carrier.GetBehavior<DisplayModel>();
            carrier.display=new(){guidRef="Carrier-Carrier-Prefab"};
            carrier.AddBehavior(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
            carrier.radius=15;
            carrier.range=25;
            carrier.areaTypes=flyingAreaType;
            carrier.dontDisplayUpgrades=true;
            display.display=new(){guidRef=carrier.display.guidRef};
            display.positionOffset=new(0,-10,190);
            carrier.RemoveBehavior<AttackModel>();
            carrier.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyBuccaneer-400").GetAttackModels().First(a=>a.name.Contains("Spawner")));
            WeaponModel carrierWeapon=carrier.GetAttackModel().weapons[0];
            SubTowerFilterModel carrierFilter=carrierWeapon.GetBehavior<SubTowerFilterModel>();
            carrierFilter.maxNumberOfSubTowers=8;
            carrierFilter.baseSubTowerId="Mothership-Interceptor";
            carrierFilter.baseSubTowerIds[0]="Mothership-Interceptor";
            carrierWeapon.projectile.GetBehavior<CreateTowerModel>().tower=Game.instance.model.GetTowerFromId("Mothership-Interceptor");
        }
        public static void Create(){
            PlaySound("carrier-birth");
        }
        public static void Select(Tower tower){
            tower.Node.graphic.GetComponent<SC2Sound>().PlaySelectSound();
        }
    }
}