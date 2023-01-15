using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGIOHandler : MonoBehaviour
{
    public string AppID;
    public string AesKey;
    // Start is called before the first frame update
    void Start()
    {
        // Set up the options for NGIO.
        var options = new Dictionary<string,object>() 
        {
            // This should match the version number in your Newgrounds App Settings page
            { "version",            "1.0.0" },

            // If you aren't using any of these features, set them to false, or delete the line
            { "checkHostLicense",   true },
            { "autoLogNewView",     true },
            { "preloadMedals",      true },
            { "preloadScoreBoards", true },
            { "preloadSaveSlots",   true }
        };

        // initialize the API, using the App ID and AES key from your Newgrounds project
        NGIO.Init(AppID, AesKey, options);
    }

    void Update()
    {
        StartCoroutine(NGIO.GetConnectionStatus(OnConnectionStatusChanged));
    }

    public void OnConnectionStatusChanged(string status)
    {
        // You blocked the website hosting this game!
        if (!NGIO.legalHost) {

            /**
            * Do something here to inform the player where they can play the game legally.
            * You can have a button that calls NGIO.LoadOfficialUrl(); when clicked.
            */
            return;
        }

        // This copy of the game is out of date
        if (NGIO.isDeprecated) {

            /**
            * This is a good place to show a 'New version available' message.
            * Throw in a button that calls NGIO.LoadOfficialUrl(); when clicked.
            */
        }

        // If the user is currently logging in, this will be true.
        if (NGIO.loginPageOpen) {

            /**
            * Here you should present the user with a 'please wait' message.
            * You should also show a 'Cancel Login' button, so they aren't
            * stuck on this message if they close their login browser 
            * without actually logging in.
            *
            * Your cancel button should call NGIO.CancelLogin();
            */

        // Here is where we check the actual status of the session.
        } else {
        
            switch(status) {

                case NGIO.STATUS_CHECKING_LOCAL_VERSION:
                    /**
                    * We're loading the host license and latest version info.
                    * Show a 'please wait' message.
                    */
                    break;

                case NGIO.STATUS_PRELOADING_ITEMS:
                    /**
                    * We're preloading medals, scoreboards, save slots, etc...
                    * Show a 'please wait' message.
                    */
                    break;

                case NGIO.STATUS_LOGIN_REQUIRED:
                    /**
                    * We have a valid session ID, but the player isn't logged in.
                    * Show a 'Log In' button, and a message about how the player
                    * needs to sign in to use certain features.
                    *
                    * The 'Log In' button should call NGIO.OpenLoginPage();
                    *
                    * It is also good practice to provide a 'No Thanks' button
                    * for players who don't want to sign in.
                    *
                    * The 'No Thanks' button should call NGIO.SkipLogin();
                    */
                    break;

                case NGIO.STATUS_READY:
                    
                    /**
                    * The user has either logged in (or declined to do so), and everything else 
                    * has finished preloading.
                    */

                    if (NGIO.hasUser) {
                        /**
                        * The user is signed in!
                        * If they selected the 'remember me' option, their session id will be saved automatically!
                        * 
                        * Show a friendly welcome message! You can get their user name via:
                        *   NGIO.user.name
                        */
                        if(PlayerPrefs.GetString("NGID") == null || PlayerPrefs.GetString("NGID") == ""){
                            PlayerPrefs.SetString("NGID", "NewGrounds User " + NGIO.user.name);
                        }

                    } else {
                        /**
                        * The user doesn't want to sign in and use your cool features.
                        */
                    }

                    /**
                    * You can close any 'please wait' messages now!
                    */

                    break;
            }
        }
    }
}
