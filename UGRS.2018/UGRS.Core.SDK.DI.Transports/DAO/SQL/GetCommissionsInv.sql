select T0.DocEntry, T0.DocDate, T0.DocNum, 'FE' as OpType, T1.U_TR_Paths, T1.U_TR_VehicleType, T1.U_TR_LoadType, T1.LineTotal,

 T1.SlpCode,T2.SlpName, T4.ItemName,T1.LineTotal * (T5.U_Commission/100) as U_Commission,

ISNULL((select A1.LineTotal from INV1 A1 where A1.DocEntry = T1.DocEntry and A1.ItemCode =  (select U_Value from [@UG_CONFIG] where Name = 'TR_INSURANCE')), 0) as U_Seguro


 from OINV T0 with (nolock)
inner join INV1 T1 with (nolock) on T0.DocEntry = T1.DocEntry
inner join OSLP T2 with (nolock) on T1.SlpCode = T2.SlpCode
inner join OITM T4 with (nolock) on T4.ItemCode = T1.ItemCode
inner join OITB T3 with (nolock) on T3.ItmsGrpCod = T4.ItmsGrpCod
inner join [@UG_TR_VETY] T5 with (nolock) on T5.U_EquipType = T1.U_TR_VehicleType
inner join OHEM T6 with (nolock) on T6.SalesPrson = T1.SlpCode
where T0.DocDate between '{DateStart}' and '{DateEnd}'
and (T3.ItmsGrpNam = 'FLETES') 
and T6.empId ='{DriverId}'