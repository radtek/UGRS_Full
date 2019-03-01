using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    /*public enum TravelExpMovTypesEnum : int
    {
        [Description("Viáticos")]
        TravelExpenses = 12,
        [Description("Gastos por D")]
        DayExpenses = 13,
        [Description("Gastos Men.")]
        MinorExpenses = 11,
    }*/

    public class TravelExpMovTypesEnum
    {
        public class TravelExpenses : TravelExpMovTypesEnum
        {
            public const string Value = "MQDIVI";
            public const string Description = "Viáticos (A razon de 100 Diarios)";
        }

        public class DayExpenses : TravelExpMovTypesEnum
        {
            public const string Value = "MQGSDI";
            public const string Description = "Gasto por Día";
        }

        public class MinorExpenses : TravelExpMovTypesEnum
        {
            public const string Value = "MQCPME";
            public const string Description = "Compras menores";
        }

        public static bool IsTravelExpensesType(TravelExpMovTypesEnum item)
        {
            if (item is TravelExpMovTypesEnum.TravelExpenses)
                return true;

            return false;
        }

        public static bool IsDayExpensesType(TravelExpMovTypesEnum item)
        {
            if (item is TravelExpMovTypesEnum.DayExpenses)
                return true;

            return false;
        }

        public static bool IsMinorExpensesType(TravelExpMovTypesEnum item)
        {
            if (item is TravelExpMovTypesEnum.MinorExpenses)
                return true;

            return false;
        }

        public static TravelExpMovTypesEnum GetEnum(string pStrValue)
        {
            switch (pStrValue)
            {
                case TravelExpMovTypesEnum.TravelExpenses.Value:
                    return new TravelExpMovTypesEnum.TravelExpenses();
                case TravelExpMovTypesEnum.DayExpenses.Value:
                    return new TravelExpMovTypesEnum.DayExpenses();
                case TravelExpMovTypesEnum.MinorExpenses.Value:
                    return new TravelExpMovTypesEnum.MinorExpenses();
                default:
                    throw new ArgumentException(string.Format("No se encontró el tipo de viático: {0}", pStrValue));
            }
        }
    }
}
