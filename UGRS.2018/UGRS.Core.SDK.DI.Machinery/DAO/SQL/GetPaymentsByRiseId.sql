SELECT DocDate, U_GLO_CodeMov, DocEntry, DocNum, U_GLO_FolioOri AS RiseId, ISNULL(T2.Code, '') AS Code,
CASE
    WHEN CANCELED = 'Y'
		THEN 2
    ELSE 1
END AS "Status",
CASE 
	WHEN DocTotalFC = 0 
		THEN DocTotal 
	ELSE DocTotalFC
END AS Total
FROM OVPM
LEFT JOIN [@UG_TBL_MQ_RITE] T2
	ON T2.U_DocEntry = OVPM.DocEntry
WHERE U_GLO_FolioOri = '{RiseId}' AND U_GLO_ObjType = '{ObjType}'