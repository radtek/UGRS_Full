SELECT DISTINCT T1.DocDate, T1.DocEntry, T1.DocNum, T1.U_MQ_Rise AS FolioRise, ISNULL(T2.DocEntry, 0) AS StockTransferDocEntry, ISNULL(T3.Code, '') AS Code
FROM OWTQ T1
LEFT JOIN WTR1 T2
	ON T1.DocEntry = T2.BaseEntry
LEFT JOIN [@UG_TBL_MQ_RICM] T3
	ON T3.U_DocEntry = T1.DocEntry
WHERE T1.U_MQ_Rise = '{RiseId}' AND T1.CANCELED = 'N' 