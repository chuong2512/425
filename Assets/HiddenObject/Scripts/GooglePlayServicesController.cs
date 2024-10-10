using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
//using GooglePlayGames;

public class GooglePlayServicesController {

    public static GooglePlayServicesController instance;

    private static Dictionary <string, string> achievments;

    public static void Authenticate (Actions.VoidVoid onSuccess = null, Actions.VoidVoid onFail = null) {

        Social.localUser.Authenticate ((bool success) => {

            Debug.Log ("Authenticate :" + success);

            if (success) {

                if (onSuccess != null) {

                    onSuccess ();
                }
            } else {

                if (onFail != null) {

                    onFail ();
                }
            }

        });
    }

    private static void CheckInstance () {

        if (instance == null) {

            new GooglePlayServicesController (true);
        }
    }

    private static void CheckAuthentification () {

        CheckInstance ();

        if (!Social.localUser.authenticated) {

            Authenticate ();
        }
    }

    public static void LoadAchievments (Actions.VoidVoid onEnd) {

        CheckAuthentification ();

        Social.LoadAchievements ((f) => {

            Debug.Log (f);

            foreach (var a in f) {

                Debug.Log ("LoadAchievements: " + a.id);
            }
        });
    }

    public static void LoadAchievementDescriptions (Actions.VoidVoid onEnd) {
        
        CheckAuthentification ();

        Social.LoadAchievementDescriptions ((g) => {

            foreach (var a in g) {

                Debug.Log ("LoadAchievementDescriptions: " + a.id + " " + a.title);
                
                if (!achievments.ContainsValue (a.id)) {

                    achievments.Add (a.title, a.id);
                }
            }
        });
    }

    /// <summary>
    /// Show Achievments UI
    /// </summary>
    public static void ShowAchievments () {
        
        CheckAuthentification ();

        Social.ShowAchievementsUI ();
    }

    /// <summary>
    /// Report progress for an achievment
    /// </summary>
    /// <param name="achievment">achievment name in the dictionary</param>
    /// <param name="toAdd">0 .. 100</param>
    /// <param name="onSuccess"></param>
    /// <param name="onFail"></param>
    public static void ReportProgress (string achievment, float toAdd, Actions.VoidVoid onSuccess = null, Actions.VoidVoid onFail = null) {
        
        CheckAuthentification ();

        if (!achievments.ContainsKey (achievment)) {

            Debug.Log ("Unknown achievment");
            onFail ();
            return;
        }
        return;
        Social.ReportProgress (achievments [achievment], toAdd, (h) => {

            if (h) {

                if (onSuccess != null) {

                    onSuccess ();
                }
            } else {

                if (onFail != null) {

                    onFail ();
                }
            }
        });
    }

    /// <param name="isMinimal">Authentificate and load data if false</param>
    /// <param name="_achievments">Dictionary name - id. Use name to call RportProgress</param>
    /// <param name="onLoad"></param>
    public GooglePlayServicesController (bool isMinimal = true, Dictionary <string, string> _achievments = null, Actions.VoidVoid onLoad = null) {

        instance = this;

        //GooglePlayGames.PlayGamesPlatform.Activate();

        if (!isMinimal) {

            if (_achievments == null) {

                achievments = new Dictionary<string, string> ();
            } else {

                achievments = _achievments;
            }

            Authenticate (() => {

                LoadAchievments (() => {

                    LoadAchievementDescriptions (() => {

                        if (onLoad != null) {

                            onLoad ();
                        }
                    });
                });
            });
        }
    }

}
