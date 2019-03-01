select T2.DocEntry, T2.CardCode, T2.CardName, T2.DocNum, Name as Cert,
sum(T3.Quantity) as HeadsInv, 
sum(U_Quantity) as HeadsExp, (sum(T3.Quantity) - sum(U_Quantity)) as HeadsNo, T2.Doctotal, T2.PaidToDate
from [@UG_CU_CERT] T0
inner join OINV T2 on T0.Name = T2.U_PE_Certificate
inner join Inv1 T3 on T3.DocEntry = T2.DocEntry
where   CANCELED = 'N' and (T0.U_CreditNote is null or T0.U_CreditNote != 'Y')
group by T2.DocEntry, T2.CardCode, T2.CardName, T2.DocNum, Name, T2.DocTotal,  T2.PaidToDate
having sum(U_Quantity) > 0 and (sum(T3.Quantity) - sum(U_Quantity)) > 0
