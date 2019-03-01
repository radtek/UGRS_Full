

	
	select T0.DocEntry, T0.DocNum, T0.DocDate, DATEDIFF(day, T0.DocDueDate, GetDate()) as Days, T1.OcrCode, max(case when t0.DocTotalFC > 0 then t0.DocTotalFC else t0.DocTotal end) - max(t0.PaidToDate) as Balance from OINV T0 
	inner join INV1 T1 on T1.DocEntry = T0.DocEntry
	--left join PDF2 T2 on T2.DocEntry != T0.DocEntry and T2.InvType = T0.ObjType
	--left join OPDF T3 on T3.DocEntry = T2.DocNum and T3.Canceled = 'N'
	where   T0.CardCode = '{CardCode}' and DocStatus = 'O' and T0.CANCELED = 'N'  and T1.OcrCode = '{OcrCode}' --T2.DocEntry is null and T3.DocEntry is null and
	group by T0.DocEntry, T0.DocNum, T0.DocDate, T1.OcrCode, DATEDIFF(day, T0.DocDueDate, GetDate()), T1.OcrCode

