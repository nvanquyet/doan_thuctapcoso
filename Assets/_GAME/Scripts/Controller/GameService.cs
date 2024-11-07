using System;
using System.Collections.Generic;
using ShootingGame.Data;
using UnityEngine;
namespace ShootingGame
{

    public class GameService
    {
        public static Stat CaculateStat(Stat currentStat, Stat bonousStat, Stat baseStat)
        {
            if (bonousStat.Value == 0) return currentStat;
            if (currentStat.TypeValueStat == bonousStat.TypeValueStat) currentStat.SetValue(currentStat.Value + bonousStat.Value);
            else
            {
                if (currentStat.TypeValueStat == TypeValueStat.FixedValue)
                {
                    currentStat.SetValue(baseStat.Value + bonousStat.GetValue(baseStat.Value));
                }
                else
                {
                    currentStat.SetValue(bonousStat.Value + currentStat.GetValue(bonousStat.Value));
                    currentStat.SetTypeValueStat(bonousStat.TypeValueStat);
                }
            }
            return currentStat;
        }


        public static void ClearList<T>(ref List<T> list)
        {
            if (list == null) list = new();
            else list.Clear();
        }

        internal static void LogColor(string v)
        {
            Debug.Log($"<color=yellow>{v}</color>");
        }

        public static T ApplyWeightToValue<T>(T value, float weight)
        {
            return (T)Convert.ChangeType(value, typeof(T)) switch
            {
                int intValue => (T)Convert.ChangeType(intValue * weight, typeof(T)),
                float floatValue => (T)Convert.ChangeType(floatValue * weight, typeof(T)),
                _ => value
            };
        }

        public static T ApplyScaleFactorToValue<T>(T value, float scaleFactor)
        {
            return (T)Convert.ChangeType(value, typeof(T)) switch
            {
                int intValue => (T)Convert.ChangeType(Mathf.Pow(intValue, scaleFactor), typeof(T)),
                float floatValue => (T)Convert.ChangeType(Mathf.Pow(floatValue, scaleFactor), typeof(T)),
                _ => value
            };
        }

        public static T[,] GetMatrix<T>(T[,] matrix, int angle)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            T[,] result;


            switch (angle)
            {
                case -90:
                    {
                        result = new T[cols, rows];  // For -270° rotation (or 90° clockwise), swap rows and columns
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                // For 270° counter-clockwise rotation: reverse column order and adjust indices
                                result[j, rows - i - 1] = matrix[i, j];
                            }
                        }
                        break;
                        
                    }
                case -180:
                    {
                        result = new T[rows, cols];  // Keep the same dimensions
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                // For 180° rotation: reverse both rows and columns
                                result[rows - i - 1, cols - j - 1] = matrix[i, j];
                            }
                        }
                        break;
                    }
                case -270:
                    {
                        result = new T[cols, rows];  // For -90° rotation, swap rows and columns
                        for (int i = 0; i < rows; i++)
                        {
                            for (int j = 0; j < cols; j++)
                            {
                                // For 90° counter-clockwise rotation: convert columns to rows and reverse the column order
                                result[cols - j - 1, i] = matrix[i, j];
                            }
                        }
                        break;
                    }
                default:
                    {
                        result = matrix;  // If no valid angle, return the original matrix
                        break;
                    }
            }

            return result;
        }


        public static Vector2 GetItemSize(Vector2 baseSize, int angle)
        {
            return angle % 180 == 0 ? baseSize : new Vector2(baseSize.y, baseSize.x);
        }
    }

}