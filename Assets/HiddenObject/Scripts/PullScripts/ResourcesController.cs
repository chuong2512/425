﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcesController {

    public static ResourcesController instance;

    private Dictionary <string, Object> resources;

	public ResourcesController () {

        if (instance != null)
            return;

        instance = this;
        resources = new Dictionary<string, Object> ();
    }

    public static Object Load (string path, bool isIgnore = false) {

        if (instance == null) {

            new ResourcesController ();
        }

        if (instance.resources.ContainsKey (path)) {

            return instance.resources [path];
        } else {


            var res = Resources.Load (path);

            if (res == null && !isIgnore) {

                Debug.LogError ("No such resource '" + path +"'");
                return null; 
            } else {

                if (res != null) {

                    instance.resources.Add (path, res);
                }

                return res;
            }
        }
    }

    public static Object LoadIgnore (string path) {

        return Load (path, true);
    }


    public static Object LoadOnce (string path) {
        
        return Resources.Load (path);
    }

    public static Texture LoadCompressedTexture (string path, bool isIgnore = false) {

        if (instance == null) {

            new ResourcesController ();
        }

        if (instance.resources.ContainsKey (path)) {

            return instance.resources [path] as Texture;
        } else {


            var res = TextureCompressor.Load (path);

            if (res == null && !isIgnore) {

                Debug.LogError ("No such resource '" + path +"'");
                return null; 
            } else {

                if (res != null) {

                    instance.resources.Add (path, res);
                }

                return res;
            }
        }
    }

    public static Texture LoadCompressedTextureOnce (string path, bool isIgnore = false) {

        return TextureCompressor.Load (path);
    }

    public static void Destroy () {
        
        if (instance == null) {

            return;
        }

        instance.resources.Clear ();
        instance = null;
    }
}
