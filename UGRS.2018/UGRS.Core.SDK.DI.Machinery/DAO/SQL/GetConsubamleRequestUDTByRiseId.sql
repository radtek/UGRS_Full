SELECT DISTINCT T1.Code, T2.DocDate, T1.U_DocEntry AS DocEntry, T2.DocNum, T1.U_IdRise AS FolioRise, ISNULL(T3.DocEntry, 0) AS StockTransferDocEntry 
FROM [@UG_TBL_MQ_RICM] T1
INNER JOIN OWTQ T2
	ON T1.U_DocEntry = T2.DocEntry AND T2.CANCELED = 'N'
LEFT JOIN WTR1 T3
	ON T1.U_DocEntry = T3.BaseEntry
WHERE T1.U_IdRise = '{RiseId}'