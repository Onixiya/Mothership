using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
namespace Mothership{
    public class Destroyer:ModTower{
        public override string DisplayName=>"Destroyer";
        public override string BaseTower=>"DartMonkey";
        public override int Cost=>0;
        public override bool DontAddToShop=>true;
        public override int TopPathUpgrades=>0;
        public override int MiddlePathUpgrades=>0;
        public override int BottomPathUpgrades=>0;
        public override string TowerSet=>"Primary";
        public override string Description=>"whatthefuckdoyouthinkyouaredoingreadingthislol";
        public override SpriteReference PortraitReference=>new(){guidRef="Ui[Destroyer-Portrait]"};
        public static Dictionary<string,string>SoundNames=new(){{"Destroyer","destroyer-"}};
        public static SC2Tower SC2Data=new(Bundles.Bundles.destroyer,/*null,*/null,null,null,Create,Select);
        public override void ModifyBaseTowerModel(TowerModel destroyer){
            DisplayModel display=destroyer.GetBehavior<DisplayModel>();
            AttackModel attack=destroyer.GetAttackModel();
            destroyer.radius=15;
            destroyer.range=65;
            destroyer.areaTypes=flyingAreaType;
            destroyer.dontDisplayUpgrades=true;
            destroyer.display=new(){guidRef="Destroyer-Destroyer-Prefab"};
            destroyer.AddBehavior(new TowerExpireModel("",0,0,false,false){name="TowerExpireModel",lifespan=30,rounds=9999,expireOnDefeatScreen=false,expireOnRoundComplete=false});
            display.display=new(){guidRef=destroyer.display.guidRef};
            display.positionOffset=new(0,0,190);
            attack.range=destroyer.range;
            attack.weapons[0]=Game.instance.model.GetTowerFromId("BallOfLightTower").GetAttackModel().weapons[0].Duplicate();
            ProjectileModel proj=attack.weapons[0].projectile;
            proj.GetDamageModel().damage=0.6f;
            proj.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
            proj.GetBehavior<CreateProjectileOnContactModel>().projectile=Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].projectile.Duplicate();
            ProjectileModel beam1=proj.GetBehavior<CreateProjectileOnContactModel>().projectile;
            LightningModel lightning=beam1.GetBehavior<LightningModel>();
            lightning.splits=1;
            lightning.splitRange=10;
            beam1.GetDamageModel().damage=0.45f;
            beam1.GetBehavior<CreateLightningEffectModel>().lifeSpan=0.1f;
        }
        public static void Create(){
            PlaySound("destroyer-birth");
        }
        public static void Select(Tower tower){
            tower.Node.graphic.GetComponent<SC2Sound>().PlaySelectSound();
        }
    }
}