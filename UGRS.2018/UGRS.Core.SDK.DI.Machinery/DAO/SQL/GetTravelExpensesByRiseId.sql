SELECT T2.DocDate, T2.U_GLO_CodeMov, T1.U_DocEntry AS DocEntry, T2.DocNum, T1.U_IdRise AS RiseId, T1.Code, 
CASE
    WHEN T2.CANCELED = 'Y'
		THEN 2
    ELSE 1
END AS "Status",
CASE 
	WHEN T2.DocTotalFC = 0 
		THEN T2.DocTotal 
	ELSE T2.DocTotalFC
END AS Total
FROM [@UG_TBL_MQ_RITE] T1
INNER JOIN OVPM T2
	ON T1.U_DocEntry = T2.DocEntry
WHERE T1.U_IdRise = '{RiseId}'