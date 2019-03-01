SELECT DocStatus, DocEntry, DocNum, DocDate, DocDueDate, DocCur, DocTotal, DocTotalFC, PaidToDate, (DocTotal-PaidToDate) as DocRemaining, OcrCode, CardCode
FROM ODPI T0
CROSS APPLY
	(
	SELECT TOP 1 OcrCode
	FROM DPI1
	WHERE DPI1.DocEntry = T0.DocEntry
	) DPI1
WHERE CardCode = '{CardCode}' AND DocStatus = 'O'