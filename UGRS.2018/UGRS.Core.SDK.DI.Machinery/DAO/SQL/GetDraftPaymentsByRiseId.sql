SELECT DocDate, U_GLO_CodeMov, DocEntry, DocNum, U_GLO_FolioOri AS RiseId, 
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
FROM OPDF WHERE U_GLO_FolioOri = '{RiseId}' AND U_GLO_ObjType = '{ObjType}' AND Canceled = 'N'