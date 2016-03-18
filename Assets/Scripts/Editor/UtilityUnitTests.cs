using System;
using PubNubMessaging.Core;
using NUnit.Framework;

namespace PubNubMessaging.Tests
{
    [TestFixture]
    public class UtilityUnitTests
    {
        #if DEBUG  
        #if(UNITY_IOS)
        [Test]
        public void TestCheckTimeoutValue(){
            int v = Utility.CheckTimeoutValue(20);
            Assert.True (v.Equals (20));
        }

        [Test]
        public void TestCheckTimeoutValueGreaterThan60(){
            int v = Utility.CheckTimeoutValue(60);
            Assert.True (v.Equals (59));
        }
        #endif

        [Test]
        [ExpectedException (typeof(ArgumentException))]
        public void TestCheckChannel ()
        {
            Utility.CheckChannel ("");
            Assert.True (true);
        }

        [Test]
        [ExpectedException (typeof(ArgumentException))]
        public void TestCheckMessage ()
        {
            Utility.CheckMessage (null);
            Assert.True (true);
        }

        [Test]
        [ExpectedException (typeof(MissingMemberException))]
        public void TestCheckPublishKey ()
        {
            Utility.CheckPublishKey (null);
            Assert.True (true);
        }

        [Test]
        [ExpectedException (typeof(ArgumentException))]
        public void TestCheckCallbackError ()
        {
            Utility.CheckCallback<PubnubClientError> (null, CallbackType.Error);
            Assert.True (true);
        }

        [Test]
        [ExpectedException (typeof(ArgumentException))]
        public void TestCheckCallbackUser ()
        {
            Utility.CheckCallback<string> (null, CallbackType.User);
            Assert.True (true);
        }

        [Test]
        public void TestCheckJSONPluggableLibrary ()
        {
            try{
                Utility.CheckJSONPluggableLibrary ();
                Assert.True (true);
            }catch{
                Assert.True (false);
            }
        }

        [Test]
        [ExpectedException (typeof(ArgumentException))]
        public void TestCheckUserState ()
        {
            Utility.CheckUserState (null);
            Assert.True (true);
        }

        [Test]
        [ExpectedException (typeof(MissingMemberException))]
        public void TestCheckSecretKey ()
        {
            Utility.CheckSecretKey (null);
            Assert.True (true);
        }

        [Test]
        public void TestGenerateGuid ()
        {
            Assert.IsTrue(Utility.GenerateGuid ().ToString() != "");
        }

        [Test]
        public void CheckRequestTimeoutMessageInError ()
        {
            CustomEventArgs<string> cea = new CustomEventArgs<string> ();
            cea.CurrRequestType = CurrentRequestType.Subscribe;
            cea.IsError = true;
            cea.IsTimeout = false;
            cea.Message = "The request timed out.";
            cea.PubnubRequestState = null;
            Assert.IsTrue(Utility.CheckRequestTimeoutMessageInError<string> (cea));
        }

        [Test]
        public void TestIsPresenceChannelTrue ()
        {

            Assert.IsTrue(Utility.IsPresenceChannel ("my_channel-pnpres"));
        }

        [Test]
        public void TestIsPresenceChannelFalse ()
        {

            Assert.IsFalse(Utility.IsPresenceChannel ("my_channel"));
        }

        [Test]
        public void TestIsUnsafeWithComma ()
        {
            RunUnsafeTests (false);
        }

        [Test]
        public void TestIsUnsafe ()
        {
            RunUnsafeTests (true);
        }

        void RunUnsafeTests(bool ignoreComma)
        {
            char[] ch = {',', ' ','~','`','!','@','#','$','%','^','&','*','(',')','+','=','[',']','\\','{','}','|',';','\'',':','\"','/','<','>','?'};

            bool bPass = true;
            char currentChar = ' ';
            foreach (char c in ch) {
                currentChar = c;
                if (ignoreComma && c.Equals (',')) {
                    continue;
                }
                if (!Utility.IsUnsafe (c, ignoreComma)) {
                    bPass = false;
                    break;
                }
            }
            if (bPass) {
                Assert.True(bPass);
            } else {
                Assert.Fail(string.Format("failed for {0}", currentChar));
            }
        }

        [Test]
        public void TestEncodeUricomponent ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.Subscribe, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentDetailedHistoryIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.DetailedHistory, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentDetailedHistoryIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.DetailedHistory, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushGetIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushGet, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushGetIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushGet, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushRemoveIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushRemove, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushRemoveIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushRemove, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushRegisterIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushRegister, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushRegisterIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushRegister, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushUnregisterIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushUnregister, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPushUnregisterIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PushUnregister, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentHereNowIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";

            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.HereNow, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentHereNowIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.HereNow, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentLeaveIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.Leave, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentLeaveIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.Leave, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPresenceHeartbeatIgnorePercentFalse ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%252F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PresenceHeartbeat, true, false);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestEncodeUricomponentPresenceHeartbeatIgnorePercentTrue ()
        {
            //test unsafe surrogate and normal test
            string expected = "Text%20with%20\ud83d\ude1c%20emoji%20\ud83c\udf89.%20testencode%20%7E%60%21%40%23%24%25%5E%26%2A%28%29%2B%3D%5B%5D%5C%7B%7D%7C%3B%27%3A%22%2F%3C%3E%3F";
            string received = Utility.EncodeUricomponent("Text with 😜 emoji 🎉. testencode ~`!@#$%^&*()+=[]\\{}|;':\"/<>?", ResponseType.PresenceHeartbeat, true, true);
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestMd5 ()
        {
            //test unsafe surrogate and normal test
            string expected = "83a644046796c6a0d76bc161f73b75b4";
            string received = Utility.Md5("test md5");
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TestTranslateDateTimeToSeconds ()
        {
            //test unsafe surrogate and normal test
            long expected = 1449792000;
            long received = Utility.TranslateDateTimeToSeconds(DateTime.Parse("11 Dec 2015"));
            UnityEngine.Debug.Log (received);
            Assert.IsTrue(expected.Equals(received));
        }

        [Test]
        public void TranslateDateTimeToUnixTime ()
        {
            UnityEngine.Debug.Log ("Running TranslateDateTimeToUnixTime()");
            //Test for 26th June 2012 GMT
            DateTime dt = new DateTime (2012, 6, 26, 0, 0, 0, DateTimeKind.Utc);
            long nanoSecondTime = Pubnub.TranslateDateTimeToPubnubUnixNanoSeconds (dt);
            Assert.True ((13406688000000000).Equals (nanoSecondTime));
        }

        [Test]
        public void TranslateUnixTimeToDateTime ()
        {
            UnityEngine.Debug.Log ("Running TranslateUnixTimeToDateTime()");
            //Test for 26th June 2012 GMT
            DateTime expectedDate = new DateTime (2012, 6, 26, 0, 0, 0, DateTimeKind.Utc);
            DateTime actualDate = Pubnub.TranslatePubnubUnixNanoSecondsToDateTime (13406688000000000);
            Assert.True (expectedDate.ToString ().Equals (actualDate.ToString ()));
        }
        #endif
    }
}
