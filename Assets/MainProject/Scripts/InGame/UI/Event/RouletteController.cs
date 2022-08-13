using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public enum EventCurrencyType
    {
        REWARD_BOX_C,
        REWARD_BOX_B,
        REWARD_BOX_A,
        MagicStone,
        EnchantStone,
        Essence,
        Gem_500,
        Gem_1000,

        End
    }
    public class RouletteController : MonoBehaviour
    {
        [HideInInspector]public bool isStarted;
        
        [Header("룰렛조각프리팹,상속받을 부모오브젝트")]
        public GameObject WheelParent;
        public GameObject piecePrefab;          // One piece for fortune wheel.
        [Header("팝업창")]
        public Button PopupOffButton;
        public Transform popupPanel;
        public Image rewardImagePopup;
        public Text rewardTextPopup;
        public Text PopupTimeText;
        float CurrentTimeInPopupPanel;
        [Header("룰렛상단 이미지")]
        public Image rewardImageHeader;
        public Text rewardTextHeader;
        [Header("룰렛쿠폰 갯수")]
        public Text RouletteCouponAmount;         // Pop-up text with cost or rewarded coins amount


        [Header("BottomUI")]
        [SerializeField] Button AutoBtn;
        [SerializeField] Image AutoOnDarkImage;
        [SerializeField] Image AutoOffDarkImage;
        [SerializeField] Button SpeedBtn;
        [SerializeField] Image SpeedOnDarkImage;
        [SerializeField] Image SpeedOffDarkImage;
        [SerializeField] float Speedvalue=3.0f;
        [Header("BottomUI시작,info")]
        [SerializeField] Button Set_1Times;
        [SerializeField] GameObject set_1Checked;
        [SerializeField] Button Set_10Times;
        [SerializeField] GameObject set_10Checked;
        bool Is1TimesForStart;
        [SerializeField] Button StartButton;
        [SerializeField] Text StartButtonText;
        [SerializeField] Button InfoBtn;
    

        public bool useCustomBackgrounds;       //Set true if you want to use package's custom design or set false if you want to use your color selections at Inspector from PiecesOfWheel array.
        public Sprite[] CustomBackgrounds;

        public ParticleSystem confetiEffect;

        //데이터
        bool IsAutomode;
        bool IsSpeedmode;

        private float[] sectorsAngles;

        private float startAngle = 0;
        private float finalAngle;
        private float currentLerpRotationTime;

        private int rewardIndex;
        private int randomChange;               //To use different Easing functions and create diversity for rotations.
        public List<RouletteTabledata> PiecesOfWheel=new List<RouletteTabledata>();
        [HideInInspector]public List<RoulettePiece> RoulettePieceList = new List<RoulettePiece>();

        //확률

        public void Awake()
        {
            WheelParent.transform.eulerAngles = new Vector3(0, 0, startAngle);
            Is1TimesForStart = true;
            Set_1Times.onClick.AddListener(() => { CheckTimes(true); });
            Set_10Times.onClick.AddListener(()=>{ CheckTimes(false); });
            set_1Checked.gameObject.SetActive(true);
            set_10Checked.gameObject.SetActive(false);
            StartButton.onClick.AddListener(TurnWheel);

            PopupOffButton.onClick.AddListener(ClaimReward);

            IsAutomode = false;
            IsSpeedmode = false;
            AutoOnDarkImage.gameObject.SetActive(true);
            AutoOffDarkImage.gameObject.SetActive(false);
            PopupTimeText.gameObject.SetActive(false);

            SpeedOnDarkImage.gameObject.SetActive(true);
            SpeedOffDarkImage.gameObject.SetActive(false);

            AutoBtn.onClick.AddListener(PushAutoBtn);
            SpeedBtn.onClick.AddListener(PushSpeedModeBtn);

            CreateWheel();

            RouletteCouponAmount.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.RouletteCoupon).value.ToDisplay();

            Message.AddListener<UI.Event.CurrencyChange>(CurrencyChangeUpdate);
           

            CheckTimes(true);
        }
        public void CreateWheel()
        {
            float startingAngle = 0;

            for (int i = 0; i < InGameDataTableManager.RouletteEventTableList.roulette.Count; i++)
            {
                PiecesOfWheel.Add(InGameDataTableManager.RouletteEventTableList.roulette[i]);
            }
            for (int i = 0; i < PiecesOfWheel.Count; i++)
            {
                GameObject pieceObj = Instantiate(piecePrefab, Vector3.zero, new Quaternion(0, 0, 0, 0), WheelParent.transform);

                pieceObj.transform.name = "Piece " + (i + 1);
                pieceObj.transform.localPosition = new Vector3(0, 0, 0);
                pieceObj.transform.Rotate(0, 0, Mathf.Abs(startingAngle), 0);
                RoulettePiece piece = pieceObj.transform.GetComponent<RoulettePiece>();
                piece.SetValues(this,i);
                RoulettePieceList.Add(piece);
                startingAngle += 45;
                
            }
        }

        void CheckTimes(bool times_1)
        {
            Is1TimesForStart = times_1;
            if(Is1TimesForStart)
                StartButtonText.text = string.Format("START\nX1");
            else
                StartButtonText.text = string.Format("START\nX10");

            set_1Checked.gameObject.SetActive(Is1TimesForStart);
            set_10Checked.gameObject.SetActive(!Is1TimesForStart);
        }

        public void TurnWheel()
        {
            if (isStarted == true)
                return;
            WheelParent.transform.eulerAngles = new Vector3(0, 0, 0);
            if (Is1TimesForStart==false)
            {
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.RouletteCoupon).value < 10)
                    return;
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.RouletteCoupon, -10);
            }
            else
            {
                if (Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.RouletteCoupon).value < 1)
                    return;
                Common.InGameManager.Instance.GetPlayerData.AddCurrency(CurrencyType.RouletteCoupon, -1);
            }

            RouletteCouponAmount.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.RouletteCoupon).value.ToDisplay();
        

            randomChange = Random.Range(0, 3);
            currentLerpRotationTime = 0f;

            // Fill the necessary angles (for example if you want to have 12 sectors you need to fill the angles with 30 degrees step)
            sectorsAngles = new float[] { -90, -135, -180, -225, -270, -315,-360,-45 };
            //{ 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };

            int fullCircles = Random.Range(5, 8);
         
            //확률 계산
            int FullCount = 0;
            for(int i=0; i< PiecesOfWheel.Count; i++)
            {
                FullCount+= PiecesOfWheel[i].rarity;
            }
            int _random = Random.Range(0, FullCount);
            int Fullcount_2=0;
            int findIndex=0;
            for (int i = 0; i < PiecesOfWheel.Count; i++)
            {
                Fullcount_2 += PiecesOfWheel[i].rarity;
                if(_random<=Fullcount_2)
                {
                    findIndex = i;
                    break;
                }
            }

            float randomFinalAngle = sectorsAngles[findIndex];
            //확률 계산
            // Here we set up how many circles our wheel should rotate before stop
            finalAngle = -(fullCircles * 360 - randomFinalAngle);
            isStarted = true;
            Set_1Times.interactable = !isStarted;
            Set_10Times.interactable = !isStarted;
        }

        public void Release()
        {
            Message.RemoveListener<UI.Event.CurrencyChange>(CurrencyChangeUpdate);
        }

        // Update is called once per frame
        void Update()
        {
            if(popupPanel.gameObject.activeInHierarchy &&IsAutomode)
            {
                CurrentTimeInPopupPanel += Time.deltaTime;
                if (CurrentTimeInPopupPanel>=5)
                {
                    ClaimReward();
                    TurnWheel();
                }
                PopupTimeText.text = string.Format("<color=red>{0}</color>초 후 자동시작합니다.", 5 - (int)CurrentTimeInPopupPanel);
            }
            if (!isStarted)
                return;

            float maxLerpRotationTime = 5f;

            // Increment timer once per frame
            currentLerpRotationTime += Time.deltaTime*((IsSpeedmode)? Speedvalue:1);

            float t = currentLerpRotationTime / maxLerpRotationTime;
            if (randomChange == 0)
                t = 1f - (1f - t) * (1f - t);
            else
                t = t * t * t * (t * (6f * t - 15f) + 10f);

            float angle = Mathf.Lerp(startAngle, finalAngle, t); //Linear Interpolation
            WheelParent.transform.eulerAngles = new Vector3(0, 0, angle);

            if (currentLerpRotationTime>=maxLerpRotationTime|| angle == finalAngle)
            {
                currentLerpRotationTime = maxLerpRotationTime;
                isStarted = false;
                Set_1Times.interactable = !isStarted;
                Set_10Times.interactable = !isStarted;
                startAngle = finalAngle % 360;
                GiveAwardByAngle();
             
            }
          

           
        }

        private void GiveAwardByAngle()
        {
            // Here you can set up rewards for every sector of wheel
            switch ((int)startAngle)
            {
                case 0:
                    rewardIndex = 6;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -45:
                    rewardIndex = 7;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -90:
                    rewardIndex = 0;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -135:
                    rewardIndex = 1;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -180:
                    rewardIndex = 2;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -225:
                    rewardIndex = 3;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -270:
                    rewardIndex = 4;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -315:
                    rewardIndex = 5;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                case -360:
                    rewardIndex = 6;
                    StartCoroutine(RewardPopup(rewardIndex));
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Log("There is no reward for this angle, please check angles");
#endif
                    break;
            }
        }

        WaitForSeconds waitformSec = new WaitForSeconds(0.1f);
        IEnumerator RewardPopup(int rewardIndex)
        {
            yield return waitformSec;
            rewardImagePopup.sprite = RoulettePieceList[rewardIndex].mySprite;
            int rewardcount = 0;
            if (Is1TimesForStart==false)
            {
                if(RoulettePieceList[rewardIndex].myData.reward_type==RewardType.REWARD_BOX)
                {
                    rewardcount = 11;
                }
                else
                {
                    rewardcount = (RoulettePieceList[rewardIndex].myData.reward_count * 11);
                }
                rewardTextPopup.text = rewardcount.ToString();
            }
            else
            {
                if (RoulettePieceList[rewardIndex].myData.reward_type == RewardType.REWARD_BOX)
                {
                    rewardcount = 1;
                }
                else
                {
                    rewardcount = RoulettePieceList[rewardIndex].myData.reward_count;
                }
                rewardTextPopup.text = rewardcount.ToString();
            }
            popupPanel.gameObject.SetActive(true);
            CurrentTimeInPopupPanel = 0;

            CurrencyType currency = CurrencyType.Gem;
            if(RoulettePieceList[rewardIndex].myData.reward_type==RewardType.REWARD_BOX)
            {
                currency= Common.InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(RoulettePieceList[rewardIndex].myData.reward_type,
                    RoulettePieceList[rewardIndex].myData.reward_count);
            }
            else
            {
                currency = Common.InGameManager.Instance.GetPlayerData.rewardinfo.RewardtypeToCurrencyType(RoulettePieceList[rewardIndex].myData.reward_type);
            }
            Common.InGameManager.Instance.GetPlayerData.AddCurrency(currency, rewardcount);

            yield break;
        }
        public void ClaimReward()
        {
            popupPanel.gameObject.SetActive(false);
        }

        void PushAutoBtn()
        {
            IsAutomode = !IsAutomode;
            AutoOnDarkImage.gameObject.SetActive(!IsAutomode);
            AutoOffDarkImage.gameObject.SetActive(IsAutomode);
            PopupTimeText.gameObject.SetActive(IsAutomode);
        }

        void PushSpeedModeBtn()
        {
            IsSpeedmode = !IsSpeedmode;
            SpeedOnDarkImage.gameObject.SetActive(!IsSpeedmode);
            SpeedOffDarkImage.gameObject.SetActive(IsSpeedmode);
        }

        void CurrencyChangeUpdate(UI.Event.CurrencyChange msg)
        {
            if (msg.CurrencyTypeSummarize)
            {
                if (msg.Type != CurrencyType.RouletteCoupon)
                    return;
            }

            if (msg.Type==CurrencyType.RouletteCoupon)
            {
                RouletteCouponAmount.text = Common.InGameManager.Instance.GetPlayerData.GetCurrency(CurrencyType.RouletteCoupon).value.ToDisplay();
            }
        }

    }

}
