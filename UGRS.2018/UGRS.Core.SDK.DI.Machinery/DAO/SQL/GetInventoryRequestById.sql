SELECT DISTINCT T1.DocDate, T1.DocEntry, T1.DocNum, T1.U_MQ_Rise AS FolioRise, ISNULL(T2.DocEntry, 0) AS StockTransferDocEntry 
FROM OWTQ T1
LEFT JOIN WTR1 T2
	ON T1.DocEntry = T2.BaseEntry
WHERE T1.DocEntry = '{DocEntry}'