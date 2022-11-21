using UnityEngine;
namespace Mothership{
    public class SC2Tower{
        public SC2Tower(byte[]bundle,/*Dictionary<string,string>soundNames=null,*/Action<Weapon>attack=null,Action<string,Tower>upgrade=null,Action<string,Tower>ability=null,Action create=null,
            Action<Tower>select=null,Action<Tower>sell=null){
            TowerBundle=bundle;
            Bundle=AssetBundle.LoadFromMemory(bundle);
            /*Log(SoundNames!=null);
            if(SoundNames!=null){
                SoundNames=soundNames;
            }*/
            if(attack!=null){
                Attack=attack;
            }
            if(upgrade!=null){
                Upgrade=upgrade;
            }
            if(ability!=null){
                Ability=ability;
            }
            if(create!=null){
                Create=create;
            }
            if(select!=null){
                Select=select;
            }
            if(sell!=null){
                Sell=sell;
            }
        }
        public byte[]TowerBundle;
        public AssetBundle Bundle=null;
        public Action<Weapon>Attack;
        public Action<string,Tower>Upgrade;
        public Action<string,Tower>Ability;
        public Action Create;
        public Action<Tower>Select;
        public Action<Tower>Sell;
        public Dictionary<string,string>SoundNames;
    }
    [RegisterTypeInIl2Cpp]
    public class SC2Sound:MonoBehaviour{
        public SC2Sound(IntPtr ptr):base(ptr){}
        public int LastSelectQuote=0;
        public int LastUpgradeQuote=0;
        public int MaxSelectQuote=7;
        public int MaxUpgradeQuote=5;
        public string TowerName;
        public void Start(){
            string[]name=gameObject.name.Split('-');
            try{
                TowerName=towerTypes[name[0]].SoundNames[name[1]];
            }catch{}
            if(name[1].ToLower()==currentHero){
                MaxSelectQuote+=3;
                MaxUpgradeQuote+=2;
            }
        }
        public void PlaySelectSound(){
            LastSelectQuote++;
            if(LastSelectQuote==MaxSelectQuote){
                LastSelectQuote=1;
            }
            PlaySound(TowerName+"select"+LastSelectQuote);
        }
        public void PlayUpgradeSound(){
            LastUpgradeQuote++;
            if(LastUpgradeQuote==MaxUpgradeQuote){
                LastUpgradeQuote=1;
            }
            LastSelectQuote=1;
            PlaySound(TowerName+"upgrade"+LastUpgradeQuote);
        }
    }
}
