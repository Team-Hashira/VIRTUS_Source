using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public static class DamageHandlerOrder
    {
        public static List<EDamageHandlerLayer> OrderList;

        static DamageHandlerOrder()
        {
            OrderList = new List<EDamageHandlerLayer>();
            int order = PlayerPrefs.GetInt("DamageHandlerLayerOrder");
            int digit = PlayerPrefs.GetInt("DamageHandlerLayerOrderDigit");
            if (order == 0)
            {
                foreach (EDamageHandlerLayer layerEnum in Enum.GetValues(typeof(EDamageHandlerLayer)))
                {
                    OrderList.Add(layerEnum);
                }
            }
            else
            {
                while (order > 0)
                {
                    int value = order % 10;
                    int curDigit = digit % 10;
                    for (int i = 1; i < curDigit; i++)
                    {
                        value *= 10;
                        order = (int)(order * 0.1f);
                        value += order % 10;
                    }
                    EDamageHandlerLayer enumValue = (EDamageHandlerLayer)value;
                    OrderList.Add(enumValue);
                    order /= 10;
                    digit /= 10;
                }
                OrderList.Reverse();
            }
        }

        public static void Initialize()
        {
            OrderList = new List<EDamageHandlerLayer>();
            foreach (EDamageHandlerLayer layerEnum in Enum.GetValues(typeof(EDamageHandlerLayer)))
            {
                OrderList.Add(layerEnum);
            }
            PlayerPrefs.SetInt("DamageHandlerLayerOrder", 0);
            PlayerPrefs.SetInt("DamageHandlerLayerOrderDigit", 0);
        }

        public static void Reorder(List<EDamageHandlerLayer> list)
        {
            OrderList = list;
            int order = 0;
            int digit = 0;
            foreach (EDamageHandlerLayer layerEnum in OrderList)
            {
                order *= 10;
                int enumValue = (int)layerEnum;
                order += enumValue;
                int length = enumValue.ToString().Length;
                digit *= 10;
                digit += 1;
                if (length > 0)
                {
                    digit += length - 1;
                }
            }
            PlayerPrefs.SetInt("DamageHandlerLayerOrder", order);
            PlayerPrefs.SetInt("DamageHandlerLayerOrderDigit", digit);
        }
    }
}
