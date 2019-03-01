SELECT DocEntry, DocNum, TransId, T0.Series, T3.SeriesName, DocDate, DocDueDate, DocCur, DocTotal, DocTotalFc, CardCode, ObjType,
OcrCode, T0.UserSign, T2.U_GLO_CashRegister
FROM ODRF T0
CROSS APPLY
	(
	SELECT TOP 1 OcrCode
	FROM DRF1 T1
	WHERE T1.DocEntry = T0.DocEntry
	) DRF1
INNER JOIN OPRC T2 ON T2.PrcCode = OcrCode
LEFT JOIN NNM1 T3 ON T3.Series = T0.Series
WHERE DocStatus = 'O' AND CardCode = '{CardCode}'