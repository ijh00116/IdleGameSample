using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;

namespace BlackTree
{
    public class GooglePlayGameService : ICredentials
    {
		static GooglePlayGameService()
		{

		}

		public void Login(System.Action<bool> onLogin)
		{
			if (authenticated == true)
			{
				onLogin(true);
				return;
			}
#if UNITY_EDITOR
			Debug.Log("Cannot use GPGS in Editor");
			onLogin(false);
#else
#endif
	
			Social.localUser.Authenticate( (isSuccess) => 
			{
				if ( isSuccess == true )
				{
					Debug.Log("LOGIN SUCCESS!! GPGS.. " + isSuccess);
					onLogin(isSuccess);
				}
				else
                {
					Debug.Log("LOGIN FAIL!! GPGS.. " + isSuccess);
					onLogin(isSuccess);
				}
			} );
		}

		public CDType type
		{
			get
			{
				return CDType.GooglePlay;
			}
		}

		private bool authenticated
		{
			get
			{
				return (Social.localUser != null)
					&& (Social.localUser.authenticated == true);
			}
		}

		public string serialized
		{
			get
			{
				if (authenticated == true)
				{
					return "temp";
				}
				else
				{
					return string.Empty;
				}
			}
		}

		public void Logout()
		{
			if (authenticated == false)
			{
				return;
			}
		}
	}

}
