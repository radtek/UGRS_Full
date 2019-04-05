select T1.ItemCode, T2.ItemName, T0.U_GLO_Ticket, T0.U_TR_Shared, T1.U_TR_LoadType, T1.U_TR_VehicleType, 
T1.U_TR_Paths, T3.SlpName, T1.OcrCode2, T1.U_TR_TotKm, T1.U_TR_AdditionalExpense, T1.U_TR_TypeA, T1.U_TR_TypeB, T1.U_TR_TypeC, 
T1.U_TR_TypeD, T1.U_TR_TypeE, T1.U_TR_TypeF, U_TR_Heads, U_TR_TotKilos, U_GLO_BagsBales, T1.Price
from OINV T0 with (Nolock)
inner join INV1 T1 with (Nolock) on T1.DocEntry = T0.DocEntry
inner join OITM T2 with (Nolock) on T2.ItemCode = T1.ItemCode
inner join OSLP T3 with (Nolock) on T3.SlpCode = T1.SlpCode
where DocNum = '{DocNum}' and U_GLO_Ticket ='{Ticket}'
