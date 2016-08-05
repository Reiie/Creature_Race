/*
 * Copyright (C) 2014 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class AutoLogin : MonoBehaviour
{
    private bool bWatingForAuth = false;
    private string mStatusText = "";

    Rect labelRect = new Rect(20, 20, Screen.width, Screen.height * 0.25f);
    Rect imageRect = new Rect(Screen.width / 2f - 200f, Screen.height / 2f - 200f, 400f, 400f);


    void Start()
    {
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();

        doLogin();
    }

    void doLogin()
    {
        if (bWatingForAuth)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            //mStatusText = "Authenticating";
            bWatingForAuth = true;
            Social.localUser.Authenticate((bool success) =>
            {
                bWatingForAuth = false;
                if (success)
                {
                    //mStatusText = "Welcome " + Social.localUser.userName;
                    string token = GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
                    Debug.Log(token);
                }
                else
                {
                    //mStatusText = "Authentication failed";
                }
            });
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = (int)(0.05 * Screen.height);
        GUI.Label(labelRect, mStatusText);

        if (Social.localUser.authenticated)
        {
            if (Social.localUser.image != null)
            {
                //GUI.DrawTexture(imageRect, Social.localUser.image, ScaleMode.ScaleToFit);
            }
        }
    }
}
