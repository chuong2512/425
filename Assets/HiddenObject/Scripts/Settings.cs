using UnityEngine;

public class Settings {

	public enum Location {EnchantedForest, LonelyHills, Scara};
	public static Location location = Location.EnchantedForest;

	public enum Language {English, Russian};
	public static Language language {
		get { 
			if (!PlayerPrefs.HasKey ("language"))
				PlayerPrefs.SetInt ("language", (int) Language.English);

			return (Language) PlayerPrefs.GetInt ("language"); 
		}

		set { 
			PlayerPrefs.SetInt ("language", (int) value);
		}
	}

	public static bool isTutorial {
		get { 
			if (!PlayerPrefs.HasKey ("isTutorial")) 
			    return true;
			
			return (PlayerPrefs.GetInt("isTutorial") == 1);
		}
		set {
			PlayerPrefs.SetInt("isTutorial",value?1:0);
		}
		
	}

    public static int maxItemsToFind {
		get { 

            return 8;

            /*
			if (!PlayerPrefs.HasKey ("maxItemsToFind")) 
			    return 8;
			
			return (PlayerPrefs.GetInt("maxItemsToFind") );*/
		}
		set {
			PlayerPrefs.SetInt("maxItemsToFind", value);
		}
		
	}

    public static bool isOpenedAllLevelsAndLocations = true;

	public static int levelPack = 0;
	public static int level = 0;

	public static int forOneItem = 150;
	public static int forOneStar = 0;
	public static int forTwoStars = 400;
	public static int forThreeStars = 800;

	public static bool isLevelEnded = false;
	public static int scoreEnded = 0;
	public static int starsEnded = 3;
    
    public static Vector2 purchasedLevel = new Vector2 (-1, -1);
    public static string purchasedLocation = "";
    public static GUIButton purchasedLocationButton = null;
    public static string purchasedLocationTexture = "";

	public static string adsIdAndroid = "ca-app-pub-4782252445867445/3279633612";
	public static string configFresco = "1200||1|205|167|875.00|643.00||2|180|145|900.00|569.00||3|189|158|891.00|439.00||4|193|152|887.00|356.00||5|169|133|911.00|256.00||6|271|154|809.00|138.00||7|257|131|823.00|54.00||8|248|105|832.00|0.00||9|176|195|691.00|0.00||10|152|190|563.00|0.00||11|202|162|380.00|0.00||12|181|149|219.00|0.00||13|180|175|75.00|0.00||14|158|159|0.00|0.00||15|180|141|0.00|129.00||16|117|234|0.00|247.00||17|136|188|0.00|451.00||18|104|189|0.00|621.00||19|262|116|71.00|694.00||20|237|117|303.00|693.00||21|274|103|505.00|707.00||22|154|211|748.00|599.00||23|143|238|788.00|401.00||24|174|137|784.00|273.00||25|166|159|657.00|211.00||26|210|125|602.00|119.00||27|203|116|402.00|113.00||28|249|195|175.00|69.00||29|194|255|112.00|178.00||30|127|242|55.00|248.00||31|108|183|98.00|415.00||32|135|137|74.00|575.00||33|249|125|186.00|613.00||34|248|138|352.00|600.00||35|201|222|574.00|516.00||36|149|242|664.00|417.00||37|139|142|721.00|319.00||38|232|161|500.00|317.00||39|206|165|470.00|188.00||40|220|103|331.00|200.00||41|191|208|282.00|251.00||42|144|147|252.00|386.00||43|152|250|167.00|416.00||44|164|180|297.00|483.00||45|184|122|439.00|526.00||46|177|169|513.00|395.00||47|155|156|374.00|404.00||48|122|169|412.00|303.00||";
	public static string configPoints = "150_2150_2200_2250|200_3000_3100_3200|250_3700_3750_4000|300_4500_4800_5100|350_5250_5500_5950|400_6000_6800_7200|450_6750_7650_8100|500_7500_9000_9500|550_8250_8500_8800|600_9000_9600_10200|650_9750_10500_11050|700_10500_11200_12600|750_11250_12000_13500|800_12000_13400_15200|850_12750_14250_16150|900_13500_15500_18000|950_14250_15350_16150|1000_15000_16500_18000|1050_15750_17600_18900|1100_16500_18800_20900|1150_17250_19250_21850|1200_18000_20000_24000|1250_18750_20500_25000|1300_19500_22500_27300|1350_20000_20150_20250|1400_21000_21900_22400|1450_21750_22200_23200|1500_22500_23250_25500|1550_23250_25000_26350|1600_24000_26000_28800|1650_24750_25650_29700|1700_25000_28300_32300|1750_26250_27000_28000|1800_27000_28500_30600|1850_27750_29250_31450|1900_28500_30500_34200|1950_29250_33150_35100|2000_30000_34000_38000|2050_30750_34250_38950|2100_31500_36750_42000|2150_32250_34250_36550|2200_33000_35250_39600|2250_33750_35750_40500|2300_34500_38250_43700|2350_35250_39500_44650|2400_36000_40500_48000|2450_36750_42750_49000|2500_37500_43500_52500|2550_33750_33850_33950|2600_39000_40000_41600|2650_39750_42000_43400|2700_40500_43500_45900|2750_41250_44250_46750|2800_42000_45600_50400|2850_42750_48250_51300|2900_43500_50250_55100|2950_44250_46250_47200|3000_45000_48000_51000|3050_45750_49250_51850|3100_46500_50500_55800|3150_47250_52350_56700|3200_48000_55250_60800|3250_48750_57750_61750|3300_49500_59250_66000|3350_50250_53750_56950|3400_51000_54250_61200|3450_51750_55500_62100|3500_52500_58250_66500|3550_53250_60000_67450|3600_54000_62500_72000|3650_54750_65000_73000|3700_55500_66500_82000|";
	public static int frescoIndex = -1;

	public static bool music {
		get {

            if (!PlayerPrefs.HasKey ("music"))
                return true;

			return (PlayerPrefs.GetInt("music") == 1);
		}
		set {
            
            if (value) 
                AudioController.instance.UnMuteMusic ();
           else 
                AudioController.instance.MuteMusic ();

			PlayerPrefs.SetInt("music",value?1:0);
		}
		
	}
	
	public static bool sounds {
		get {
            
            if (!PlayerPrefs.HasKey ("sounds"))
                return true;

            return (PlayerPrefs.GetInt("sounds") == 1); }
		set {

            if (value) 
                AudioController.instance.UnMuteSounds ();
           else 
                AudioController.instance.MuteSounds ();

			PlayerPrefs.SetInt("sounds",value?1:0);
		}
		
	}

	public static string GetBeforeLevelLevelName () {

		switch (location) {

		case Location.EnchantedForest : 
			
			switch (levelPack) {
			case 0:
				switch (language) {
				case Language.English:
					return "Dwarfs Liar";
				case Language.Russian:
					return "C Dpst>w u Dopnpc";
				}
				break;
			case 1:
				switch (language) {
				case Language.English:
					return "Forest Fairies";
				case Language.Russian:
					return "Mfso}f Vfj";
				}
				break;
			case 2:
				switch (language) {
				case Language.English:
					return "Elven House";
				case Language.Russian:
					return "_m~vjksljk Epn";
				}
				break;
				
			}
			
			break;
		case Location.LonelyHills : 
			
			switch (levelPack) {
			case 0:
				switch (language) {
				case Language.English:
					return "Ancient Gates";
				case Language.Russian:
					return "Qfrcpb}to}f Crata";
				}
				break;
			case 1:
				switch (language) {
				case Language.English:
					return "Abandoned Land";
				case Language.Russian:
					return "Star}k Dprpe";
				}
				break;
			case 2:
				switch (language) {
				case Language.English:
					return "Old Fountain";
				case Language.Russian:
					return "Nranpro}k Vpotao";
				}
				break;
				
			}
			
			break;
		case Location.Scara : 
			
			switch (levelPack) {
			case 0:
				switch (language) {
				case Language.English:
					return "Trade Square";
				case Language.Russian:
					return "Tprdpca> Qmp{ae~";
				}
				break;
			case 1:
				switch (language) {
				case Language.English:
					return "Master Quarter";
				case Language.Russian:
					return "Lcartam Nastfrpc";
				}
				break;
			case 2:
				switch (language) {
				case Language.English:
					return "Mill-Eater";
				case Language.Russian:
					return "Nfm~ojxa-M<epfe";
				}
				break;
				
			}
			
			break;


		}

		return "UNDEF";
	}

	private static string GetBeforeLevelTextName () {
		switch (location) {
			
		case Location.EnchantedForest : 
			
			switch (levelPack) {
			case 0:
				switch (language) {
				case Language.English:
					return "dwarfs lair";
				case Language.Russian:
					return "epn dopnpc";
				}
				break;
			case 1:
				switch (language) {
				case Language.English:
					return "forest fairies";
				case Language.Russian:
					return "pifrp vfk";
				}
				break;
			case 2:
				switch (language) {
				case Language.English:
					return "elven house";
				case Language.Russian:
					return "#m~vjksljk epn";
				}
				break;
				
			}
			
			break;
		case Location.LonelyHills : 
			
			switch (levelPack) {
			case 0:
				switch (language) {
				case Language.English:
					return "ancient gates";
				case Language.Russian:
					return "erfcojf crata";
				}
				break;
			case 1:
				switch (language) {
				case Language.English:
					return "abandoned land";
				case Language.Russian:
					return "star}k dprpe";
				}
				break;
			case 2:
				switch (language) {
				case Language.English:
					return "old fountain";
				case Language.Russian:
					return "nranpro}k vpotao";
				}
				break;
				
			}
			
			break;
		case Location.Scara : 
			
			switch (levelPack) {
			case 0:
				switch (language) {
				case Language.English:
					return "trade square";
				case Language.Russian:
					return "tprdpcu< qmp{ae~";
				}
				break;
			case 1:
				switch (language) {
				case Language.English:
					return "master quarter";
				case Language.Russian:
					return "lcartam nastfrpc";
				}
				break;
			case 2:
				switch (language) {
				case Language.English:
					return "mill-eater";
				case Language.Russian:
					return "nfm~ojxu-m<epfe";
				}
				break;
				
			}
			
			break;
			
			
		}
		
		return "UNDEF";
	}


	public static string GetBeforeLevelText () {

		switch (language) {
			
		case Language.English:
			return "Explore " + GetBeforeLevelTextName () + " and\nlook out "+maxItemsToFind+" items, that\nProfessor Madgard have put.\nHurry, professor does not\nlike to wait long.";
		case Language.Russian:
			return "Jssmfeuk " + GetBeforeLevelTextName () + " j\npt}{j "+maxItemsToFind+" qrfenftpc,\nlptpr}f sqr>tam qrpvfsspr\nNaedare. Qpsqfzj, qrpvfsspr\nof m<bjt epmdp heat~.";

			 
		}
		return "Yutpylu";
	}
		
    
    private static string eng = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz[{\\|]}^~_#&</>";
    private static string rus = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";

    private static char GetRight (char c) {


        for (int i = 0; i < rus.Length ; i++) {
         
            if (rus[i] == c)
            return eng[i];
        }

        return c;
    }
    public static string TranslateText (string rus) {

        string res = "";

        for (int i = 0; i < rus.Length; i++) {

            res += GetRight (rus[i]);
        }

        return res;
    }

    public static int GetFor3StarsPoints (int level, int levelPack) {

        switch (levelPack) {
            
            case 0:

                switch (level) {
                    
                    case 0:
                        return 12;
                    case 1:
                        return 13;
                    case 2:
                        return 13;
                    case 3:
                        return 14;
                    case 4:
                        return 14;
                    case 5:
                        return 15;
                    case 6:
                        return 15;
                    case 7:
                        return 16;
                }

                break;

            case 1:
                
                switch (level) {
                    
                    case 0:
                        return 13;
                    case 1:
                        return 13;
                    case 2:
                        return 14;
                    case 3:
                        return 14;
                    case 4:
                        return 15;
                    case 5:
                        return 15;
                    case 6:
                        return 16;
                    case 7:
                        return 16;
                }
                break;

            case 2:
                
                switch (level) {
                    
                    case 0:
                        return 13;
                    case 1:
                        return 14;
                    case 2:
                        return 14;
                    case 3:
                        return 15;
                    case 4:
                        return 15;
                    case 5:
                        return 16;
                    case 6:
                        return 16;
                    case 7:
                        return 17;
                }
                break;

        }

        return 13;
    }
}
