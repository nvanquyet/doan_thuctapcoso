using System;
using System.Collections.Generic;
using System.Linq;
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


        //public static T ApplyScaleFactorToValue<T>(T value, float scaleFactor)
        //{
        //    return (T)Convert.ChangeType(value, typeof(T)) switch
        //    {
        //        int intValue => (T)Convert.ChangeType(Mathf.Pow(intValue, scaleFactor), typeof(T)),
        //        float floatValue => (T)Convert.ChangeType(Mathf.Pow(floatValue, scaleFactor), typeof(T)),
        //        _ => value
        //    };
        //}
        public static T ApplyScaleFactorToValue<T>(T value, float scaleFactor)
        {
            return (T)Convert.ChangeType(value, typeof(T)) switch
            {
                int intValue => (T)Convert.ChangeType(intValue * Mathf.Pow(1 + scaleFactor, 1), typeof(T)),
                float floatValue => (T)Convert.ChangeType(floatValue * Mathf.Pow(1 + scaleFactor, 1), typeof(T)),
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


        public static int RandomLevel()
        {
            int roll = UnityEngine.Random.Range(1, 101);
            return roll <= 90 ? 1 : 2; // 90% Level 1, 10% Level 2
        }

        public static T RandomItem<T>(List<T> items) where T : ItemDataSO
        {
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("Items list is null or empty.");
                return null;
            }

            List<T> filteredItems = new List<T>();

            if (items.Any(i => i is ItemEquiqmentData))
            {
                var equiqmentDatas = items.OfType<ItemEquiqmentData>().ToList();
                int level = RandomLevel();
                items = equiqmentDatas
                                .Where(i => i.Level == (LevelItem)level)
                                .Cast<T>()
                                .ToList();
            }
            filteredItems = items;

            var rarityRandom = UnityEngine.Random.Range(1, 101);

            if (rarityRandom <= 80)
            {
                filteredItems = filteredItems.Where(i => i.Rarity == RarityItem.Common).ToList();
            }
            else if (rarityRandom <= 95)
            {
                filteredItems = filteredItems.Where(i => i.Rarity == RarityItem.Rare).ToList();
            }
            else if (rarityRandom <= 98)
            {
                filteredItems = filteredItems.Where(i => i.Rarity == RarityItem.Epic).ToList();
            }
            else
            {
                filteredItems = filteredItems.Where(i => i.Rarity == RarityItem.Legendary).ToList();
            }
            if (filteredItems.Count == 0)
            {
                int indexItems = UnityEngine.Random.Range(0, items.Count);
                return items[indexItems];
            }

            int index = UnityEngine.Random.Range(0, filteredItems.Count);
            return filteredItems[index];
        }

        public static int CalculateScore(int enemiesDefeated, float timeLeft)
        {
            int baseScore = enemiesDefeated * 100;
            int timeBonus = Mathf.FloorToInt(timeLeft * 50);
            return baseScore + timeBonus + baseScore;
        }
        public static int CalculateStars(int score, int maxScore)
        {
            float percentage = (float)score / maxScore;
            if (percentage >= 0.9f) return 3; 
            else if (percentage >= 0.7f) return 2;
            else if (percentage >= 0.5f) return 1;
            else return 0;
        }

        public static int CalculateGold(int baseGold, float scalingFactor, int currentWave, float rarityBonus)
        {
            float scaledGold = baseGold * Mathf.Pow(scalingFactor, currentWave - 1);
            float totalGold = scaledGold * (1 + rarityBonus);   
            return Mathf.FloorToInt(totalGold);                                  
        }

        public static WaveProperties CalculateWaveProperties(int currentWave, float scaleFactor)
        {
            WaveProperties waveProperties = new WaveProperties
            {
                timeNormalSpawn = GameConfig.Instance.waveProperties.timeNormalSpawn / Mathf.Pow(scaleFactor, currentWave),
                timeThreshold = GameConfig.Instance.waveProperties.timeThreshold * Mathf.Pow(scaleFactor, currentWave),
                spawnThreshold = Mathf.RoundToInt(GameConfig.Instance.waveProperties.spawnThreshold * Mathf.Pow(scaleFactor, currentWave)),
                strengthWave = Mathf.RoundToInt(GameConfig.Instance.waveProperties.strengthWave * Mathf.Pow(scaleFactor, currentWave)),
                timeWave = Mathf.RoundToInt(GameConfig.Instance.waveProperties.timeWave / Mathf.Pow(scaleFactor, currentWave))
            };

            return waveProperties;
        }

    }

}