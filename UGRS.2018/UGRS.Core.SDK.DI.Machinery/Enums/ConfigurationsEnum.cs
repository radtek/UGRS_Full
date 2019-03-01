using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.Enums
{
    public enum ConfigurationsEnum : int
    {
        [Description("MQ_PRICEBOMBA")]
        BombPrice = 1,
        [Description("MQ_MINORPURCHASES")]
        MinorExpenses = 2,
        [Description("MQ_VIATIC")]
        ViaticExpenses = 3,
        [Description("MQ_EXPNSESDAY")]
        DayExpenses = 4,
        [Description("MQ_CTAREEMBOLSO")]
        DeudoresViaticos = 5,
        [Description("MQ_VIATICPRICE")]
        ViaticPrice = 6,
        [Description("MQ_EXPNSESDAYPRICE")]
        DayExpensesPrice = 7,
        [Description("MQ_GOODISSUEACCT")]
        GoodIssueAccount = 8,
        [Description("MQ_GOODISSUEWAREHOUSECODE")]
        GoodIssueCostingCode = 9,
        [Description("MQ_COMMISSIONSACCT")]
        CommissionAccount = 10,
        [Description("MQ_OPR_COMMISSIONRATE")]
        OperatorCommisionRate = 11,
        [Description("MQ_SUP_COMMISSIONRATE")]
        SupervisorCommisionRate = 12,
        [Description("MQ_CAJAACCOUNT")]
        CashRegisterAccount = 13,
    }
}
