SELECT DocStatus, DocEntry, DocNum, DocDate, DocDueDate, DocCur, DocTotal, DocTotalFC, PaidToDate, OcrCode, CardCode
FROM OINV T0
CROSS APPLY
	(
	SELECT TOP 1 OcrCode
	FROM INV1
	WHERE INV1.DocEntry = T0.DocEntry
	) INV1
WHERE DocStatus = 'O' AND CardCode = '{CardCode}'