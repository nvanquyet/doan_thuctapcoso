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
    [SerializeField] private Vector2 itemSize;
    [SerializeField] private bool isUniformMatrix;
    [SerializeField] private MatrixStruct[] matrixStruct;
    public bool IsUniformMatrix => isUniformMatrix;
    public Vector2 ItemSize => itemSize;
    public int[,] Matrix
    {
        get
        {
            int[,] matrix = new int[(int)ItemSize.y, (int)ItemSize.x];
            for (int i = 0; i < ItemSize.y; i++)
            {
                for (int j = 0; j < ItemSize.x; j++)
                {
                    matrix[i, j] = matrixStruct[i].values[j];
                }
            }

            return matrix;
        }
    }

    private void OnValidate()
    {
        if (isUniformMatrix)
        {
            matrixStruct = new MatrixStruct[(int)itemSize.y];
            for (int i = 0; i < itemSize.y; i++)
            {
                matrixStruct[i].values = new int[(int)itemSize.x];
                for (int j = 0; j < itemSize.x; j++)
                {
                    matrixStruct[i].values[j] = 1;
                }
            }
        }
    }
}

