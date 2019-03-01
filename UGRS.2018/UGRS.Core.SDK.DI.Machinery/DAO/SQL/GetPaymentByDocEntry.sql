SELECT DocDate, U_GLO_CodeMov, DocEntry, DocNum, 
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
FROM OVPM WHERE DocEntry = '{DocEntry}'