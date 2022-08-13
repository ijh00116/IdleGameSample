using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackTree
{
    public class MailBoxInterface : MonoBehaviour
    {
        public MailBoxUISlot slotPrefab;
        public Transform parent;

        float CurrentTime;

        List<MailBoxUISlot> slotList = new List<MailBoxUISlot>();

        public void Init()
        {
            //BackEnd.BackendReturnObject _bro = BackEnd.Backend.Social.Post.GetPostListV2();

            //if (_bro.IsSuccess())
            //{
            //    LitJson.JsonData json = BackEnd.Backend.Social.Post.GetPostListV2().GetReturnValuetoJSON()["fromAdmin"];
            //    int k = json.Count;
            //    for (int i = 0; i < json.Count; i++)
            //    {
            //        LitJson.JsonData _json = json[i];
            //        if (_json.ContainsKey("inDate"))
            //        {
            //            string postIndate = _json["inDate"]["S"].ToString();
            //            string title = _json["title"]["S"].ToString();
            //            int amount = int.Parse(_json["itemCount"]["N"].ToString());

            //            var obj = Instantiate(slotPrefab);
            //            obj.transform.SetParent(parent, false);
            //            obj.Init(title, amount, postIndate, RecieveMailData);
            //            slotList.Add(obj);

            //            Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.MailBox, true));
            //        }
            //    }
            //}
            //if (slotList.Count > 0)
            //    Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.MailBox, true));
        }

        public void PopupMailWindow()
        {

            //if (_bro.IsSuccess())
            //{
            //    LitJson.JsonData json = BackEnd.Backend.Social.Post.GetPostListV2().GetReturnValuetoJSON()["fromAdmin"];
            //    int k = json.Count;
            //    for (int i = 0; i < json.Count; i++)
            //    {
            //        LitJson.JsonData _json = json[i];
            //        if (_json.ContainsKey("inDate"))
            //        {
            //            string postIndate = _json["inDate"]["S"].ToString();
            //            string title = _json["title"]["S"].ToString();
            //            int amount = int.Parse(_json["itemCount"]["N"].ToString());

            //            var obj = Instantiate(slotPrefab);
            //            obj.transform.SetParent(parent, false);
            //            obj.Init(title, amount, postIndate, RecieveMailData);
            //            slotList.Add(obj);

            //            Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.MailBox, true));
            //        }
            //    }
            //}

        }

        void RecieveMailData(string indate)
        {
            //리시브 낼 테스트
            
//            BackEnd.BackendReturnObject _bro= BackEnd.Backend.Social.Post.ReceiveAdminPostItemV2(indate);
//            if(_bro.IsSuccess())
//            {
//#if UNITY_EDITOR
//                Debug.LogError("수령 완료");
//#endif
//                bool ismailExist = false;
//                for (int i=0; i< slotList.Count; i++)
//                {
//                    if(slotList[i].gameObject.activeInHierarchy)
//                    {
//                        ismailExist = true;
//                        break;
//                    }
//                }
//                Message.Send<UI.Event.SideBtnNewIconActivate>(new UI.Event.SideBtnNewIconActivate(UI.SideButtonType.MailBox, false));
//            }
        }
    }

}
