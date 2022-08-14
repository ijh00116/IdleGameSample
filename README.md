# KnightRush
- Valkirie 키우기 포트폴리오 입니다(`해당 프로젝트는 스크립트만 존재하여 플레이 진행이 되지 않습니다.`)
- 방치형게임의 핵심 로직내용과 플레이팹(Playfab)연동 코드가 첨부되어 있습니다.
- 현재 출시되어 있으며 출시 직전까지 개발을 맡아 하였습니다. 개발인원 4명(아트디자이너1명,기획자1명,애니메이터1명,개발자1명(본인))
- [발키리 소녀 키우기](https://play.google.com/store/apps/details?id=com.HighSpirit.KnightRush):해당 링크에서 게임을 다운받아 즐겨보실수 있습니다.

## Index

1. [JsonParsing](#jsonparsing)
2. [재화관리](#재화관리)
3. [미션관리](#미션관리)
4. [튜토리얼 시스템](#튜토리얼시스템)
5. [인벤토리 관리](#인벤토리관리)


## JsonParsing

### 소개 
- 각종 데이터 관리에 필요한 테이블을 json으로 관리하기에 json으로 게임에 필요한 데이터를 게임 시작시 세팅하여 사용합니다.
<details>
<summary>
    <span style="color:#008000"> <JsonParsing 코드,테이블관리 내용 보기> </span>
</summary>
    <div markdown="1">
     
- json에 사용되는 엑셀 예시(미션)

| idx | name | mission_type | mission_value | reward_type | reward_count |
| --- | ---- | ------------ | ------------- | ----------- | ------------ |
|50001|	m_daily_name_001 |	MISSION_CLEAR|	6|	DIAMOND|	100|
|50002|	m_daily_name_002	|MONSTER_KILL|	30|	DIAMOND	|20|
|50003|	m_daily_name_003	|GACHA_COUNT	|5	|DIAMOND	|20|
|50004|	m_daily_name_004	|MONSTER_KILL	|2|	DIAMOND	|20|

- JsonParsing 코드

```code
  public class DailyMissionDesc
    {
        public int idx;
        public string name;
        public MissionType mission_type;

        public int mission_value;
        public RewardType reward_type;
        public int reward_count;
    }
...
  T ReadData<T>(string fileName)
    {
        var path = new System.Text.StringBuilder();
        path.Append(Table_PATH);
        path.Append(fileName);

        TextAsset jsonString = Resources.Load<TextAsset>(path.ToString());

        if (jsonString != null)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString.text);
        }
        return default;
    }
```
</div>
</details>

## 재화관리
- `옵저버 패턴`을 사용해 재화가 변화될때 이벤트가 등록된 곳에 재화의 변화를 전달한다.
<details>
<summary>
    `재화관리 내용 보기`
</summary>
<div markdown="1">

```code
   public class GlobalCurrency 
    {
        CurrencyChange currencyMsg;
        public Dictionary<CurrencyType, Currency> currencylist = new Dictionary<CurrencyType, Currency>();

        public void Init()
        {
            currencyMsg = new CurrencyChange();
        }
        public Currency GetCurrency(CurrencyType _CurrenyType)
        {
            Currency _currency = null;
            if (currencylist.ContainsKey(_CurrenyType))
            {
                _currency=currencylist[_CurrenyType];
            }
            else
            {
                _currency = new Currency() { currencyType = _CurrenyType, value = 0 };
                currencylist.Add(_CurrenyType,_currency);
            }

            return _currency;
        }
        
        public void UpdateCurrency(CurrencyType _CurrenyType, int _value)
        {
            var updateCurreny = GetCurrency(_CurrenyType);
         
            if (null == updateCurreny)
            {
                currencylist.Add(_CurrenyType ,new Currency() { currencyType = _CurrenyType, value = _value});
            }
            else
            {
                updateCurreny.value = _value;
            }

            currencyMsg.Set(_CurrenyType, _value);

            Message.Send<CurrencyChange>(currencyMsg);
        }
    }
```

</div>
</details>

## 미션관리
- 미션은 반복,일일,가이드 미션으로 구성되며 테이블에서 토탈데이터를 가져오고 유저의 정보를 세이브,로드 하는 방식입니다.
- 유저의 특정 기록을 이벤트로 기록하여 담아두고(PlayingRecord.cs) 미션데이터와 연동하여 관리됩니다.

<details>
<summary>
    `미션관리 내용 보기`
</summary>
<div markdown="1">

- PlayingRecord.cs

```code
 public class PlayingRecord
    {
        public long MONSTER_KILL     { get; set; }
      ...
        public long GetMissionValue(MissionType _MissionType)
        {
           var t = this.GetType();
            var field = t.GetProperty(_MissionType.ToString());
            if (null == field) return -1;

            object o = field.GetValue(this);
            if (null == o) return -1;

            return (long)o;
        }
        public long SetMissionValue(MissionType _MissionType, int _IncValue)
        {
              var t = this.GetType();
            var field = t.GetProperty(_MissionType.ToString());
            if (null == field) return -1;

            object o = field.GetValue(this);
            if (null == o) return -1;

            long curval = (long)o;
            curval = _IncValue;

            field.SetValue(this, _IncValue);

            return curval;
        }

      ...
    }
```

- Data_Mission.cs

```code
...
public class Data_Mission
{
    ...
         public void IncMissionValue(MissionType _type, int value)
        {
            _playingRecord.IncMissionValue(_type, value);
            missionUpdater.missiontype = _type;
            if (CurrentGuideMission.baseInfo.m_type==_type)
            {
                CurrentGuideMission.curCount += value;
            }
            DailyMission _dmission = dailyMission.Find(o => o.baseInfo.m_type == _type);
            if(_dmission != null)
                _dmission.curCount += value;
            RepeatMission _rmission = repeatMissions.Find(o => o.baseInfo.m_type == _type);
            if (_rmission != null)
                _rmission.curCount += value;

           ...
        }
        public void SetMissionValue(MissionType _type, int value,bool sendmsg)
        {
            _playingRecord.SetMissionValue(_type, value);
            missionUpdater.missiontype = _type;
            if (CurrentGuideMission.baseInfo.m_type == _type)
            {
                CurrentGuideMission.curCount = value;
            }
            DailyMission _dmission = dailyMission.Find(o => o.baseInfo.m_type == _type);
            if (_dmission != null)
                _dmission.curCount = value;
            RepeatMission _rmission = repeatMissions.Find(o => o.baseInfo.m_type == _type);
            if (_rmission != null)
                _rmission.curCount = value;

        }
    ...
}
```

</div>
</details>

## 튜토리얼시스템

## 인벤토리관리
