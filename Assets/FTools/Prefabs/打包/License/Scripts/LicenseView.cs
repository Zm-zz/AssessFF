using System;
using UnityEngine;
using UnityEngine.UI;

namespace WeFrame
{
	[Serializable]
	public class LicenseCheckClientData : BaseClientData
	{
		public string version = "0.0";

		public string seriesNo = "ABCDEFGHIJKL00000001";

		public string macAddr = "ABCDEFGHIJKL";

		public string softwareType = "000000";

		public string softwareVersion = "01";

		public string name = "Name";

		public string email = "Email@qq.com";
	}

	[Serializable]
	public class LicenseCheckServerData : BaseServerData
	{
		public string code = "0000";

		public string msg = "SUCCESS";

		public LicenseCheckTimeData data = new LicenseCheckTimeData();
	}

	[Serializable]
	public class LicenseCheckTimeData
	{
		public int registerTime = 0;

		public int expiredTime = 0;

		public int now = 0;
	}

	[Serializable]
	public class LicenseCheckSettingData
	{
		public string mLastUseTime = DateTime.MinValue.ToString();

		public void SetLastUseTime(DateTime time)
		{
			DateTime lastTime = DateTime.Parse(mLastUseTime);

			if (lastTime < time)
			{
				lastTime = time;
				mLastUseTime = lastTime.ToString();
			}
		}

		public DateTime GetLastUseTime()
		{
			DateTime lastTime = DateTime.Parse(mLastUseTime);

			return lastTime;
		}
	}

	public class LicenseViewData
	{
		protected static string mLicenseIndex = "ABCDEFGHIJKL00000001";

		protected static string mPhysicalAddress = "ABCDEFGHIJKL";

		protected static string mSoftwareIndex = "020111";      // wxc：[需要修改]软件唯一ID

		protected static string mSoftwareVersion = "01";

		protected static DateTime mSoftwareTime = new DateTime(2020, 01, 01);

		public static string LicenseIndex
		{
			get { mLicenseIndex = PhysicsAddress + SoftwareIndex + SoftwareVerison; return mLicenseIndex; }
		}

		public static string PhysicsAddress
		{
			get { mPhysicalAddress = CComputer.GetPhysicalAddress(); return mPhysicalAddress; }
		}

		public static string SoftwareIndex
		{
			get { return mSoftwareIndex; }
		}

		public static string SoftwareVerison
		{
			get { return mSoftwareVersion; }
		}

		public static DateTime SoftwareTime
		{
			get { return mSoftwareTime; }
		}
	}

	public class LicenseView : UnityObject
	{
		protected LicenseCheckClientData mCheckClientData = new LicenseCheckClientData();

		protected LicenseCheckServerData mCheckServerData = new LicenseCheckServerData();

		protected LicenseCheckSettingData mLicenseSettingData = new LicenseCheckSettingData();

		protected bool mIsCheckSuccess = false;

		[SerializeField] Text codeText, errorText;
		[SerializeField] GameObject spacePanel, checkPanel;

        protected override void BeforeInit()
        {
            base.BeforeInit();

			LicenseCheckSettingData settingData = RegeditManager.Self.GetObject<LicenseCheckSettingData>("LicenseCheckSettingData");
			mLicenseSettingData = settingData == null ? mLicenseSettingData : settingData;

			ClientManager.Self.OpenConnect();

			mCheckClientData.seriesNo = LicenseViewData.LicenseIndex;
			mCheckClientData.macAddr = LicenseViewData.PhysicsAddress;
			mCheckClientData.softwareType = LicenseViewData.SoftwareIndex;
			mCheckClientData.softwareVersion = LicenseViewData.SoftwareVerison;
		}

        protected override void Init()
        {
            base.Init();

			EventManager.Self.LoginEvent(NetworkEventType.License_Check, OnLicenseViewCheckRespond);

			SetCodeData(LicenseViewData.LicenseIndex);

			CLog.InputOperate("输出许可证");
			CLog.InputTarget("物理地址", LicenseViewData.PhysicsAddress);
			CLog.InputTarget("软件索引", LicenseViewData.SoftwareIndex);
			CLog.InputTarget("软件版本", LicenseViewData.SoftwareVerison);
			CLog.Output();
		}
		

		protected override  void AfterInit()
		{
			base.Init();

            if (ClientManager.Self.IsConnect)
            {
                SetCheckBtnClick();
            }
            else
            {
				SetLoginBtnClick();
			}
		}

		protected override void Release()
		{
			base.Release();

			EventManager.Self.LogoutEvent(NetworkEventType.License_Check);

			mLicenseSettingData.SetLastUseTime(DateTime.Now);
            RegeditManager.Self.SetObject("LicenseCheckSettingData", mLicenseSettingData);
            ClientManager.Self.CloseConnect();
		}

		public void OnCodeBtnClick()
		{
			GUIUtility.systemCopyBuffer = mCheckClientData.seriesNo;
		}

		public void OnCheckBtnClick()
		{
			SetCheckBtnClick();
		}

		public void SetCheckBtnClick()
		{
			if (LicenseViewData.SoftwareIndex == "000000")
			{
				SetErrorData("软件索引认证失败");

				CLog.InputOperate("认证许可证");
				CLog.InputWrong("软件索引认证失败");
				CLog.Output();
			}
			else
			{
				ClientManager.Self.SendMessage(NetworkEventType.License_Check, JsonUtility.ToJson(mCheckClientData));
			}
		}
		
		public void OnLicenseViewCheckRespond(BaseEventData data)
		{
			NetworkEventData netData = data as NetworkEventData;
			
			if (netData != null)
			{
				mCheckServerData = JsonUtility.FromJson<LicenseCheckServerData>(netData.JsonData);

				if (mCheckServerData != null)
				{
					if (mCheckServerData.msg == "SUCCESS")
					{
						RegeditManager.Self.SetObject("LicenseCheckServerData", mCheckServerData);

						mLicenseSettingData.SetLastUseTime(CComputer.GetUnixTime(mCheckServerData.data.now));

						SetLoginBtnClick();
					}
				}
			}
		}

		public void SetLoginBtnClick()
		{
			LicenseCheckServerData checkServerData = RegeditManager.Self.GetObject<LicenseCheckServerData>("LicenseCheckServerData");

			if (checkServerData != null)
			{
				DateTime registerTime = CComputer.GetUnixTime(checkServerData.data.registerTime);
				DateTime expiredTime = CComputer.GetUnixTime(checkServerData.data.expiredTime);
				DateTime nowTime = CComputer.GetUnixTime(checkServerData.data.now);
				DateTime lastTime = mLicenseSettingData.GetLastUseTime();
				DateTime localTime = DateTime.Now;

				CLog.InputOperate("认证许可证");
				CLog.InputTarget("注册时间", registerTime.ToString());
				CLog.InputTarget("过期时间", expiredTime.ToString());
				CLog.InputTarget("认证时间", nowTime.ToString());
				CLog.InputTarget("本地时间", localTime.ToString());
				CLog.InputTarget("使用时间", lastTime.ToString());

				if (nowTime < expiredTime)
				{
					if (localTime < lastTime.AddMinutes(-1))
					{
						mIsCheckSuccess = false;

						SetErrorData("本地时间认证失败");

						CLog.InputWrong("本地时间认证失败");
					}
					else
					{
						if (localTime < expiredTime)
						{
							mIsCheckSuccess = true;

							SetErrorData("");

							CLog.InputRight("许可证认证成功");
						}
						else
						{
							mIsCheckSuccess = false;

							SetErrorData("过期时间认证失败");

							CLog.InputWrong("过期时间认证失败");
						}		
					}
				}
				else
				{					
					mIsCheckSuccess = false;

					SetErrorData("过期时间认证失败");

					CLog.InputWrong("过期时间认证失败");
				}
			}
			else
			{
				mIsCheckSuccess = false;

				SetErrorData("许可证认证失败");

				CLog.InputWrong("许可证认证失败");
			}

			CLog.Output();

			if (mIsCheckSuccess)
			{
				gameObject.SetActive(false);
			}
			else
			{
				spacePanel.SetActive(true);
				checkPanel.SetActive(true);
			}

			Debug.Log($"mIsCheckSuccess: {mIsCheckSuccess}");
		}

		public void OnExitBtnClick()
		{
			Application.Quit();
		}

		void SetCodeData(string code)
        {
			codeText.text = code;
        }

		void SetErrorData(string error)
		{
			errorText.text = error;
		}
	}

}