using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackTree
{
    public class Authorization
    {
        public enum Status
        {
            NotAuthorized,
            InProgress,
            Done,
        }

        private static readonly ICredentials notAuthorized = new None();
        private ICredentials _active = notAuthorized;
        private ICredentials _inProgress = null;

        public delegate void Validator(ICredentials Current, System.Action<bool> onComplete);

        public ILoginData current
        {
            get
            {
                return _active;
            }
        }

        public Status status
        {
            get
            {
                if (_inProgress != null)
                {
                    return Status.InProgress;
                }

                if (current == notAuthorized)
                {
                    return Status.NotAuthorized;
                }
                else
                {
                    return Status.Done;
                }
            }
        }

        public static void Init()
        {
            // 페이스북제거
            // Facebook.Init();
        }

        public static IEnumerator TryAutoLogin()
        {
            var authorization = new Authorization();
            return new WaitUntil(() => (authorization.status != Status.InProgress));
        }
        private Authorization()
        {
            var saved = Data_local.Instance.LoadLogin();
            TryLogin(saved, null);
        }

        public IEnumerator Login(CDType type, Validator handler)
        {
            var data = new Data_login()
            {
                type = type,
            };

           

            TryLogin(data, handler);
            return new WaitUntil(() => (status != Status.InProgress));
        }

        public void Logout()
        {
            MakeActive(notAuthorized);
        }

        //로컬 저장정보에서 로그인 계정정보 확인 후 로그인 계정정보 있으면   OnLoginComplete(isSuccess); 호출 됨
        void TryLogin(Data_login saved, Validator validator)
        {
            _inProgress = GenerateCredentials(saved);

            System.Action<bool> onLogin = delegate (bool isSuccess)
            {
                if (validator == null)
                {
                    OnLoginComplete(isSuccess);
                    return;
                }

                if (!isSuccess)
                {
                    OnLoginComplete(false);
                    return;
                }

                // MakeActive(_inProgress);
                validator(_inProgress, OnLoginComplete);
            };

            _inProgress.Login(onLogin);
        }

        ICredentials GenerateCredentials(Data_login saved)
        {
            switch (saved.type)
            {
                case CDType.Guest:
                    return new Guest(saved.loginId);
                //case CDType.Facebook:
                //    return new Test("Facebook", saved.type);

                //하단의 설정은 로그인 뒤끝과 연동작업시 다시(7/30일이나 31일)                   
                //#else
                //#if UNITY_ANDROID
                case CDType.GooglePlay:
                   return new GooglePlayGameService();
                //#elif UNITY_IOS
                //case CDType.IOS_GameCenter:
                //    return new AppleGameCenter();
                //#endif
                //case CDType.Facebook:
                //    return new Facebook();
                //default:
                //    return notAuthorized;
                default:
                    return notAuthorized;
            }
        }

        void OnLoginComplete(bool isSuccess)
        {
            if(isSuccess)
            {
                switch (_inProgress.type)
                {
                    case CDType.Guest:
                        break;
                    case CDType.IOS_GameCenter:
                        break;
                    case CDType.GooglePlay:
                        break;
                    default:
                        break;
                }
                
            }
            else
            {
                BackendLoginComplete(isSuccess);
            }
        }

        void BackendLoginComplete(bool isSuccess)
        {
            if (isSuccess)
            {
                MakeActive(_inProgress);
            }
            else
            {
                _inProgress.Logout();
            }
            

                _inProgress = null;
        }

        public void MakeActive(ICredentials newCredentials)
        {
            _active.Logout();
            _active = newCredentials;
            Data_local.Instance.SaveLogin(_active);
        }
    }

}
