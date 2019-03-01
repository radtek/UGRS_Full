SELECT T0.DocEntry, T0.DocNum, T0.Series, T4.SeriesName, T0.CardCode, T0.DocDate, T0.DocDueDate, T0.DocTotal, T0.DocTotalFC, T0.TransId, 
DocCur, DocStatus, PaidToDate, (T0.DocTotal-PaidToDate-ISNULL(T3.CheckSum, 0)) as DocRemaining, T1.OcrCode, T0.CardCode, T0.ObjType, T3.CheckSum
FROM OINV T0
CROSS APPLY
	(
	SELECT TOP 1 OcrCode
	FROM INV1 T1
	WHERE T1.DocEntry = T0.DocEntry
	) T1
LEFT JOIN PDF2 T2 ON T2.DocEntry = T0.DocEntry
LEFT JOIN OPDF T3 ON T3.DocEntry = T2.DocNum
LEFT JOIN NNM1 T4 ON T4.Series = T0.Series
WHERE ((T0.DocTotal-PaidToDate-ISNULL(T3.CheckSum, 0))) > 0 AND DocStatus LIKE '{DocStatus}' AND T0.CardCode = '{CardCode}'