using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Socks
{
    public static class Utils 
    {
        /**
        * GetAllDescendents
        * - Pulls a list of all children and grandchildren recursively, given a "parent" transform
        */
        public static List<Transform> GetAllDescendents(Transform trunk)
        {
            List<Transform> newList = new List<Transform>();

            foreach (Transform stem in trunk)
            {
                newList.Add(stem);
                newList.AddRange(GetAllDescendents(stem));
            }

            return newList;
        }

        public static List<T> GetAllDescendents<T>(Transform trunk)
        {
            List<T> newList = new List<T>();

            foreach (Transform stem in trunk)
            {
                T element = stem.GetComponent<T>();
                if (element != null)
                {
                    newList.Add(stem.GetComponent<T>());
                }
                newList.AddRange(GetAllDescendents<T>(stem));
            }

            return newList;
        }

        public static List<Transform> GetAllDirectChildren(Transform trunk)
        {
            List<Transform> newList = new List<Transform>();

            foreach (Transform stem in trunk)
            {
                newList.Add(stem);
            }

            return newList;
        }

        /**
        * Util function to grab lines from txt files
        */
        public static List<string> LoadLinesFromFile(string filename)
        {
            List<string> lines = new List<string>();
            StreamReader sr = new StreamReader(Application.dataPath + filename);

            // Grab lines
            while (sr.Peek() >= 0)
            {
                lines.Add(sr.ReadLine());
            }

            return lines;
        }

        public static List<Color> LoadColoursFromFile(string filename)
        {
            List<Color> colours = new List<Color>();

            List<string> lines = LoadLinesFromFile(filename);

            foreach (string colour in lines)
            {
                Color parsedColour = new Color();

                if (ColorUtility.TryParseHtmlString(colour, out parsedColour))
                {
                    colours.Add(parsedColour);
                }
                else
                {
                    Debug.LogWarning("Error while loading '" + filename + "': Colour value '" + colour + "' is not a proper colour code. Will not be added.");
                }
            }

            return colours;
        }

        public static List<float> LoadFloatsFromFile(string filename)
        {
            List<float> floats = new List<float>();

            List<string> lines = LoadLinesFromFile(filename);

            foreach (string line in lines)
            {
                float attemptfloat = 0f;

                if (float.TryParse(line, out attemptfloat))
                {
                    floats.Add(attemptfloat);
                }
                else
                {
                    Debug.LogWarning("Error while loading '" + filename + "': Float value '" + line + "' could not be parsed. Will not be added.");
                }
            }

            return floats;
        }

        public static Vector3 AddVectorToTransform(Transform givenTransform, Vector3 add)
        {
            return (givenTransform.position + (givenTransform.up * add.y) + (givenTransform.forward * add.z) + (givenTransform.right * add.x));
        }

        public static float GetCurveLength(AnimationCurve curve)
        {
            return curve.keys[curve.keys.Length-1].time;
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static Vector3 ConvertWorldToScreen(Vector3 positionIn, Camera otherCam, Transform transform)
        {
            RectTransform rectTrans = transform.GetComponentInParent<RectTransform>(); //RenderTextHolder

            Vector2 viewPos = otherCam.WorldToViewportPoint (positionIn);
            Vector2 localPos = new Vector2 (viewPos.x * rectTrans.sizeDelta.x, viewPos.y * rectTrans.sizeDelta.y);
            Vector3 worldPos = rectTrans.TransformPoint (localPos);
            float scalerRatio = (1 / transform.lossyScale.x) * 2; //Implying all x y z are the same for the lossy scale

            return new Vector3(worldPos.x - rectTrans.sizeDelta.x / scalerRatio, worldPos.y - rectTrans.sizeDelta.y / scalerRatio, 1f);
        }

        public static Vector3 TransformPoint(Vector3 localPos, Vector3 pos, Quaternion rot)
        {
            return rot * localPos + pos;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            string toBuilt = "";
            foreach (byte b in bytes)
            {
                toBuilt += b.ToString();
            }

            return toBuilt;
        }

        public static void ScaleAround(Transform target, Vector3 pivot, Vector3 newScale)
        {
            Vector3 A = target.localPosition;
            Vector3 B = pivot;
        
            Vector3 C = A - B; // diff from object pivot to desired pivot/origin
        
            float RS = newScale.x / target.localScale.x; // relataive scale factor
        
            // calc final position post-scale
            Vector3 FP = B + C * RS;
        
            // finally, actually perform the scale/translation
            target.localScale = newScale;
            target.localPosition = FP;
        }

        public static void ScaleAround(RectTransform target, Vector3 pivot, Vector3 newScale)
        {
            Vector3 A = target.anchoredPosition;
            Vector3 B = pivot;
        
            Vector3 C = A - B; // diff from object pivot to desired pivot/origin
        
            float RS = newScale.x; // relataive scale factor
            if (target.localScale != Vector3.zero && target.localScale.x != 0)
            {
                RS /= target.localScale.x;
            }
        
            // calc final position post-scale
            Vector3 FP = B + C * RS;
        
            // finally, actually perform the scale/translation
            target.localScale = newScale;
            target.anchoredPosition = FP;
        }

        public static Direction GetDirection(WorldTile one, WorldTile two)
        {
            if (one.x < two.x)
            {
                return Direction.East;
            }
            else if (one.x > two.x)
            {
                return Direction.West;
            }
            else if (one.z > two.z)
            {
                return Direction.South;
            }
            else
            {
                return Direction.North;
            }
        }
    }
}