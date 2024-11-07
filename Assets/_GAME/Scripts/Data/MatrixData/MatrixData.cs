using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MatrixData", menuName = "Custom/MatrixData")]
public class MatrixData : ScriptableObject
{

    [Serializable]
    protected struct MatrixStruct
    {
        public int[] values;
    }
    public Vector2 itemSize;
    [SerializeField] private MatrixStruct[] matrixStruct;

    public int[,] Matrix
    {
        get
        {
            int[,] matrix = new int[(int)itemSize.y, (int)itemSize.x];
            for (int i = 0; i < itemSize.y; i++)
            {
                for (int j = 0; j < itemSize.x; j++)
                {
                    matrix[i, j] = matrixStruct[i].values[j];
                }
            }

            return matrix;
        }
    }


}

