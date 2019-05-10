
select 
   T0.U_GLO_Auxiliar as CardCode, max(T2.CardName) as CardName, sum(Credit) -sum(Debit) as Venta, max(T1.Invoice) Invoice, 
   max(T3.U_Value) as AccountD
from JDT1 T0
left join (
	select sum(A0.Balance) Invoice, A0.CardCode
	from
	(select  B0.CardCode, MAX(case when B0.DocTotalFC > 0 then B0.DocTotalFC else B0.DocTotal end) - MAX(B0.PaidToDate) as Balance
	from OINV B0 
	inner join INV1 B1 on B1.DocEntry = B0.DocEntry
	where B0.DocStatus = 'O' and B0.CANCELED = 'N' and 
	(B1.OcrCode = '{OcrCode}' or '{CYC}' = 'Y') 
	group by B0.CardCode, B0.DocEntry) A0
	group by A0.CardCode
) T1
on T1.CardCode = T0.U_GLO_Auxiliar
inner join [@UG_CONFIG] T3 on Name =Case when (select U_Location from [@UG_SU_AUTN] where U_Folio = '{Folio}') = 'SU_OBREG' THEN 'SU_VENDEDORSS' ELSE 'SU_VENDEDOR' END and T3.U_Value = T0.Account
inner join OCRD T2 on T2.CardCode = T0.U_GLO_Auxiliar
where  U_SU_Folio = '{Folio}' and isnull(T1.Invoice,0) >0 
group by T0.U_GLO_Auxiliar,t2.CardName
having sum(Credit - Debit) > 0
order by t2.CardName
