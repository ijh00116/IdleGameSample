using DLL_Common.Common;
using BlackTree.InGame;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree.UI.Event
{
    public class ShowDialogMsg : Message
    {

    }

    public class HideDialogMsg : Message
    {

    }
}

namespace BlackTree.Contents.Event
{
    public class EnterContentMsg : Message
    {

    }

    public class ExitContentMsg : Message
    {

    }
}

namespace BlackTree.Process.Event
{

}


namespace BlackTree.UI.Event
{
    public class CurrencyChange : Message
    {
        public BigInteger value;
        public CurrencyType Type;
        public bool CurrencyTypeSummarize;
        public CurrencyChange(CurrencyType _type,BigInteger _value)
        {
            Type = _type;
            value = _value;
        }
        public CurrencyChange()
        {
            value = 0;
            Type = CurrencyType.Gold;
        }
        public void Set(CurrencyType _type, BigInteger _value)
        {
            Type = _type;
            value = _value;
        }
    }
    public class PetAmountAdded:Message
    {
        public PetInventorySlot slot;
        public int AddCount;
        public PetAmountAdded()
        {
        }
        public PetAmountAdded(PetInventorySlot _slot,int add)
        {
            slot = _slot;
            AddCount = add;
        }
    }

    public class EarnMonsterGold: Message
    {
        public BigInteger value;
        public CurrencyType Type;
       
        public EarnMonsterGold()
        {
            value = 0;
            Type = CurrencyType.Gold;
        }
      
    }

    public class BoxChange:Message
    {
        public BigInteger value;
        public CurrencyType Type;

        public BoxChange(CurrencyType _type,BigInteger _value)
        {
            this.Type = _type;
            this.value = _value;
        }

        public BoxChange()
        {
            value = 0;
            Type = CurrencyType.WeaponBox_D;
        }

        public void Set(CurrencyType _type, BigInteger _value)
        {
            this.Type = _type;
            this.value = _value;
        }
    }

    public class GachaActivate:Message
    {
        public Dictionary<int, int> Itemvalue = new Dictionary<int, int>();
        public int EnforceJewelAmount = 0;
        public int magicstoneamount = 0;
        public ItemType _itemtype;
        public GachaButtonType _buttonType;
        public bool IsEvent=false;
        public void Clear()
        {
            EnforceJewelAmount = 0;
            Itemvalue.Clear();
        }
    }

    /// <summary>
    /// 캐릭터 데이터 체인지 되면 UI업데이트 되기위해 호출해주는거
    /// </summary>
    public class CharacterInfoUIUpdate:Message
    {

    }

    public class UserInfoUIUpdate : Message
    {
        public int EarnExp;
    }


    public class FlashPopup:Message
    {
        public string Eventmsg;
        public FlashPopup(string Msg)
        {
            Eventmsg = Msg;
        }
    }

    public class PopupItemInformationUI:Message
    {
        public ItemUIDisplay InvenSlotui;

        public PopupItemInformationUI(ItemUIDisplay _invenslotui)
        {
            this.InvenSlotui = _invenslotui;
        }
    }

   
    public class DungeonLevelTouch:Message
    {
        public DungeonPrime dungeondata;
        public bool CanEnter;
        public DungeonLevelTouch(DungeonPrime data,bool canenter)
        {
            dungeondata = data;
            CanEnter = canenter;
        }
    }

    public class DungeonEnd:Message
    {

    }

    public class DungeonStart:Message
    {

    }

    public class DungeonEndStartMain:Message
    {

    }
    public class PetDungeonLevelTouch : Message
    {
        public PetDungeonPrime dungeondata;
        public PetDungeonLevelTouch(PetDungeonPrime data)
        {
            dungeondata = data;
        }
    }

    public class PetDungeonEnd : Message
    {
        
    }

    public class EnemyInActive:Message
    {
        public CharacterType charactertype;
        public EnemyInActive(CharacterType _type)
        {
            charactertype = _type;
        }
    }

    public class TutorialUIpopup:Message
    {
        public TutorialTouch tutorialTouch;
        public System.Action callbackNext;
        public TutorialUIpopup(TutorialTouch _touch,System.Action _callbackNext)
        {
            tutorialTouch = _touch;
            this.callbackNext = _callbackNext;
        }
    }

    public class OtherUIPopup:Message
    {
        public GameObject PopupUI;
        public OtherUIPopup(GameObject meUI)
        {
            PopupUI = meUI;
        }
    }

    public class SideWindowPopup:Message
    {
        public SideButtonType type;
        public SideWindowPopup(SideButtonType _type)
        {
            type = _type;
        }
    }

    public class PetEquiped:Message
    {
        public PetData EquipedPet;
        public PetEquiped(PetData slot)
        {
            EquipedPet = slot;
        }
    }

    public class UsingSkillButtonPush:Message
    {
        public SkillType skillType;
        public CharacterType charactertype;
        public UsingSkillButtonPush(SkillType _st,CharacterType _type)
        {
            skillType = _st;
            charactertype = _type;
        }
    }

    public class SrelicLevelUpdate:Message
    {

    }

    public class ShapeChange:Message
    {
        public Character _character;
        public ItemType itemtype;
        public string itemName;
        public bool Equiped;
        public string SpineName;
        public ShapeChange(ItemType _type,string name,  bool equip)
        {
            itemtype = _type;
            itemName = name;
            Equiped = equip;
        }
    }

    public class PVPEnd:Message
    {
        public bool MeWin;
        public PVPEnd(bool win)
        {
            MeWin = win;
        }
    }

    public class pvpHpDamgaged:Message
    {
        public CharacterType charactertype;
        public float sliderValue;
        public pvpHpDamgaged(CharacterType character,float value)
        {
            charactertype = character;
            sliderValue = value;
        }
    }

    public class MainSceneBossEvent:Message
    {
        public MainSceneBossEvent()
        {
        }
    }

    public class MainSceneBosshpPopupEvent : Message
    {
        public MainSceneBosshpPopupEvent()
        {
        }
    }

    public class BossInfiniteChange:Message
    {

    }

    public class PetQuickStartButtonOnOff:Message
    {
        public bool On;
        public PetQuickStartButtonOnOff(bool on)
        {
            On = on;
        }
    }

    public class SideBtnNewIconActivate:Message
    {
        public SideButtonType buttonType;
        public bool IsHaveSomethingNew;
        public SideBtnNewIconActivate(SideButtonType btnType,bool _new)
        {
            buttonType = btnType;
            IsHaveSomethingNew= _new;
        }
    }

    public class SavingBatteryMessage:Message
    {
        public bool Open;
        public SavingBatteryMessage(bool _open)
        {
            Open = _open;
        }
    }

    public class PopupNetworkCancel : Message
    {

    }

    public class PopupGameExit : Message
    {

    }
    public class GlobalLvUp:Message
    {

    }
}

namespace BlackTree.InGame.Event
{
    public class BuffOn:Message
    {
        public bool On;
        public Attributes attribute;
        public BuffOn(Attributes ab,bool _on=true)
        {
           // skillData = _skilldata;
            attribute = ab;
            On = _on;
        }
    }

    public class CharacterAbilityUpdate : Message
    {
        public AbilitiesType abilType;
        public CharacterAbilityUpdate(AbilitiesType _type)
        {
            abilType = _type;
        }
    }

    public class EnemyKilled:Message
    {
        public GameObject DeadObj;
        public CharacterType charactertype;
        public int CharacterIdx=-1;
        public EnemyKilled(GameObject obj,CharacterType _type)
        {
            DeadObj = obj;
            charactertype = _type;
        }
    }

    public class BuffActivate:Message
    {
        public BuffType buffType;
        public int StartTime;

        public BuffActivate(BuffType _type,int time)
        {
            buffType = _type;
            StartTime = time;
        }
    }

    public class BuffTimer:Message
    {
        public BuffType Bufftype;
        public float ElapsedTime;
        public BuffTimer()
        {
            Bufftype = BuffType.AttackPower;
            ElapsedTime = 0;
        }
        public BuffTimer(BuffType _type,float _time)
        {
            Bufftype = _type;
            ElapsedTime = _time;
        }
    }

    public class MissionValueUpdate:Message
    {
        public MissionType missiontype;

        public MissionValueUpdate()
        {
        }
    }

    public class FairyPresentActive:Message
    {

    }

    
}

namespace BlackTree.Global.Event
{
    public class PlayEffectSoundMsg : Message
    {
        private BlackTree.SoundType _soundType = BlackTree.SoundType.None;
        public BlackTree.SoundType SoundType { get { return _soundType; } }

        public PlayEffectSoundMsg(BlackTree.SoundType sound)
        {
            _soundType = sound;
        }

    }
}