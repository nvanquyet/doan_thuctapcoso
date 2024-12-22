using ShootingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
namespace UnityEngine
{

    public static class Extension
    {

#if UNITY_EDITOR
        [UnityEditor.MenuItem("GAME/PLAY")]
        private static void PlayGame()
        {
            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                string firstScenePath = EditorBuildSettings.scenes[0].path;
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(firstScenePath);

                EditorApplication.isPlaying = true;
            }
        }
#endif
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }


        public static Rect Normalize(this Rect rect)
        {
            if (rect.xMin > rect.xMax) (rect.xMin, rect.xMax) = (rect.xMax, rect.xMin);
            if (rect.yMin > rect.yMax) (rect.yMin, rect.yMax) = (rect.yMax, rect.yMin);
            return rect;
        }

        public static Rect SafeRect(this Tuple<Vector3, Vector3> tuple)
        {
            var minX = Mathf.Min(tuple.Item1.x, tuple.Item2.x);
            var minY = Mathf.Min(tuple.Item1.y, tuple.Item2.y);
            var maxX = Mathf.Max(tuple.Item1.x, tuple.Item2.x);
            var maxY = Mathf.Max(tuple.Item1.y, tuple.Item2.y);
            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }

        public static Vector3 Clamp(this Vector3 vector, Rect rect)
        {
            if (rect.xMin > rect.xMax) (rect.xMin, rect.xMax) = (rect.xMax, rect.xMin);
            if (rect.yMin > rect.yMax) (rect.yMin, rect.yMax) = (rect.yMax, rect.yMin);
            var x = Mathf.Clamp(vector.x, rect.xMin, rect.xMax);
            var y = Mathf.Clamp(vector.y, rect.yMin, rect.yMax);
            return new Vector3(x, y, vector.z);
        }


        public static Vector3 Rotate(this Vector3 vector, Vector3 rotation, bool inverse = false)
        {
/*            Debug.Log(rotation);*/
            if (rotation.x % 180 >= 90) (vector.z, vector.y) = (vector.y, vector.z);       
            if (rotation.y % 180 >= 90) (vector.x, vector.z) = (vector.z, vector.x);
            if (rotation.z % 180 >= 90) (vector.x, vector.y) = (vector.y, vector.x);
            if (inverse && rotation.x >= 180) (vector.z, vector.y) = (-vector.z, -vector.y);
            if (inverse && rotation.y >= 180) (vector.x, vector.z) = (-vector.x, -vector.z);
            if (inverse && rotation.z >= 180) (vector.x, vector.y) = (-vector.x, -vector.y);
            return vector;
        }
        public static Rect GetScreenRect(this Renderer renderer)
        {
            Vector3 cen = renderer.bounds.center;
            Vector3 ext = renderer.bounds.extents;
            Vector2 min = new(), max = new();

            for (int i=0; i<8; i++)
            {
                
                Vector3Int vector = new(i /4 % 2, i / 2 % 2, i % 2);
                vector = Vector3Int.one - 2 * vector;
                Vector2 point = Camera.main.WorldToScreenPoint(cen+ext.Multiply(vector));
                if (i==0) min = max = point;
                else
                {
                    min = point.Min(min);
                    max = point.Max(max);
                }
            }
            return Rect.MinMaxRect(min.x,min.y,max.x,max.y);
        }

        public static float Min(this float number, float min) => number < min ? number : min;
        public static float Max(this float number, float max) => number > max ? number : max;

        public static Vector2 Min(this Vector2 vector, Vector2 min) => new(vector.x.Min(min.x), vector.y.Min(min.y));

        public static Vector2 Max(this Vector2 vector, Vector2 min) => new(vector.x.Max(min.x), vector.y.Max(min.y));
        public static T Instantiate<T>(this T prefab) where T : Component
        {
            prefab.gameObject.RegisterPool();
            T result = prefab.Spawn(prefab.transform.position, prefab.transform.lossyScale, prefab.transform.rotation); //Object.Instantiate(prefab);
            result.name = prefab.name;
            return result;
        }
        public static Vector3[] GetScreenCorners(this Camera camera, float widthScale = 1, float heightScale = 1)
        {
            var widthOffset = (1 - widthScale) / 2 * camera.pixelWidth;
            var heightOffset = (1 - heightScale) / 2 * camera.pixelHeight;
            return new Vector3[]
            {
                new Vector2(widthOffset,heightOffset),
                new Vector2(widthOffset,camera.pixelHeight-heightOffset),
                new Vector2(camera.pixelWidth-widthOffset,camera.pixelHeight-heightOffset),
                new Vector2(camera.pixelWidth-widthOffset,heightOffset)
            };
        }

        


        public static string ToTime(this float time, bool showHour = true, bool showMinutes = true, bool showSeconds = true) 
            => ((int)time).ToTime(showHour, showMinutes, showSeconds);

        public static string ToTime(this int time, bool showHour = true, bool showMinutes = true, bool showSeconds = true)
        {
            
            int hour = time/3600;
            time %= 3600;
            int minutes = time / 60;
            time %= 60;
            int seconds = time;

            Dictionary<string, int> t = new();
            if (showHour) t.Add("h", hour);
            if (showMinutes || (showHour && showSeconds)) t.Add("m", minutes);
            if (showSeconds) t.Add("s", seconds);

            var result = "";

            var keys = t.Keys.ToList();
            for (int i = 0; i<keys.Count; i++)
                if (i == 0) result += t[keys[i]].ToString();
                else result += string.Format(":{0:00}", t[keys[i]]);
            return result;
        }
        public static Vector3 ScreenCenter(this Camera camera) => new(camera.pixelWidth / 2, camera.pixelHeight / 2);

        public static Vector3 ScreenCenter(this Camera camera, float z)
            => camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth / 2, camera.pixelHeight / 2, Camera.main.nearClipPlane + z));

        public static Vector3 ScreenTopRight(this Camera camera) => camera.ScreenToWorldPoint
            (new Vector2(camera.pixelWidth, camera.pixelHeight)) + Vector3.forward;

        public static int ToInt(this string s)
        {
            var getNumbers = (from t in s where char.IsDigit(t) select t).ToArray();
            var stringNumber = new string(getNumbers);
            return int.Parse(stringNumber);
        }

        public static Vector3Int Divide(this Vector3Int left, Vector3Int right)
        {
            return new Vector3Int(left.x / right.x, left.y / right.y, left.z / right.z);
        }


        public static Vector3 Divide(this Vector3 left, Vector3 right)
        {
            return new Vector3(left.x / right.x, left.y / right.y, left.z / right.z);
        }

        public static Vector3 Divide(this Vector3 left, float x = 1, float y = 1, float z = 1) 
            => new(left.x / x, left.y / y, left.z / z);

        public static Vector3 Divide(this Vector3 left, Vector2 right) 
            => new(left.x / right.x, left.y / right.y, left.z);

        public static Vector2 Divide(this Vector2 left, Vector2 right)
        {
            return new Vector3(left.x / right.x, left.y / right.y);
        }

        public static Vector3 Multiply(this Vector3 left, Vector3 right)
        {
            return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
        }

        public static Vector2 Multiply(this Vector2 left, Vector2 right) {
            return new Vector2(left.x * right.x, left.y * right.y);
        }

        public static Vector3 Multiply(this Vector3 left, float x = 1, float y = 1, float z = 1) 
            => new(left.x * x, left.y * y, left.z * z);

        public static float ScalarMultiply(this Vector3 left, Vector3 right)
        {
            var temp = left.Multiply(right);
            return temp.x + temp.y + temp.z;
        }
        public static float ScalarMultiply(this Vector2 left, Vector2 right) {
            Vector2 temp = left.Multiply(right);
            return temp.x + temp.y;
        }


        public static float GetAngleInRadians(this Vector2 left, Vector2 right) {
            return Mathf.Acos(left.ScalarMultiply(right) / (left.magnitude + right.magnitude));
        }

        public static float GetAngleInDegrees(this Vector2 left, Vector2 right) {
            return left.GetAngleInRadians(right) * Mathf.Rad2Deg;
        }

        public static bool GetIntersection((Vector2, Vector2) vector1, (Vector2, Vector2) vector2, out Vector2 ret) {
            Vector2 v1, v2, v3;
            v1 = vector1.Item2 - vector1.Item1;
            v2 = vector2.Item2 - vector2.Item1;
            v3 = vector1.Item1 - vector2.Item1;

            static float f(Vector2 left, Vector2 right) => left.x * right.y - left.y * right.x;

            float t1 = f(v1, v3) / f(v1, v2);
            float t2 = f(v2, v3) / f(v1, v2);
            if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1) {
                ret = vector1.Item1 + t2 * v1;
                return true;
            }
            ret = Vector2.zero;
            return false;
        }

        public static Vector3 ToVector3XZ(this Vector2 vector) => new(vector.x, 0, vector.y);

        public static Vector2 SetX(this Vector2 vector, float x) => new(x, vector.y);
        public static Vector2 SetY(this Vector2 vector, float y) => new(vector.x, y);

        public static Vector3 SetX(this Vector3 vector, float x) => new(x, vector.y, vector.z);
        public static Vector3 SetY(this Vector3 vector, float y) => new(vector.x, y, vector.z);
        public static Vector3 SetZ(this Vector3 vector, float z) => new(vector.x, vector.y, z);

    }

}

namespace System.Collections
{
    static class Extension
    {

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        } 
        public static string ToText<T>(this IList<T> list, string delimiter = "; ")
        {
            string result = "";
            foreach (var item in list)
            {
                result += ((item != null) ? item.ToString() : "NULL") + delimiter;
            }
            return result;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; ++i)
            {
                var r = new Random().Next(i, list.Count - 1);
                (list[r], list[i]) = (list[i], list[r]);
            }
        }

        public static T[] Add<T>(this T[] target, T item)
        {
            target ??= new T[0];
            var result = target.ToList();
            result.Add(item);
            return result.ToArray();
        }

        public static T[] Remove<T>(this T[] target, T item)
        {
            target ??= new T[0];
            var result = target.ToList();
            result.Remove(item);
            return result.ToArray();
        }

        public static bool Contains<T>(this T[] target, T item)
        {
            if (target == null) return false;
            return target.ToList().Contains(item);
        }

        public static void Toggle<T>(this IList<T> list, bool condition, T item, bool forced)
        {
            if (list == null) return;
            if (condition)
            {
                if (forced)
                {
                    if (list.Count == 1) list.Add(item);
                }
                else if (!list.Contains(item)) list.Add(item);
            }
            else
            {
                if (list.Contains(item)) list.Remove(item);
                else if (forced && list.Count > 1) list.RemoveAt(list.Count - 1);
            }
            
        }


        public static T[] Toggle<T>(this T[] target, bool condition, T item, bool forced = false)
        {
            var temp = target.ToList();
            temp.Toggle(condition, item, forced);
            return temp.ToArray();
        }
    }
    
}
